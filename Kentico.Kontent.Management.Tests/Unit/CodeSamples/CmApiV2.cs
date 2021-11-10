using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.Assets.Patch;
using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.Languages;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.LanguageVariants.Elements;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.TaxonomyGroups;
using Kentico.Kontent.Management.Models.TaxonomyGroups.Patch;
using Kentico.Kontent.Management.Models.Types;
using Kentico.Kontent.Management.Models.Types.Elements;
using Kentico.Kontent.Management.Models.Types.Patch;
using Kentico.Kontent.Management.Models.TypeSnippets;
using Kentico.Kontent.Management.Models.TypeSnippets.Patch;
using Kentico.Kontent.Management.Models.Webhooks;
using Kentico.Kontent.Management.Models.Webhooks.Triggers;
using Kentico.Kontent.Management.Models.Workflow;
using Kentico.Kontent.Management.Modules.HttpClient;
using Kentico.Kontent.Management.Modules.ModelBuilders;
using Kentico.Kontent.Management.Tests.Unit.Base;
using NSubstitute;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Unit.CodeSamples
{
    public class CmApiV2 : IClassFixture<FileSystemFixture>
    {

        // DocSection: cm_api_v2_delete_asset
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        // using Kentico.Kontent.Management;
        //
        // var client = new ManagementClient(new ManagementOptions
        // {
        //     ApiKey = "<YOUR_API_KEY>",
        //     ProjectId = "<YOUR_PROJECT_ID>"
        // });
        // 
        // var identifier = Reference.ById(Guid.Parse("fcbb12e6-66a3-4672-85d9-d502d16b8d9c"));
        // // var identifier = Reference.ByExternalId("which-brewing-fits-you");

        // await client.DeleteAssetAsync(identifier);
        // EndDocSection

        private FileSystemFixture _fileSystemFixture;

        public CmApiV2(FileSystemFixture fileSystemFixture)
        {
            _fileSystemFixture = fileSystemFixture;
        }

        // DocSection: cm_api_v2_delete_asset
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void DeleteAsset()
        {
            var client = _fileSystemFixture.CreateMockClient(Substitute.For<IManagementHttpClient>());

            var identifier = Reference.ById(Guid.Parse("fcbb12e6-66a3-4672-85d9-d502d16b8d9c"));
            // var identifier = Reference.ByExternalId("which-brewing-fits-you");

            await client.DeleteAssetAsync(identifier);
        }


        // DocSection: cm_api_v2_delete_item
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void DeleteItem()
        {
            var client = _fileSystemFixture.CreateMockClient(Substitute.For<IManagementHttpClient>());

            var identifier = Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474"));
            // var identifier = Reference.ByCodename("my_article");
            // var identifier = Reference.ByExternalId("59713");

            await client.DeleteContentItemAsync(identifier);
        }

        // DocSection: cm_api_v2_delete_snippet
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void DeleteSnippet()
        {
            var client = _fileSystemFixture.CreateMockClient(Substitute.For<IManagementHttpClient>());

            var identifier = Reference.ById(Guid.Parse("baf884be-531f-441f-ae88-64205efdd0f6"));
            // var identifier = Reference.ByCodename("metadata");
            // var identifier = Reference.ByExternalId("snippet-type-123");

            await client.DeleteContentTypeSnippetAsync(identifier);
        }

        // DocSection: cm_api_v2_delete_taxonomy_group
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void DeleteTaxonomyGroup()
        {
            var client = _fileSystemFixture.CreateMockClient(Substitute.For<IManagementHttpClient>());

            var identifier = Reference.ById(Guid.Parse("0be13600-e57c-577d-8108-c8d860330985"));
            // var identifier = Reference.ByCodename("personas");
            // var identifier = Reference.ByExternalId("Tax-Group-123");

            await client.DeleteTaxonomyGroupAsync(identifier);
        }

        // DocSection: cm_api_v2_delete_type
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void DeleteType()
        {
            var client = _fileSystemFixture.CreateMockClient(Substitute.For<IManagementHttpClient>());

            var identifier = Reference.ById(Guid.Parse("269202ad-1d9d-47fd-b3e8-bdb05b3e3cf0"));
            // var identifier = Reference.ByCodename("hosted_video");
            // var identifier = Reference.ByExternalId("Content-Type-123");

            await client.DeleteContentTypeAsync(identifier);
        }

        // DocSection: cm_api_v2_delete_variant
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void DeleteLanguageVariant()
        {
            var client = _fileSystemFixture.CreateMockClient(Substitute.For<IManagementHttpClient>());

            var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ByCodename("es-ES"));
            // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ByCodename("es-ES"));
            // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ByCodename("es-ES"));

            await client.DeleteLanguageVariantAsync(identifier);
        }

        // DocSection: cm_api_v2_delete_webhook
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void DeleteWebhook()
        {
            var client = _fileSystemFixture.CreateMockClient(Substitute.For<IManagementHttpClient>());

            var identifier = Reference.ById(Guid.Parse("d53360f7-79e1-42f4-a524-1b53a417d03e"));

            await client.DeleteWebhookAsync(identifier);
        }

        // DocSection: cm_api_v2_get_asset
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetAsset()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Asset.json");

            var identifier = Reference.ById(Guid.Parse("fcbb12e6-66a3-4672-85d9-d502d16b8d9c"));
            // var identifier = Reference.ByCodename("which-brewing-fits-you");

            var response = await client.GetAssetAsync(identifier);

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_get_assets
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetAssets()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Assets.json");

            var response = await client.ListAssetsAsync();

            Assert.Single(response);
        }

        // DocSection: cm_api_v2_get_components_of_type
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetComponentsOfType()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("ContentItemsWithComponents.json");

            var identifier = Reference.ById(Guid.Parse("6434e475-5a29-4866-9fd1-6d1ca873f5be"));
            // var identifier = Reference.ByCodename("article");
            // var identifier = Reference.ByExternalId("my-article-id");

            var response = await client.ListLanguageVariantsOfContentTypeWithComponentsAsync(identifier);

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_get_content_collections
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetContentCollections()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Collections.json");

            var response = await client.ListCollectionsAsync();

            Assert.Equal(2, response.Collections.Count());
        }

        // DocSection: cm_api_v2_get_asset_folders
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetFolders()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("AssetFolders.json");

            var response = await client.GetAssetFoldersAsync();

            Assert.Equal(2, response.Folders.Count());
            Assert.Single(response.Folders.First().Folders);
        }

        // DocSection: cm_api_v2_get_item
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetItem()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("ContentItem.json");

            var identifier = Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474"));
            // var identifier = Reference.ByCodename("my_article");
            // var identifier = Reference.ByExternalId("59713");

            var response = await client.GetContentItemAsync(identifier);

            Assert.NotNull(response);
        }


        // DocSection: cm_api_v2_get_items
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetItems()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("ContentItems.json");

            var response = await client.ListContentItemsAsync();

            Assert.Single(response);
        }

        // DocSection: cm_api_v2_get_language
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetLanguage()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Language.json");

            var identifier = Reference.ById(Guid.Parse("2ea66788-d3b8-5ff5-b37e-258502e4fd5d"));
            // var identifier = Reference.ByCodename("de-DE");
            // var identifier = Reference.ByExternalId("standard-german");

            var response = await client.GetLanguageAsync(identifier);

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_get_languages
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetLanguages()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Languages.json");

            var response = await client.ListLanguagesAsync();

            Assert.Single(response);
        }

        // DocSection: cm_api_v2_get_project_information
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetProjectInformation()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Project.json");

            var response = await client.GetProjectInformation();

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_get_snippet
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetSnippet()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Snippet.json");

            var identifier = Reference.ById(Guid.Parse("baf884be-531f-441f-ae88-64205efdd0f6"));
            // var identifier = Reference.ByCodename("metadata");
            // var identifier = Reference.ByExternalId("snippet-type-123");

            var response = await client.GetContentTypeSnippetAsync(identifier);

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_get_snippets
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetSnippets()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Snippets.json");

            var response = await client.ListContentTypeSnippetsAsync();

            Assert.Single(response);
        }

        // DocSection: cm_api_v2_get_taxonomy_group
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetTaxonomyGroup()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("TaxonomyGroup.json");

            var identifier = Reference.ById(Guid.Parse("0be13600-e57c-577d-8108-c8d860330985"));
            // var identifier = Reference.ByCodename("personas");
            // var identifier = Reference.ByExternalId("Tax-Group-123");

            var response = await client.GetTaxonomyGroupAsync(identifier);

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_get_taxonomy_groups
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetTaxonomyGroups()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("TaxonomyGroups.json");


            var response = await client.ListTaxonomyGroupsAsync();

            Assert.Single(response);
        }

        // DocSection: cm_api_v2_get_type
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetContentType()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("ContentType.json");

            var identifier = Reference.ById(Guid.Parse("269202ad-1d9d-47fd-b3e8-bdb05b3e3cf0"));
            // var identifier = Reference.ByCodename("new_article");
            // var identifier = Reference.ByExternalId("article");

            var response = await client.GetContentTypeAsync(identifier);

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_get_types
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetContentTypes()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("ContentTypes.json");


            var response = await client.ListContentTypesAsync();

            Assert.Single(response);
        }

        // DocSection: cm_api_v2_get_variant
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetLanguageVariant()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("LanguageVariant.json");

            var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ByCodename("es-ES"));
            // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("on_roasts"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("on_roasts"), Reference.ByCodename("es-ES"));
            // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ByCodename("es-ES"));

            var response = await client.GetLanguageVariantAsync(identifier);

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_get_variants
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetLanguageVariants()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("LanguageVariants.json");

            var identifier = Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474"));
            // var identifier = Reference.ByCodename("on_roasts");
            // var identifier = Reference.ByExternalId("59713");

            var response = await client.ListLanguageVariantsByItemAsync(identifier);

            Assert.Single(response);
        }

        // DocSection: cm_api_v2_get_variants_of_type
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetLanguageVariantsByType()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("LanguageVariantsOfType.json");

            var identifier = Reference.ById(Guid.Parse("b7aa4a53-d9b1-48cf-b7a6-ed0b182c4b89"));
            // var identifier = Reference.ByCodename("article");
            // var identifier = Reference.ByExternalId("my-article-id");

            var response = await client.ListLanguageVariantsByTypeAsync(identifier);

            Assert.Single(response);
        }

        // DocSection: cm_api_v2_get_components_of_type
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetVariantsWithComponentsOfType()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("LanguageVariantsOfType.json");

            var identifier = Reference.ById(Guid.Parse("6434e475-5a29-4866-9fd1-6d1ca873f5be"));
            // var identifier = Reference.ByCodename("article");
            // var identifier = Reference.ByExternalId("my-article-id");

            var response = await client.ListLanguageVariantsOfContentTypeWithComponentsAsync(identifier);

            Assert.Single(response);
        }

        // DocSection: cm_api_v2_get_webhook
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetWebhook()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Webhook.json");

            var identifier = Reference.ById(Guid.Parse("5df74e27-1213-484e-b9ae-bcbe90bd5990"));

            var response = await client.GetContentTypeAsync(identifier);

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_get_webhooks
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetWebhooks()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Webhooks.json");

            var response = await client.ListWebhooksAsync();

            Assert.Single(response);
        }

        // DocSection: cm_api_v2_get_workflow_steps
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void GetWorkflowSteps()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("WorkflowSteps.json");

            var response = await client.ListWorkflowStepsAsync();

            Assert.Equal(4, response.Count());
        }

        // DocSection: cm_api_v2_patch_asset_folders
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PatchAssetFolders()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("PatchAssetsFolderResponse.json");

            var response = await client.ModifyAssetFoldersAsync(new AssetFolderOperationBaseModel[]
            {
                new AssetFolderAddIntoModel
                {
                    Reference = Reference.ByExternalId("folder-with-shared-asset"),
                    Value = new AssetFolderHierarchy
                    {
                        ExternalId = "folder-with-shared-assets",
                        Name = "Shared assets",
                        Folders = Enumerable.Empty<AssetFolderHierarchy>(),
                    },
                    Before = Reference.ByExternalId("folder-with-downloadable-assets")
                },
                new AssetFolderRemoveModel
                {
                    Reference = Reference.ByExternalId("folder-with-archived-assets")
                },
                new AssetFolderRenameModel
                {
                    Reference = Reference.ByExternalId("folder-documents"),
                    Value = "Legal documents"
                }
            });

            Assert.NotNull(response);
            Assert.Equal(3, response.Folders.Count());
            Assert.Single(response.Folders.Skip(1).First().Folders);
        }

        // DocSection: cm_api_v2_patch_language
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PatchLanguage()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("PatchLanguageResponse.json");

            var identifier = Reference.ById(Guid.Parse("2ea66788-d3b8-5ff5-b37e-258502e4fd5d"));
            // var identifier = Reference.ByCodename("de-DE");
            // var identifier = Reference.ByExternalId("standard-german");


            var response = await client.ModifyLanguageAsync(identifier, new[]
            {
                new LanguagePatchModel
                {
                    PropertyName = LanguangePropertyName.FallbackLanguage,
                    Value = Reference.ByCodename("en-US")
                },
                new LanguagePatchModel
                {
                    PropertyName = LanguangePropertyName.Name,
                    Value = "Deutsch"
                },
            });

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_patch_snippet
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PatchSnippet()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("PatchSnippetResponse.json");

            var identifier = Reference.ById(Guid.Parse("baf884be-531f-441f-ae88-64205efdd0f6"));
            // var identifier = Reference.ByCodename("my_metadata_snippet");
            // var identifier = Reference.ByExternalId("my-metadata-snippet-id");

            var response = await client.ModifyContentTypeSnippetAsync(identifier, new ContentTypeSnippetOperationBaseModel[]
            {
                new ContentTypeSnippetPatchReplaceModel
                {
                    Path = "/name",
                    Value = "A new snippet name"
                },
                new ContentTypeSnippetPatchReplaceModel
                {
                    Path = "/elements/codename:my_metadata__my_meta_description/guidelines",
                    Value = "Length: 70-150 characters."
                },
                new ContentTypeSnippetAddIntoPatchModel
                {
                    Path = "/elements",
                    Value = new TextElementMetadataModel
                    {
                        Name = "My meta title",
                        Guidelines = "Length: 30–60 characters.",
                        ExternalId = "my-meta-title-id"
                    },
                },
                new SnippetPatchRemoveModel
                {
                    Path = "/elements/id:0b2015d0-16ae-414a-85f9-7e1a4b3a3eae"
                },
                new SnippetPatchRemoveModel
                {
                    Path = "/elements/external_id:my-multiple-choice-id/options/codename:my_option"
                }
            });

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_patch_taxonomy_group
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PatchTaxonomyGroup()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("PatchTaxonomyGroupResponse.json");

            var identifier = Reference.ById(Guid.Parse("0be13600-e57c-577d-8108-c8d860330985"));
            // var identifier = Reference.ByCodename("personas");
            // var identifier = Reference.ByExternalId("Tax-Group-123");

            var response = await client.ModifyTaxonomyGroupAsync(identifier, new TaxonomyGroupOperationBaseModel[]
            {
                new TaxonomyGroupReplacePatchModel
                {
                    PropertyName = PropertyName.Name,
                    Value = "Categories"
                },
                new TaxonomyGroupReplacePatchModel
                {
                    PropertyName = PropertyName.Codename,
                    Value = "category"
                },
                new TaxonomyGroupReplacePatchModel
                {
                    Reference = Reference.ByCodename("first_term"),
                    PropertyName = PropertyName.Terms,
                    Value = new TaxonomyGroupCreateModel[]
                    {
                        new TaxonomyGroupCreateModel
                        {
                            Name = "Second-level taxonomy term",
                            Codename = "second_term",
                            Terms = new TaxonomyGroupCreateModel[]
                            {
                                new TaxonomyGroupCreateModel
                                {
                                    Name = "Third-level taxonomy term",
                                }
                            }
                        }
                    }
                },
                new TaxonomyGroupRemovePatchModel
                {
                    Reference = Reference.ByExternalId("unused-taxonomy-term")
                },
                new TaxonomyGroupAddIntoPatchModel
                {
                    Reference = Reference.ByCodename("second_term"),
                    Value = new TaxonomyGroupCreateModel
                    {
                        Name = "New taxonomy term",
                        ExternalId = "my-new-term",
                        Terms = Array.Empty<TaxonomyGroupCreateModel>()
                    }
                }
            });

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_patch_type
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PatchContentType()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("PatchContentTypeResponse.json");

            var identifier = Reference.ById(Guid.Parse("0be13600-e57c-577d-8108-c8d860330985"));
            // var identifier = Reference.ByCodename("my_article");
            // var identifier = Reference.ByExternalId("my-article-id");

            var response = await client.ModifyContentTypeAsync(identifier, new ContentTypeOperationBaseModel[]
            {
                new ContentTypeReplacePatchModel
                {
                    Path = "/name",
                    Value = "A new type name"
                },
                new ContentTypeReplacePatchModel
                {
                    Path = "/elements/codename:my_text_element/guidelines",
                    Value = "Here you can tell users how to fill in the element."
                },
                new ContentTypeAddIntoPatchModel
                {
                    Path = "/elements",
                    Value = new TextElementMetadataModel
                    {
                        Name = "My title",
                        Guidelines = "Title of the article in plain text.",
                        ExternalId = "my-title-id",
                    },
                },
                new ContentTypeRemovePatchModel
                {
                    Path = "/elements/id:0b2015d0-16ae-414a-85f9-7e1a4b3a3eae"
                }
            });

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_post_asset
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PostAsset()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("PostAssetResponse.json");

            var response = await client.CreateAssetAsync(new AssetCreateModel
            {
                FileReference = new FileReference
                {
                    Id = "fcbb12e6-66a3-4672-85d9-d502d16b8d9c",
                    Type = FileReferenceTypeEnum.Internal
                },
                Folder = Reference.ByExternalId("another-folder"),
                Title = "Coffee Brewing Techniques",
                ExternalId = "which-brewing-fits-you",
                Descriptions = new[]
                {
                    new AssetDescription
                    {
                        Language = Reference.ByCodename("en-US"),
                        Description = "Coffee Brewing Techniques"
                    },
                     new AssetDescription
                    {
                        Language = Reference.ByCodename("es-ES"),
                        Description = "Técnicas para hacer café"
                    }
                }
            });

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_post_asset_folders
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PostAssetFolders()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("PostAssetFoldersResponse.json");

            var response = await client.CreateAssetFoldersAsync(new AssetFolderCreateModel
            {
                Folders = new[]
                {
                    new AssetFolderHierarchy
                    {
                        Name = "Top level folder",
                        ExternalId = "top-folder",
                        Folders = new []
                        {
                            new AssetFolderHierarchy
                            {
                                Name = "Second level folder",
                                ExternalId = "second-folder",
                                Folders = Enumerable.Empty<AssetFolderHierarchy>(),
                            }
                        }
                    }
                }
            });

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_post_file
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PostFile()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("PostFileResponse.json");

            var filePath = Path.Combine(Environment.CurrentDirectory, "Unit", "Data", "kentico_rgb_bigger.png");
            var contentType = "image/png";

            // Binary file reference to be used when adding a new asset
            var response = await client.UploadFileAsync(new FileContentSource(filePath, contentType));

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_post_item
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PostItem()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("PostItemResponse.json");

            var response = await client.CreateContentItemAsync(new ContentItemCreateModel
            {
                Name = "On Roasts",
                Codename = "my_article",
                Type = Reference.ByCodename("article"),
                Collection = Reference.ByCodename("default"),
                ExternalId = "59713",
            });

            Assert.NotNull(response);
        }


        // DocSection: cm_api_v2_post_language
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PostLanguage()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("PostLanguageResponse.json");

            var response = await client.CreateLanguageAsync(new LanguageCreateModel
            {
                Name = "German (Germany)",
                Codename = "de-DE",
                IsActive = true,
                FallbackLanguage = Reference.ByCodename("de-AT"),
                ExternalId = "standard-german"
            });

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_post_snippet
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PostSnippet()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("PostSnippetResponse.json");

            var response = await client.CreateContentTypeSnippetAsync(new CreateContentSnippetCreateModel
            {
                Name = "metadata",
                Codename = "my_metadata",
                ExternalId = "snippet-item-123",
                Elements = new ElementMetadataBase[]
                {
                    new TextElementMetadataModel
                    {
                        Name = "Meta title",
                        Codename = "my_metadata__meta_title",
                        Guidelines = "Length: 30-60 characters",
                        ExternalId = "meta_title",
                    },
                    new TextElementMetadataModel
                    {
                        Name = "Meta description",
                        Codename = "my_metadata__meta_description",
                        Guidelines = "Length: 70-11500 characters",
                        ExternalId = "meta_description",
                    }
                }
            });

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_post_taxonomy_group
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PostTaxonomyGroup()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("PostTaxonomyGroupResponse.json");

            var response = await client.CreateTaxonomyGroupAsync(new TaxonomyGroupCreateModel
            {
                Name = "Personas",
                ExternalId = "Tax-Group-123",
                Codename = "people",
                Terms = new TaxonomyGroupCreateModel[]
                    {
                        new TaxonomyGroupCreateModel
                        {
                            Name = "Coffee expert",
                            Codename = "expert",
                            ExternalId = "Tax-term-456",
                            Terms = new TaxonomyGroupCreateModel[]
                            {
                                new TaxonomyGroupCreateModel
                                {
                                    Name = "Barista",
                                    ExternalId = "Tax-term-789",
                                    Terms = Enumerable.Empty<TaxonomyGroupCreateModel>()
                                },
                                new TaxonomyGroupCreateModel
                                {
                                    Name = "Cafe owner",
                                    ExternalId = "Tax-term-101",
                                    Terms = Enumerable.Empty<TaxonomyGroupCreateModel>()
                                }
                            }
                        },
                        new TaxonomyGroupCreateModel
                        {
                            Name = "Coffee enthusiast",
                            Codename = "enthusiast",
                            ExternalId = "Tax-term-112",
                            Terms = new TaxonomyGroupCreateModel[]
                            {
                                new TaxonomyGroupCreateModel
                                {
                                    Name = "Coffee lover",
                                    ExternalId = "Tax-term-131",
                                    Codename = "lover",
                                    Terms = Enumerable.Empty<TaxonomyGroupCreateModel>()
                                },
                                new TaxonomyGroupCreateModel
                                {
                                    Name = "Coffee blogger",
                                    ExternalId = "Tax-term-145",
                                    Codename = "blogger",
                                    Terms = Enumerable.Empty<TaxonomyGroupCreateModel>()
                                }
                            }
                        }
                    }
            });

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_post_type
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PostType()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("PostTypeResponse.json");

            var response = await client.CreateContentTypeAsync(new ContentTypeCreateModel
            {
                ExternalId = "article",
                Name = "Article",
                Codename = "my_article",
                ContentGroups = new[]
                {
                    new ContentGroupModel
                    {
                        Name = "Article Copy",
                        ExternalId = "article-copy",
                    },
                    new ContentGroupModel
                    {
                        Name = "Author",
                        CodeName = "author",
                    }
                },
                Elements = new ElementMetadataBase[]
                {
                    new TextElementMetadataModel
                    {
                        Name = "Article title",
                        Codename = "title",
                        ContentGroup = Reference.ByCodename("article-copy"),
                    },
                    new RichTextElementMetadataModel
                    {
                        Name = "Article body",
                        Codename = "body",
                        ContentGroup = Reference.ByCodename("article-copy"),
                    },
                    new RichTextElementMetadataModel
                    {
                        Name = "Author bio",
                        Codename = "bio",
                        AllowedBlocks = new HashSet<RichTextBlockType>()
                        {
                            RichTextBlockType.Images,
                            RichTextBlockType.Text
                        },
                        ContentGroup = Reference.ByCodename("author"),
                    },
                }
            });

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_post_validate
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PostValidate()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("PostValidateResponse.json");

            var response = await client.ValidateProjectAsync();

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_post_webhook
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PostWebhook()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("PostWebhookResponse.json");

            var response = await client.CreateWebhookAsync(new WebhookCreateModel
            {
                Name = "Example webhook",
                Url = "https://example.com/webhook",
                Secret = "secret_key",
                Triggers = new WebhookTriggersModel
                {
                    DeliveryApiContentChanges = new[]
                    {
                        new DeliveryApiTriggerModel
                        {
                            Type = TriggerChangeType.LanguageVariant,
                            Operations = new []
                            {
                                "publish",
                                "unpublish"
                            }
                        },
                        new DeliveryApiTriggerModel
                        {
                            Type = TriggerChangeType.Taxonomy,
                            Operations = new []
                            {
                                "archive",
                                "restore",
                                "upsert"
                            }
                        }
                    },
                    PreviewDeliveryApiContentChanges = new[]
                    {
                        new DeliveryApiTriggerModel
                        {
                            Type = TriggerChangeType.LanguageVariant,
                            Operations = new []
                            {
                                "archive",
                                "upsert"
                            }
                        },
                        new DeliveryApiTriggerModel
                        {
                            Type = TriggerChangeType.Taxonomy,
                            Operations = new []
                            {
                                "archive",
                                "restore",
                                "upsert"
                            }
                        }
                    },
                    WorkflowStepChanges = new[]
                    {
                        new WorkflowStepTriggerModel
                        {
                            TransitionsTo = new []
                            {
                                Reference.ById(Guid.Parse("b4363ccd-8f21-45fd-a840-5843d7b7f008")),
                                Reference.ById(Guid.Parse("88ac5e6e-1c5c-4638-96e1-0d61221ad5bf")),
                            }
                        },
                    },
                    ManagementApiContentChanges = new[]
                    {
                        new ManagementApiTriggerModel
                        {
                            Operations = new []
                            {
                                "archive",
                                "create",
                                "restore",
                            }
                        }
                    },
                }
            });

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_put_asset
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PutAsset()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("PutAssetResponse.json");

            var identifier = Reference.ByExternalId("which-brewing-fits-you");
            // var identifier = Reference.ById(Guid.Parse("fcbb12e6-66a3-4672-85d9-d502d16b8d9c"));

            // Used when updating an existing asset
            var updatedAssetResponse = await client.UpdateAssetAsync(identifier, new AssetUpdateModel
            {
                Title = "Coffee Brewing Techniques",
                Descriptions = new List<AssetDescription>
                {
                    new AssetDescription
                    {
                        Description = "Coffee Brewing Techniques",
                        Language = Reference.ByCodename("en-US")
                    },
                    new AssetDescription
                    {
                        Description = "Técnicas para hacer café",
                        Language = Reference.ByCodename("es-ES")
                    }
                }
            });

            // Used when creating a new asset or updating an existing one
            var createdAssetResponse = await client.UpsertAssetByExternalIdAsync("which-brewing-fits-you", new AssetUpsertModel
            {
                // 'fileReference' is only required when creating a new asset
                // To create a file reference, see the "Upload a binary file" endpoint
                FileReference = new FileReference
                {
                    Id = "ab7bdf75-781b-4bf9-aed8-501048860402",
                    Type = FileReferenceTypeEnum.Internal
                },
                Title = "Coffee Brewing Techniques",
                Descriptions = new AssetDescription[]
                {
                    new AssetDescription
                    {
                        Description = "Coffee Brewing Techniques",
                        Language = Reference.ByCodename("en-US")
                    },
                    new AssetDescription
                    {
                        Description = "Técnicas para hacer café",
                        Language = Reference.ByCodename("es-ES")
                    }
                }
            });

            Assert.NotNull(createdAssetResponse);
            Assert.NotNull(updatedAssetResponse);
        }

        // DocSection: cm_api_v2_put_item
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PutItem()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("PutItemResponse.json");

            var identifier = Reference.ByExternalId("59713");
            // var identifier = Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474"));
            // var identifier = Reference.ByCodename("my_article");

            var updatedItemResponse = await client.UpdateContentItemAsync(identifier, new ContentItemUpdateModel
            {
                Name = "On Roasts",
                Codename = "my_article_my_article",
                Collection = Reference.ByCodename("default"),
            });

            var upsertedItemResponse = await client.UpsertContentItemByExternalIdAsync("59713", new ContentItemUpsertModel
            {
                Name = "On Roasts",
                Codename = "my_article_my_article",
                Collection = Reference.ByCodename("default"),
                // 'Type' is only required when creating a new content item
                Type = Reference.ByCodename("article"),
            });

            Assert.NotNull(updatedItemResponse);
            Assert.NotNull(upsertedItemResponse);
        }

        // DocSection: cm_api_v2_put_variant
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PutLanguageVariant()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("PutLanguageVariantResponse.json");

            var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ByCodename("es-ES"));
            // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ByCodename("es-ES"));
            // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ByCodename("es-ES"));

            var response = await client.UpsertLanguageVariantAsync(identifier, new LanguageVariantUpsertModel
            {
                Elements = ElementBuilder.GetElementsAsDynamic(new BaseElement[]
                {
                    new TaxonomyElement
                    {
                        Element = Reference.ByCodename("personas"),
                        Value = new []
                        {
                            Reference.ByCodename("barista"),
                            Reference.ByCodename("coffee_blogger"),
                        }
                    },
                    new DateTimeElement
                    {
                        Element = Reference.ByCodename("post_date"),
                        Value = DateTime.Parse("2014-11-07T00:00:00Z")
                    },
                    new TextElement
                    {
                        Element = Reference.ByCodename("summary"),
                        Value = "Tostar granos de café puede tardar de 6 a 13 minutos. ..."
                    },
                    new LinkedItemsElement
                    {
                        Element = Reference.ByCodename("related_articles"),
                        Value = new []
                        {
                            Reference.ByCodename("coffee_processing_techniques"),
                            Reference.ByCodename("origins_of_arabica_bourbon"),
                        }
                    },
                    new TextElement
                    {
                        Element = Reference.ByCodename("meta_keywords"),
                        Value = "asados, café"
                    },
                    new TextElement
                    {
                        Element = Reference.ByCodename("meta_description"),
                        Value = "Tostar granos de café puede tardar de 6 a 13 minutos. ..."
                    },
                    new UrlSlugElement
                    {
                        Element = Reference.ByCodename("url_pattern"),
                        Mode = "autogenerated"
                    },
                })
            });

            Assert.NotNull(response);
        }

        // DocSection: cm_api_v2_put_variant_cancel_schedule
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PutLanguageVariantCancelSchedule()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Empty.json");

            var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ByCodename("es-ES"));
            // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ByCodename("es-ES"));
            // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ByCodename("es-ES"));

            var exception = await Record.ExceptionAsync(async () => await client.CancelPublishingOfLanguageVariant(identifier));

            Assert.Null(exception);
        }

        // DocSection: cm_api_v2_put_var_cancel_sched_unpublish
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PutCancelUnpublishingOfLanguageVariant()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Empty.json");

            var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ByCodename("es-ES"));
            // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ByCodename("es-ES"));
            // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ByCodename("es-ES"));

            var exception = await Record.ExceptionAsync(async () => await client.CancelUnpublishingOfLanguageVariant(identifier));

            Assert.Null(exception);
        }

        // DocSection: cm_api_v2_put_variant_create_new_version
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PutLanguageVariantNewVersion()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Empty.json");

            var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ByCodename("es-ES"));
            // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ByCodename("es-ES"));
            // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ByCodename("es-ES"));

            var exception = await Record.ExceptionAsync(async () => await client.CreateNewVersionOfLanguageVariant(identifier));
            Assert.Null(exception);
        }

        // DocSection: cm_api_v2_put_variant_publish_or_schedule
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PutPublishLanguageVariant()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Empty.json");

            var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ByCodename("es-ES"));
            // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ByCodename("es-ES"));
            // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ByCodename("es-ES"));

            // Immediate publish
            var immediateException = await Record.ExceptionAsync(async () => await client.PublishLanguageVariant(identifier));

            // Scheduled publish
            var scheduledPublishException = await Record.ExceptionAsync(async () => await client.SchedulePublishingOfLanguageVariant(identifier, new ScheduleModel
            {
                ScheduleTo = DateTime.Parse("2038-01-19T04:14:08+01:00")
            }));

            Assert.Null(immediateException);
            Assert.Null(scheduledPublishException);
        }

        // DocSection: cm_api_v2_put_variant_unpublish_archive
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PutUnpublishLanguageVariant()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Empty.json");

            var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ByCodename("es-ES"));
            // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ByCodename("es-ES"));
            // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
            // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ByCodename("es-ES"));

            // Immediate publish
            var immediateException = await Record.ExceptionAsync(async () => await client.UnpublishLanguageVariant(identifier));

            // Scheduled publish
            var scheduledUnpublishException = await Record.ExceptionAsync(async () => await client.ScheduleUnpublishingOfLanguageVariant(identifier, new ScheduleModel
            {
                ScheduleTo = DateTime.Parse("2038-01-19T04:14:08+01:00")
            }));

            Assert.Null(immediateException);
            Assert.Null(scheduledUnpublishException);
        }

        // DocSection: cm_api_v2_put_variant_workflow
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PutVariantWorkflow()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Empty.json");

            var itemIdentifier = Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474"));
            // var itemIdentifier = Reference.ByCodename("my_article");
            // var itemIdentifier = Reference.ByExternalId("59713");

            var languageIdentifier = Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8"));
            // var languageIdentifier = Reference.ByCodename("es-ES");

            var workflowStepIdentifier = Reference.ById(Guid.Parse("16221cc2-bd22-4414-a513-f3e555c0fc93"));

            var exception = await Record.ExceptionAsync(async () =>
                await client.ChangeLanguageVariantWorkflowStep(new WorkflowIdentifier(itemIdentifier, languageIdentifier, workflowStepIdentifier)));
            Assert.Null(exception);
        }

        // DocSection: mapi_v2_disable_webhook
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PutDisableWebhook()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Empty.json");

            var exception = await Record.ExceptionAsync(async () =>
                await client.DisableWebhookAsync(Reference.ById(Guid.Parse("5df74e27-1213-484e-b9ae-bcbe90bd5990"))));
            Assert.Null(exception);
        }

        // DocSection: mapi_v2_enable_webhook
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PutEnableWebhook()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Empty.json");

            var exception = await Record.ExceptionAsync(async () =>
                await client.EnableWebhookAsync(Reference.ById(Guid.Parse("5df74e27-1213-484e-b9ae-bcbe90bd5990"))));
            Assert.Null(exception);
        }
    }
}