using Kentico.Kontent.Management.Exceptions;
using Kentico.Kontent.Management.Extenstions;
using Kentico.Kontent.Management.Models;
using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.Types;
using Kentico.Kontent.Management.Models.Types.Elements;
using Kentico.Kontent.Management.Models.Types.Patch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    partial class ManagementClientTests
    {
        #region Type

        [Fact]
        [Trait("Category", "ContentType")]
        public async void ListContentTypes_ListsContentTypes()
        {
            var client = CreateManagementClient(nameof(ListContentTypes_ListsContentTypes));

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
            var client = CreateManagementClient(nameof(ListContentTypes_WithContinuation_ListsContentTypes));

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
            var client = CreateManagementClient(nameof(GetContentType_ById_GetsContentType));

            var identifier = ContentTypeIdentifier.ById(EXISTING_CONTENT_TYPE_ID);

            var response = await client.GetContentTypeAsync(identifier);
            Assert.Equal(EXISTING_CONTENT_TYPE_ID, response.Id);
        }

        [Fact]
        [Trait("Category", "ContentType")]
        public async void GetContentType_ByCodename_GetsContentType()
        {
            var client = CreateManagementClient(nameof(GetContentType_ByCodename_GetsContentType));

            var identifier = ContentTypeIdentifier.ByCodename(EXISTING_CONTENT_TYPE_CODENAME);

            var response = await client.GetContentTypeAsync(identifier);
            Assert.Equal(EXISTING_CONTENT_TYPE_CODENAME, response.Codename);
        }

        [Fact]
        [Trait("Category", "ContentType")]
        public async void GetContentType_ByExternalId_GetsContentType()
        {
            var externalId = "b7aa4a53-d9b1-48cf-b7a6-ed0b182c4b89";

            var client = CreateManagementClient(nameof(GetContentType_ByExternalId_GetsContentType));

            var identifier = ContentTypeIdentifier.ByExternalId(externalId);

            var response = await client.GetContentTypeAsync(identifier);
            Assert.Equal(externalId, response.ExternalId);
        }

        [Fact]
        [Trait("Category", "ContentType")]
        public async void DeleteContentType_ById_DeletesContentType()
        {
            var client = CreateManagementClient(nameof(DeleteContentType_ById_DeletesContentType));

            var typeName = "TestDeleteById!";
            var typeCodename = "test_delete_id";
            var typeExternalId = "test_delete_externalId_id";
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
                        ExternalId = "guidelines_test_delete_id",
                        Guidelines = "<h3>Guidelines</h3>"
                    }
                }
            };

            var responseType = await client.CreateContentTypeAsync(type);


            var identifier = ContentTypeIdentifier.ById(responseType.Id);
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
            var client = CreateManagementClient(nameof(DeleteContentType_ByCodename_DeletesContentType));

            var typeName = "TestDeleteByCodename!";
            var typeCodename = "test_delete_codename";
            var typeExternalId = "test_delete_externalId_codename";
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
                        ExternalId = "guidelines_test_delete_codename",
                        Guidelines = "<h3>Guidelines</h3>"
                    }
                }
            };

            var responseType = await client.CreateContentTypeAsync(type);


            var identifier = ContentTypeIdentifier.ByCodename(typeCodename);
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
            var client = CreateManagementClient(nameof(DeleteContentType_ByExternalId_DeletesContentType));

            var typeName = "TestDeleteByExternalId!";
            var typeCodename = "test_delete_externalid";
            var typeExternalId = "test_delete_externalId_externalid";
            var type = new ContentTypeCreateModel
            {
                Name = typeName,
                Codename = typeCodename,
                ExternalId = typeExternalId,
                Elements = new List<ElementMetadataBase>
                {
                    new GuidelinesElementMetadataModel
                    {
                        Codename = "guidelines_externalid",
                        ExternalId = "guidelines_test_delete_externalid",
                        Guidelines = "<h3>Guidelines</h3>"
                    }
                }
            };

            var responseType = await client.CreateContentTypeAsync(type);


            var identifier = ContentTypeIdentifier.ByExternalId(typeExternalId);
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
            var client = CreateManagementClient(nameof(CreateContentType_CreatesContentType));

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
            var typeToClean = ContentTypeIdentifier.ByCodename(typeCodename);
            await client.DeleteContentTypeAsync(typeToClean);
        }

        [Fact]
        [Trait("Category", "ContentType")]
        public async void ModifyContentType_AddInto_ModifiesContentType()
        {
            //Arrange
            var client = CreateManagementClient(nameof(ModifyContentType_AddInto_ModifiesContentType));

            var typeCodename = "patch_codename_add_into";
            var type = new ContentTypeCreateModel
            {
                Name = "PatchTypeAddInto!",
                Codename = typeCodename,
                ExternalId = "patchAddInto_external_id",
                Elements = new List<ElementMetadataBase>
                {
                    new GuidelinesElementMetadataModel
                    {
                        Codename = "guidelines_codename_patchaddinto",
                        ExternalId = "guidelines_external_id_patchaddinto",
                        Guidelines = "<h3>Guidelines</h3>"
                    },
                    new TextElementMetadataModel
                    {
                        Codename = "text_codename_patchaddinto",
                        ExternalId = "text_external_id_patchaddinto",
                        Guidelines = "Guidelines",
                        Name = "textName",
                        IsRequired = false,
                        MaximumTextLength = new MaximumTextLengthModel
                        {
                            AppliesTo = TextLengthLimitType.Words,
                            Value = 30
                        }
                    },
                }
            };

            _ = await client.CreateContentTypeAsync(type);

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
                Before = Reference.ByCodename("guidelines_codename_patchaddinto"),
                Path = "/elements"
            };


            //act
            var modifiedType = await client.ModifyContentTypeAsync(ContentTypeIdentifier.ByCodename(typeCodename), new List<ContentTypeOperationBaseModel> { changes });


            //assert
            var addedElement = modifiedType.Elements.FirstOrDefault(x => x.Codename == elementCodename).ToTextElement();
            Assert.NotNull(addedElement);
            Assert.Equal(addedElement.Name, textName);


            // Cleanup
            var typeToClean = ContentTypeIdentifier.ByCodename(typeCodename);
            await client.DeleteContentTypeAsync(typeToClean);
        }

        [Fact]
        [Trait("Category", "ContentType")]
        public async void ModifyContentType_Replace_ModifiesContentType()
        {
            //arrange
            //todo extract creation of type to method
            var client = CreateManagementClient(nameof(ModifyContentType_Replace_ModifiesContentType));

            var typeCodename = "patch_codename_replace";
            var elementCodename = "text_codename_replace";
            var type = new ContentTypeCreateModel
            {
                Name = "PatchTypeReplace!",
                Codename = typeCodename,
                ExternalId = "patch_external_id_replace",
                Elements = new List<ElementMetadataBase>
                {
                    new GuidelinesElementMetadataModel
                    {
                        Codename = "guidelines_codename_replace",
                        ExternalId = "guidelines_external_id_replace",
                        Guidelines = "<h3>Guidelines</h3>"
                    },
                    new TextElementMetadataModel
                    {
                        Codename = elementCodename,
                        ExternalId = "text_external_id_replace",
                        Guidelines = "Guidelines",
                        Name = "textName",
                        IsRequired = false,
                        MaximumTextLength = new MaximumTextLengthModel
                        {
                            AppliesTo = TextLengthLimitType.Words,
                            Value = 30
                        }
                    },
                }
            };

            _ = await client.CreateContentTypeAsync(type);

            var expectedValue = "Here you can tell users how to fill in the element.";

            var changes = new ContentTypePatchReplaceModel
            {
                Value = expectedValue,
                After = Reference.ByCodename("guidelines_codename"),
                Path = $"/elements/codename:{elementCodename}/guidelines"
            };


            //Act
            var modifiedType = await client.ModifyContentTypeAsync(ContentTypeIdentifier.ByCodename(typeCodename), new List<ContentTypeOperationBaseModel> { changes });


            //Assert
            Assert.Equal(expectedValue, modifiedType.Elements.FirstOrDefault(x => x.Codename == elementCodename)?.ToTextElement().Guidelines);


            // Cleanup
            var typeToClean = ContentTypeIdentifier.ByCodename(typeCodename);
            await client.DeleteContentTypeAsync(typeToClean);
        }

        [Fact]
        [Trait("Category", "ContentType")]
        public async void ModifyContentType_Remove_ModifiesContentType()
        {
            //arrange
            var client = CreateManagementClient(nameof(ModifyContentType_Remove_ModifiesContentType));

            var typeCodename = "patch_codename_remove";
            var elementCodename = "text_codename_replace";
            var type = new ContentTypeCreateModel
            {
                Name = "PatchTypeReplace!",
                Codename = typeCodename,
                ExternalId = "patch_external_id_remove",
                Elements = new List<ElementMetadataBase>
                {
                    new GuidelinesElementMetadataModel
                    {
                        Codename = "guidelines_codename_replace",
                        ExternalId = "guidelines_external_id_remove",
                        Guidelines = "<h3>Guidelines</h3>"
                    },
                    new TextElementMetadataModel
                    {
                        Codename = elementCodename,
                        ExternalId = "text_external_id_remove",
                        Guidelines = "Guidelines",
                        Name = "textName",
                        IsRequired = false,
                        MaximumTextLength = new MaximumTextLengthModel
                        {
                            AppliesTo = TextLengthLimitType.Words,
                            Value = 30
                        }
                    },
                }
            };

            _ = await client.CreateContentTypeAsync(type);

            var changes = new ContentTypePatchRemoveModel
            {
                Path = $"/elements/codename:{elementCodename}"
            };


            //Act
            var modifiedType = await client.ModifyContentTypeAsync(ContentTypeIdentifier.ByCodename(typeCodename), new List<ContentTypeOperationBaseModel> { changes });


            //Assert
            Assert.Null(modifiedType.Elements.FirstOrDefault(x => x.Codename == elementCodename));


            // Cleanup
            var typeToClean = ContentTypeIdentifier.ByCodename(typeCodename);
            await client.DeleteContentTypeAsync(typeToClean);
        }
        #endregion
    }
}
