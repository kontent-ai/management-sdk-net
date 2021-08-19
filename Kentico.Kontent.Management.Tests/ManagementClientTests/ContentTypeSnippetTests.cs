using Kentico.Kontent.Management.Exceptions;
using Kentico.Kontent.Management.Extenstions;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Types.Elements;
using Kentico.Kontent.Management.Models.TypeSnippets;
using Kentico.Kontent.Management.Models.TypeSnippets.Patch;
using System;
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
        [Trait("Category", "Snippet")]
        public async void ListSnippet_ListsSnippets()
        {
            var client = CreateManagementClient();

            var response = await client.ListContentTypeSnippetsAsync();

            Assert.NotNull(response);
            Assert.NotNull(response.FirstOrDefault(x => x.Codename == EXISTING_SNIPPET_CODENAME));
        }

        [Fact]
        [Trait("Category", "Snippet")]
        public async void ListSnippet_WithContinuation_ListsSnippet()
        {
            var client = CreateManagementClient();

            var response = await client.ListContentTypeSnippetsAsync();
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
        [Trait("Category", "Snippet")]
        public async void GetSnippet_ById_GetsSnippet()
        {
            var client = CreateManagementClient();

            var identifier = Reference.ById(EXISTING_SNIPPET_ID);

            var response = await client.GetContentTypeSnippetAsync(identifier);
            Assert.Equal(EXISTING_SNIPPET_ID, response.Id);
        }

        [Fact]
        [Trait("Category", "Snippet")]
        public async void GetSnippet_ByCodename_GetsSnippet()
        {
            var client = CreateManagementClient();

            var identifier = Reference.ByCodename(EXISTING_SNIPPET_CODENAME);

            var response = await client.GetContentTypeSnippetAsync(identifier);
            Assert.Equal(EXISTING_SNIPPET_CODENAME, response.Codename);
        }

        [Fact]
        [Trait("Category", "Snippet")]
        public async void GetSnippet_ByExternalId_GetsSnippet()
        {
            var externalId = "baf884be-531f-441f-ae88-64205efdd0f6";

            var client = CreateManagementClient();

            var identifier = Reference.ByExternalId(externalId);

            var response = await client.GetContentTypeSnippetAsync(identifier);
            Assert.Equal(externalId, response.ExternalId);
        }

        [Fact]
        [Trait("Category", "Snippet")]
        public async void DeleteSnippet_ById_DeletesSnippet()
        {
            var client = CreateManagementClient();

            var responseType = await CreateSnippet(client);

            var identifier = Reference.ById(responseType.Id);
            var exception = await Record.ExceptionAsync(async () => await client.DeleteContentTypeSnippetAsync(identifier));

            if (_runType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await client.DeleteContentTypeSnippetAsync(identifier));
            }

            Assert.Null(exception);
        }

        [Fact]
        [Trait("Category", "Snippet")]
        public async void DeleteSnippet_ByCodename_DeletesSnippet()
        {
            var client = CreateManagementClient();

            var responseType = await CreateSnippet(client);

            var identifier = Reference.ByCodename(responseType.Codename);
            var exception = await Record.ExceptionAsync(async () => await client.DeleteContentTypeSnippetAsync(identifier));


            if (_runType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await client.DeleteContentTypeSnippetAsync(identifier));
            }

            Assert.Null(exception);
        }

        [Fact]
        [Trait("Category", "Snippet")]
        public async void DeleteSnippet_ByExternalId_DeletesSnippet()
        {
            var client = CreateManagementClient();

            var responseType = await CreateSnippet(client);

            var identifier = Reference.ByExternalId(responseType.ExternalId);
            var exception = await Record.ExceptionAsync(async () => await client.DeleteContentTypeSnippetAsync(identifier));


            if (_runType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await client.DeleteContentTypeSnippetAsync(identifier));
            }

            Assert.Null(exception);
        }

        [Fact]
        [Trait("Category", "Snippet")]
        //Todo create more elements
        public async void CreateSnippet_CreatesSnippet()
        {
            var client = CreateManagementClient();

            var typeName = "HoorayType!";
            var typeCodename = "hooray_codename_type";
            var typeExternalId = "hooray_codename_external_id";
            var type = new SnippetCreateModel
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

            var responseType = await client.CreateContentTypeSnippetAsync(type);

            Assert.Equal(typeName, responseType.Name);
            Assert.Equal(typeCodename, responseType.Codename);
            Assert.Equal(typeExternalId, responseType.ExternalId);

            // Cleanup
            var typeToClean = Reference.ByCodename(typeCodename);
            await client.DeleteContentTypeSnippetAsync(typeToClean);
        }

        [Fact]
        [Trait("Category", "Snippet")]
        public async void ModifySnippet_AddInto_ModifiesSnippet()
        {
            //Arrange
            var client = CreateManagementClient();

            var responseType = await CreateSnippet(client);

            var elementExternalId = "snippet_external_id2_patchaddinto";
            var textName = "snippetName2";
            var changes = new SnippetAddIntoPatchModel
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
            var modifiedType = await client.ModifyContentTypeSnippetAsync(Reference.ByCodename(responseType.Codename), new List<SnippetOperationBaseModel> { changes });


            //assert
            var addedElement = modifiedType.Elements.FirstOrDefault(x => x.ExternalId == elementExternalId).ToTextElement();
            Assert.NotNull(addedElement);
            Assert.Equal(addedElement.Name, textName);


            // Cleanup
            var typeToClean = Reference.ByCodename(responseType.Codename);
            await client.DeleteContentTypeSnippetAsync(typeToClean);
        }

        [Fact]
        [Trait("Category", "Snippet")]
        public async void ModifySnippet_Replace_ModifiesSnippet()
        {
            //arrange
            //todo extract creation of type to method
            var client = CreateManagementClient();

            var responseType = await CreateSnippet(client);

            var expectedValue = "<h1>Here you can tell users how to fill in the element.</h1>";

            var changes = new SnippetPatchReplaceModel
            {
                Value = expectedValue,
                After = Reference.ByCodename(responseType.Elements.First().Codename),
                Path = $"/elements/codename:{responseType.Elements.First().Codename}/guidelines"
            };


            //Act
            var modifiedType = await client.ModifyContentTypeSnippetAsync(Reference.ByCodename(responseType.Codename), new List<SnippetOperationBaseModel> { changes });


            //Assert
            Assert.Equal(expectedValue, modifiedType.Elements.FirstOrDefault(x => x.Codename == responseType.Elements.First().Codename)?.ToGuidelines().Guidelines);


            // Cleanup
            var typeToClean = Reference.ByCodename(responseType.Codename);
            await client.DeleteContentTypeSnippetAsync(typeToClean);
        }

        [Fact]
        [Trait("Category", "Snippet")]
        public async void ModifySnippet_Remove_ModifiesSnippet()
        {
            //arrange
            var client = CreateManagementClient();

            var responseType = await CreateSnippet(client);

            var changes = new SnippetPatchRemoveModel
            {
                Path = $"/elements/codename:{responseType.Elements.First().Codename}"
            };


            //Act
            var modifiedType = await client.ModifyContentTypeSnippetAsync(Reference.ByCodename(responseType.Codename), new List<SnippetOperationBaseModel> { changes });


            //Assert
            Assert.Null(modifiedType.Elements.FirstOrDefault(x => x.Codename == responseType.Elements.First().Codename));


            // Cleanup
            var typeToClean = Reference.ByCodename(responseType.Codename);
            await client.DeleteContentTypeSnippetAsync(typeToClean);
        }

        private async Task<SnippetModel> CreateSnippet(ManagementClient client, [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            var suffix = $"{memberName.ToLower().Substring(0, Math.Min(40, memberName.Length))}_{sourceLineNumber:d}";

            var type = new SnippetCreateModel
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

            return await client.CreateContentTypeSnippetAsync(type);
        }
    }
}
