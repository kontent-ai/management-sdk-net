using Kentico.Kontent.Management.Exceptions;
using Kentico.Kontent.Management.Extenstions;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Types;
using Kentico.Kontent.Management.Models.Types.Elements;
using Kentico.Kontent.Management.Models.Types.Patch;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    partial class ManagementClientTests
    {
        [Fact]
        [Trait("Category", "ContentType")]
        public async void ListContentTypes_ListsContentTypes()
        {
            var client = CreateManagementClient();

            var response = await client.ListContentTypesAsync();

            Assert.NotNull(response);
            Assert.NotNull(response.FirstOrDefault());
        }

        [Fact]
        [Trait("Category", "ContentType")]
        //Todo
        //does not really test pagination as the default page size is 50 items 
        //same applies to content item test (where is page size 100)
        public async void ListContentTypes_WithContinuation_ListsContentTypes()
        {
            var client = CreateManagementClient();

            var response = await client.ListContentTypesAsync();
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
        [Trait("Category", "ContentType")]
        public async void GetContentType_ById_GetsContentType()
        {
            var client = CreateManagementClient();

            var identifier = Reference.ById(EXISTING_CONTENT_TYPE_ID);

            var response = await client.GetContentTypeAsync(identifier);
            Assert.Equal(EXISTING_CONTENT_TYPE_ID, response.Id);
        }

        [Fact]
        [Trait("Category", "ContentType")]
        public async void GetContentType_ByCodename_GetsContentType()
        {
            var client = CreateManagementClient();

            var identifier = Reference.ByCodename(EXISTING_CONTENT_TYPE_CODENAME);

            var response = await client.GetContentTypeAsync(identifier);
            Assert.Equal(EXISTING_CONTENT_TYPE_CODENAME, response.Codename);
        }

        [Fact]
        [Trait("Category", "ContentType")]
        public async void GetContentType_ByExternalId_GetsContentType()
        {
            var externalId = "b7aa4a53-d9b1-48cf-b7a6-ed0b182c4b89";

            var client = CreateManagementClient();

            var identifier = Reference.ByExternalId(externalId);

            var response = await client.GetContentTypeAsync(identifier);
            Assert.Equal(externalId, response.ExternalId);
        }

        [Fact]
        [Trait("Category", "ContentType")]
        public async void DeleteContentType_ById_DeletesContentType()
        {
            var client = CreateManagementClient();

            var responseType = await CreateContentType(client);

            var identifier = Reference.ById(responseType.Id);
            var exception = await Record.ExceptionAsync(async () => await client.DeleteContentTypeAsync(identifier));

            if (_runType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await client.DeleteContentTypeAsync(identifier));
            }

            Assert.Null(exception);
        }

        [Fact]
        [Trait("Category", "ContentType")]
        public async void DeleteContentType_ByCodename_DeletesContentType()
        {
            var client = CreateManagementClient();

            var responseType = await CreateContentType(client);

            var identifier = Reference.ByCodename(responseType.Codename);
            var exception = await Record.ExceptionAsync(async () => await client.DeleteContentTypeAsync(identifier));


            if (_runType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await client.DeleteContentTypeAsync(identifier));
            }

            Assert.Null(exception);
        }

        [Fact]
        [Trait("Category", "ContentType")]
        public async void DeleteContentType_ByExternalId_DeletesContentType()
        {
            var client = CreateManagementClient();

            var responseType = await CreateContentType(client);

            var identifier = Reference.ByExternalId(responseType.ExternalId);
            var exception = await Record.ExceptionAsync(async () => await client.DeleteContentTypeAsync(identifier));


            if (_runType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await client.DeleteContentTypeAsync(identifier));
            }

            Assert.Null(exception);
        }

        [Fact]
        [Trait("Category", "ContentType")]
        //Todo create more elements
        public async void CreateContentType_CreatesContentType()
        {
            var client = CreateManagementClient();

            var typeName = "HoorayType!";
            var typeCodename = "hooray_codename_type";
            var typeExternalId = "hooray_codename_external_id";
            var type = new ContentTypeCreateModel
            {
                Name = typeName,
                Codename = typeCodename,
                ExternalId = typeExternalId,
                Elements = new List<ElementMetadataBase>
                {
                    new GuidelinesElementMetadataModel
                    {
                        Codename = "guidelines_codename",
                        ExternalId = "guidelines_external_id",
                        Guidelines = "<h3>Guidelines</h3>"
                    }
                }
            };

            var responseType = await client.CreateContentTypeAsync(type);

            Assert.Equal(typeName, responseType.Name);
            Assert.Equal(typeCodename, responseType.Codename);
            Assert.Equal(typeExternalId, responseType.ExternalId);

            // Cleanup
            var typeToClean = Reference.ByCodename(typeCodename);
            await client.DeleteContentTypeAsync(typeToClean);
        }

        [Fact]
        [Trait("Category", "ContentType")]
        public async void ModifyContentType_AddInto_ModifiesContentType()
        {
            //Arrange
            var client = CreateManagementClient();

            var responseType = await CreateContentType(client);

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
            var modifiedType = await client.ModifyContentTypeAsync(Reference.ByCodename(responseType.Codename), new List<ContentTypeOperationBaseModel> { changes });


            //assert
            var addedElement = modifiedType.Elements.FirstOrDefault(x => x.Codename == elementCodename).ToTextElement();
            Assert.NotNull(addedElement);
            Assert.Equal(addedElement.Name, textName);


            // Cleanup
            var typeToClean = Reference.ByCodename(responseType.Codename);
            await client.DeleteContentTypeAsync(typeToClean);
        }

        [Fact]
        [Trait("Category", "ContentType")]
        public async void ModifyContentType_Replace_ModifiesContentType()
        {
            //arrange
            //todo extract creation of type to method
            var client = CreateManagementClient();

            var responseType = await CreateContentType(client);

            var expectedValue = "<h1>Here you can tell users how to fill in the element.</h1>";

            var changes = new ContentTypeReplacePatchModel
            {
                Value = expectedValue,
                After = Reference.ByCodename(responseType.Elements.First().Codename),
                Path = $"/elements/codename:{responseType.Elements.First().Codename}/guidelines"
            };


            //Act
            var modifiedType = await client.ModifyContentTypeAsync(Reference.ByCodename(responseType.Codename), new List<ContentTypeOperationBaseModel> { changes });


            //Assert
            Assert.Equal(expectedValue, modifiedType.Elements.FirstOrDefault(x => x.Codename == responseType.Elements.First().Codename)?.ToGuidelines().Guidelines);


            // Cleanup
            var typeToClean = Reference.ByCodename(responseType.Codename);
            await client.DeleteContentTypeAsync(typeToClean);
        }

        [Fact]
        [Trait("Category", "ContentType")]
        public async void ModifyContentType_Remove_ModifiesContentType()
        {
            //arrange
            var client = CreateManagementClient();

            var responseType = await CreateContentType(client);

            var changes = new ContentTypeRemovePatchModel
            {
                Path = $"/elements/codename:{responseType.Elements.First().Codename}"
            };


            //Act
            var modifiedType = await client.ModifyContentTypeAsync(Reference.ByCodename(responseType.Codename), new List<ContentTypeOperationBaseModel> { changes });


            //Assert
            Assert.Null(modifiedType.Elements.FirstOrDefault(x => x.Codename == responseType.Elements.First().Codename));


            // Cleanup
            var typeToClean = Reference.ByCodename(responseType.Codename);
            await client.DeleteContentTypeAsync(typeToClean);
        }

        private async Task<ContentTypeModel> CreateContentType(ManagementClient client, [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            var suffix = $"{memberName.ToLower().Substring(0,40)}_{sourceLineNumber:d}";

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

            return await client.CreateContentTypeAsync(type);
        }
    }
}
