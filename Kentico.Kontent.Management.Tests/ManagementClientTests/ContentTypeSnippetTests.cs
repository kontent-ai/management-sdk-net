using Kentico.Kontent.Management.Exceptions;
using Kentico.Kontent.Management.Extenstions;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Types.Elements;
using Kentico.Kontent.Management.Models.TypeSnippets;
using Kentico.Kontent.Management.Models.TypeSnippets.Patch;
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
    [Trait("ManagementClient", "Snippets")]
    public class ContentTypeSnippetTests
    {
        private readonly ManagementClient _client;
        private readonly Scenario _scenario;

        public ContentTypeSnippetTests(ITestOutputHelper output)
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
        public async void ListSnippet_WithContinuation_ListsSnippet()
        {
            var response = await _client.ListContentTypeSnippetsAsync();
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
        public async void GetSnippet_ById_GetsSnippet()
        {
            var identifier = Reference.ById(EXISTING_SNIPPET_ID);

            var response = await _client.GetContentTypeSnippetAsync(identifier);
            Assert.Equal(EXISTING_SNIPPET_ID, response.Id);
        }

        [Fact]
        public async void GetSnippet_ByCodename_GetsSnippet()
        {
            var identifier = Reference.ByCodename(EXISTING_SNIPPET_CODENAME);

            var response = await _client.GetContentTypeSnippetAsync(identifier);
            Assert.Equal(EXISTING_SNIPPET_CODENAME, response.Codename);
        }

        [Fact]
        public async void GetSnippet_ByExternalId_GetsSnippet()
        {
            var externalId = "baf884be-531f-441f-ae88-64205efdd0f6";

            var identifier = Reference.ByExternalId(externalId);

            var response = await _client.GetContentTypeSnippetAsync(identifier);
            Assert.Equal(externalId, response.ExternalId);
        }

        [Fact]
        public async void DeleteSnippet_ById_DeletesSnippet()
        {
            var responseType = await CreateSnippet();

            var identifier = Reference.ById(responseType.Id);
            var exception = await Record.ExceptionAsync(async () => await _client.DeleteContentTypeSnippetAsync(identifier));

            if (RunType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await _client.DeleteContentTypeSnippetAsync(identifier));
            }

            Assert.Null(exception);
        }

        [Fact]
        public async void DeleteSnippet_ByCodename_DeletesSnippet()
        {
            var responseType = await CreateSnippet();

            var identifier = Reference.ByCodename(responseType.Codename);
            var exception = await Record.ExceptionAsync(async () => await _client.DeleteContentTypeSnippetAsync(identifier));


            if (RunType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await _client.DeleteContentTypeSnippetAsync(identifier));
            }

            Assert.Null(exception);
        }

        [Fact]
        public async void DeleteSnippet_ByExternalId_DeletesSnippet()
        {
            var responseType = await CreateSnippet();

            var identifier = Reference.ByExternalId(responseType.ExternalId);
            var exception = await Record.ExceptionAsync(async () => await _client.DeleteContentTypeSnippetAsync(identifier));


            if (RunType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await _client.DeleteContentTypeSnippetAsync(identifier));
            }

            Assert.Null(exception);
        }

        [Fact]
        public async void CreateSnippet_CreatesSnippet()
        {
            var typeName = "HoorayType!";
            var typeCodename = "hooray_codename_type";
            var typeExternalId = "hooray_codename_external_id";
            var type = new CreateContentSnippetCreateModel
            {
                Name = typeName,
                Codename = typeCodename,
                ExternalId = typeExternalId,
                Elements = _elementMetadataForSnippets
            };

            var responseType = await _client.CreateContentTypeSnippetAsync(type);

            Assert.Equal(typeName, responseType.Name);
            Assert.Equal(typeCodename, responseType.Codename);
            Assert.Equal(typeExternalId, responseType.ExternalId);

            // Cleanup
            var typeToClean = Reference.ByCodename(typeCodename);
            await _client.DeleteContentTypeSnippetAsync(typeToClean);
        }

        [Fact]
        public async void ModifySnippet_AddInto_ModifiesSnippet()
        {
            //Arrange
            var responseType = await CreateSnippet();

            var elementExternalId = "snippet_external_id2_patchaddinto";
            var textName = "snippetName2";
            var changes = new ContentTypeSnippetAddIntoPatchModel
            {
                Value = new TextElementMetadataModel
                {
                    ExternalId = "snippet_external_id2_patchaddinto",
                    Guidelines = "snippetGuidelines2",
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
            var modifiedType = await _client.ModifyContentTypeSnippetAsync(Reference.ByCodename(responseType.Codename), new List<ContentTypeSnippetOperationBaseModel> { changes });


            //assert
            var addedElement = modifiedType.Elements.FirstOrDefault(x => x.ExternalId == elementExternalId).ToElement<TextElementMetadataModel>();
            Assert.NotNull(addedElement);
            Assert.Equal(addedElement.Name, textName);


            // Cleanup
            var typeToClean = Reference.ByCodename(responseType.Codename);
            await _client.DeleteContentTypeSnippetAsync(typeToClean);
        }

        [Fact]
        public async void ModifySnippet_Replace_ModifiesSnippet()
        {
            //arrange
            var responseType = await CreateSnippet();

            var expectedValue = "<h1>Here you can tell users how to fill in the element.</h1>";

            var changes = new ContentTypeSnippetPatchReplaceModel
            {
                Value = expectedValue,
                Path = $"/elements/codename:{responseType.Elements.First().Codename}/guidelines"
            };


            //Act
            var modifiedType = await _client.ModifyContentTypeSnippetAsync(Reference.ByCodename(responseType.Codename), new List<ContentTypeSnippetOperationBaseModel> { changes });


            //Assert
            Assert.Equal(expectedValue, 
                modifiedType.Elements.FirstOrDefault(x => x.Codename == responseType.Elements.First().Codename)?.ToElement<GuidelinesElementMetadataModel>().Guidelines);


            // Cleanup
            var typeToClean = Reference.ByCodename(responseType.Codename);
            await _client.DeleteContentTypeSnippetAsync(typeToClean);
        }

        [Fact]
        public async void ModifySnippet_Remove_ModifiesSnippet()
        {
            //arrange
            

            var responseType = await CreateSnippet();

            var changes = new SnippetPatchRemoveModel
            {
                Path = $"/elements/codename:{responseType.Elements.First().Codename}"
            };


            //Act
            var modifiedType = await _client.ModifyContentTypeSnippetAsync(Reference.ByCodename(responseType.Codename), new List<ContentTypeSnippetOperationBaseModel> { changes });


            //Assert
            Assert.Null(modifiedType.Elements.FirstOrDefault(x => x.Codename == responseType.Elements.First().Codename));


            // Cleanup
            var typeToClean = Reference.ByCodename(responseType.Codename);
            await _client.DeleteContentTypeSnippetAsync(typeToClean);
        }

        private async Task<ContentTypeSnippetModel> CreateSnippet([CallerMemberName] string memberName = "")
        {
            var suffix = $"{memberName.ToLower().Substring(0, Math.Min(40, memberName.Length))}";

            var type = new CreateContentSnippetCreateModel
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

            return await _client.CreateContentTypeSnippetAsync(type);
        }

        private List<ElementMetadataBase> _elementMetadataForSnippets =>
            new List<ElementMetadataBase>(ElementMetadata.RemoveAll(x => x.GetType() == typeof(UrlSlugElementMetadataModel) ||  x.GetType() == typeof(ContentTypeSnippetElementMetadataModel)));
    }
}
