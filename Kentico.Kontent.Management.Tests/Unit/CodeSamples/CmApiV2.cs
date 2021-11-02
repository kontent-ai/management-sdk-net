using System;
using System.Linq;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Modules.HttpClient;
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
    }
}