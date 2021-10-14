using Kentico.Kontent.Management.Exceptions;
using Kentico.Kontent.Management.Extenstions;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Types;
using Kentico.Kontent.Management.Models.Types.Elements;
using Kentico.Kontent.Management.Models.Types.Patch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static Kentico.Kontent.Management.Tests.ManagementClientTests.Scenario;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    [Trait("ManagementClient", "ContentType")]
    public class ContentTypeTests
    {
        private readonly ManagementClient _client;
        private readonly Scenario _scenario;

        public ContentTypeTests(ITestOutputHelper output)
        {
            //this magic can be replace once new xunit is delivered
            //https://github.com/xunit/xunit/issues/621
            var type = output.GetType();
            var testMember = type.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
            var test = (ITest)testMember.GetValue(output);

            _scenario = new Scenario(test.TestCase.TestMethod.Method.Name);
            _client = _scenario.Client;
        }

        [Fact]
        public async void ListContentTypes_ListsContentTypes()
        {
            var response = await _client.ListContentTypesAsync();

            Assert.NotNull(response);
            Assert.NotNull(response.FirstOrDefault());
        }

        [Fact]
        public async void ListContentTypes_WithContinuation_ListsContentTypes()
        {
            var response = await _client.ListContentTypesAsync();
            Assert.NotNull(response);

            while (true)
            {
                foreach (var item in response)
                {
                    Assert.NotNull(item);
                }

                if (!response.HasNextPage())
                {
                    break;
                }
                response = await response.GetNextPage();
                Assert.NotNull(response);
            }
        }

        [Fact]
        public async void GetContentType_ById_GetsContentType()
        {
            var identifier = Reference.ById(EXISTING_CONTENT_TYPE_ID);

            var response = await _client.GetContentTypeAsync(identifier);
            Assert.Equal(EXISTING_CONTENT_TYPE_ID, response.Id);
        }

        [Fact]
        public async void GetContentType_ByCodename_GetsContentType()
        {
            var identifier = Reference.ByCodename(EXISTING_CONTENT_TYPE_CODENAME);

            var response = await _client.GetContentTypeAsync(identifier);
            Assert.Equal(EXISTING_CONTENT_TYPE_CODENAME, response.Codename);
        }

        [Fact]
        public async void GetContentType_ByExternalId_GetsContentType()
        {
            var externalId = "b7aa4a53-d9b1-48cf-b7a6-ed0b182c4b89";

            var identifier = Reference.ByExternalId(externalId);

            var response = await _client.GetContentTypeAsync(identifier);
            Assert.Equal(externalId, response.ExternalId);
        }

        [Fact]
        public async void DeleteContentType_ById_DeletesContentType()
        {
            var responseType = await CreateContentType();

            var identifier = Reference.ById(responseType.Id);
            var exception = await Record.ExceptionAsync(async () => await _client.DeleteContentTypeAsync(identifier));

            if (RunType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await _client.DeleteContentTypeAsync(identifier));
            }

            Assert.Null(exception);
        }

        [Fact]
        public async void DeleteContentType_ByCodename_DeletesContentType()
        {
            var responseType = await CreateContentType();

            var identifier = Reference.ByCodename(responseType.Codename);
            var exception = await Record.ExceptionAsync(async () => await _client.DeleteContentTypeAsync(identifier));


            if (RunType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await _client.DeleteContentTypeAsync(identifier));
            }

            Assert.Null(exception);
        }

        [Fact]
        public async void DeleteContentType_ByExternalId_DeletesContentType()
        {
            var responseType = await CreateContentType();

            var identifier = Reference.ByExternalId(responseType.ExternalId);
            var exception = await Record.ExceptionAsync(async () => await _client.DeleteContentTypeAsync(identifier));


            if (RunType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await _client.DeleteContentTypeAsync(identifier));
            }

            Assert.Null(exception);
        }

        [Fact]
        //todo add assert for all elements
        public async void CreateContentType_CreatesContentType()
        {
            var typeName = "HoorayType!";
            var typeCodename = "hooray_codename_type";
            var typeExternalId = "hooray_codename_external_id";

            var elements = new List<ElementMetadataBase>(ElementMetadata);
            elements.ForEach(x => x.ContentGroup = Reference.ByExternalId("contentGroupExternalId"));


            var type = new ContentTypeCreateModel
            {
                Name = typeName,
                Codename = typeCodename,
                ExternalId = typeExternalId,
                Elements = elements,
                ContentGroups = new[]
                {
                    new ContentGroupModel
                    {
                        Name = "contentGroupName",
                        ExternalId = "contentGroupExternalId"
                    }
                }
            };

            var responseType = await _client.CreateContentTypeAsync(type);

            Assert.Equal(typeName, responseType.Name);
            Assert.Equal(typeCodename, responseType.Codename);
            Assert.Equal(typeExternalId, responseType.ExternalId);

            Assert.Contains(
                "yes",
                responseType.Elements
                    .FirstOrDefault(element => element.Codename == "multiple_choice_element_codename")
                    .ToElement<MultipleChoiceElementMetadataModel>().Options
                    .First().Codename
                );

            // Cleanup
            var typeToClean = Reference.ByCodename(typeCodename);
            await _client.DeleteContentTypeAsync(typeToClean);
        }

        [Fact]
        public async void ModifyContentType_AddInto_ModifiesContentType()
        {
            //Arrange
            var responseType = await CreateContentType();

            var elementCodename = "text_codename2_patchaddinto";
            var textName = "textName2";
            var changes = new ContentTypeAddIntoPatchModel
            {
                Value = new TextElementMetadataModel
                {
                    Codename = elementCodename,
                    ExternalId = "text_external_id2_patchaddinto",
                    Guidelines = "Guidelines2",
                    Name = textName,
                    IsRequired = false,
                    MaximumTextLength = new MaximumTextLengthModel
                    {
                        AppliesTo = TextLengthLimitType.Words,
                        Value = 30
                    }
                },
                Before = Reference.ByCodename(responseType.Elements.First().Codename),
                Path = "/elements"
            };


            //act
            var modifiedType = await _client.ModifyContentTypeAsync(Reference.ByCodename(responseType.Codename), new List<ContentTypeOperationBaseModel> { changes });


            //assert
            var addedElement = modifiedType.Elements.FirstOrDefault(x => x.Codename == elementCodename).ToElement<TextElementMetadataModel>();
            Assert.NotNull(addedElement);
            Assert.Equal(addedElement.Name, textName);


            // Cleanup
            var typeToClean = Reference.ByCodename(responseType.Codename);
            await _client.DeleteContentTypeAsync(typeToClean);
        }

        [Fact]
        public async void ModifyContentType_Replace_ModifiesContentType()
        {
            //arrange
            var responseType = await CreateContentType();

            var expectedValue = "<h1>Here you can tell users how to fill in the element.</h1>";

            var changes = new ContentTypeReplacePatchModel
            {
                Value = expectedValue,
                Path = $"/elements/codename:{responseType.Elements.First().Codename}/guidelines"
            };


            //Act
            var modifiedType = await _client.ModifyContentTypeAsync(Reference.ByCodename(responseType.Codename), new List<ContentTypeOperationBaseModel> { changes });


            //Assert
            Assert.Equal(expectedValue,
                modifiedType.Elements.FirstOrDefault(x => x.Codename == responseType.Elements.First().Codename)?.ToElement<GuidelinesElementMetadataModel>().Guidelines);


            // Cleanup
            var typeToClean = Reference.ByCodename(responseType.Codename);
            await _client.DeleteContentTypeAsync(typeToClean);
        }

        [Fact]
        public async void ModifyContentType_Remove_ModifiesContentType()
        {
            //arrange
            var responseType = await CreateContentType();

            var changes = new ContentTypeRemovePatchModel
            {
                Path = $"/elements/codename:{responseType.Elements.First().Codename}"
            };


            //Act
            var modifiedType = await _client.ModifyContentTypeAsync(Reference.ByCodename(responseType.Codename), new List<ContentTypeOperationBaseModel> { changes });


            //Assert
            Assert.Null(modifiedType.Elements.FirstOrDefault(x => x.Codename == responseType.Elements.First().Codename));


            // Cleanup
            var typeToClean = Reference.ByCodename(responseType.Codename);
            await _client.DeleteContentTypeAsync(typeToClean);
        }

        private async Task<ContentTypeModel> CreateContentType([CallerMemberName] string memberName = "")
        {
            var suffix = $"{memberName.ToLower().Substring(0, 40)}_{memberName.ToLower().Substring(40, Math.Min(memberName.Length - 40, 10))}";

            var type = new ContentTypeCreateModel
            {
                Name = $"{suffix}",
                Codename = $"c_{suffix}",
                ExternalId = $"eid_{suffix}",
                Elements = new List<ElementMetadataBase>
                {
                    new GuidelinesElementMetadataModel
                    {
                        Codename = $"g_c_{suffix}",
                        ExternalId = $"g_eid_{suffix}",
                        Guidelines = "<h3>Guidelines</h3>"
                    }
                }
            };

            return await _client.CreateContentTypeAsync(type);
        }
    }
}
