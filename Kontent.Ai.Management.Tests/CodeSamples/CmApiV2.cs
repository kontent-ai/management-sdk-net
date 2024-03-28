using Kontent.Ai.Management.Models.AssetFolders;
using Kontent.Ai.Management.Models.AssetFolders.Patch;
using Kontent.Ai.Management.Models.AssetRenditions;
using Kontent.Ai.Management.Models.Assets;
using Kontent.Ai.Management.Models.Collections;
using Kontent.Ai.Management.Models.Collections.Patch;
using Kontent.Ai.Management.Models.Environments;
using Kontent.Ai.Management.Models.Environments.Patch;
using Kontent.Ai.Management.Models.Items;
using Kontent.Ai.Management.Models.Languages;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.LanguageVariants.Elements;
using Kontent.Ai.Management.Models.LegacyWebhooks;
using Kontent.Ai.Management.Models.LegacyWebhooks.Triggers;
using Kontent.Ai.Management.Models.Publishing;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.TaxonomyGroups;
using Kontent.Ai.Management.Models.TaxonomyGroups.Patch;
using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management.Models.Types.Elements;
using Kontent.Ai.Management.Models.Types.Elements.DefaultValues;
using Kontent.Ai.Management.Models.Types.Patch;
using Kontent.Ai.Management.Models.TypeSnippets;
using Kontent.Ai.Management.Models.TypeSnippets.Patch;
using Kontent.Ai.Management.Models.Users;
using Kontent.Ai.Management.Models.Webhooks;
using Kontent.Ai.Management.Models.Webhooks.Triggers;
using Kontent.Ai.Management.Models.Webhooks.Triggers.Asset;
using Kontent.Ai.Management.Models.Webhooks.Triggers.ContentItem;
using Kontent.Ai.Management.Models.Webhooks.Triggers.ContentType;
using Kontent.Ai.Management.Models.Webhooks.Triggers.Language;
using Kontent.Ai.Management.Models.Webhooks.Triggers.Taxonomy;
using Kontent.Ai.Management.Models.Workflow;
using Kontent.Ai.Management.Modules.HttpClient;
using Kontent.Ai.Management.Modules.ModelBuilders;
using Kontent.Ai.Management.Tests.Base;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Kontent.Ai.Management.Tests.CodeSamples;

/// <summary>
/// Source for Code examples being store in https://github.com/Kontent-ai-Learn/kontent-ai-learn-code-samples/tree/master/net/management-api-v2
/// </summary>
public class CmApiV2 : IClassFixture<FileSystemFixture>
{

    // IF YOU MAKE ANY CHANGE TO THIS FILE - ADJUST THE CODE SAMPLES
    // USE FOLLOWING TEMPLATE

    // DocSection: cm_api_v2_delete_asset
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    // using Kontent.Ai.Management;
    //
    // var client = new ManagementClient(new ManagementOptions
    // {
    //     ApiKey = "<YOUR_API_KEY>",
    //     EnvironmentId = "<YOUR_ENVIRONMENT_ID>"
    // });
    // 
    // var identifier = Reference.ById(Guid.Parse("fcbb12e6-66a3-4672-85d9-d502d16b8d9c"));
    // // var identifier = Reference.ByExternalId("which-brewing-fits-you");

    // await client.DeleteAssetAsync(identifier);
    // EndDocSection

    private readonly FileSystemFixture _fileSystemFixture;

    public CmApiV2(FileSystemFixture fileSystemFixture)
    {
        _fileSystemFixture = fileSystemFixture;
        _fileSystemFixture.SetSubFolder("CodeSamples");
    }

    // DocSection: cm_api_v2_delete_asset
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void DeleteAsset()
    {
        var client = _fileSystemFixture.CreateMockClient(Substitute.For<IManagementHttpClient>());

        var identifier = Reference.ById(Guid.Parse("fcbb12e6-66a3-4672-85d9-d502d16b8d9c"));
        // var identifier = Reference.ByExternalId("which-brewing-fits-you");

        await client.DeleteAssetAsync(identifier);
    }


    // DocSection: cm_api_v2_delete_item
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
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
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
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
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
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
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
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
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
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
    
    // DocSection: cm_api_v2_delete_legacy_webhook
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void DeleteLegacyWebhook()
    {
        var client = _fileSystemFixture.CreateMockClient(Substitute.For<IManagementHttpClient>());
        
        var identifier = Reference.ById(Guid.Parse("d53360f7-79e1-42f4-a524-1b53a417d03e"));
        
        await client.DeleteLegacyWebhookAsync(identifier);
    }

    // DocSection: cm_api_v2_delete_webhook
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void DeleteWebhook()
    {
        var client = _fileSystemFixture.CreateMockClient(Substitute.For<IManagementHttpClient>());

        var identifier = Reference.ById(Guid.Parse("d53360f7-79e1-42f4-a524-1b53a417d03e"));

        await client.DeleteWebhookAsync(identifier);
    }

    // DocSection: cm_api_v2_delete_workflow
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void DeleteWorkflow()
    {
        var client = _fileSystemFixture.CreateMockClient(Substitute.For<IManagementHttpClient>());

        var identifier = Reference.ById(Guid.Parse("8bfdb62d-7aa1-473b-9d80-311ef93db108"));
        // var identifier = Reference.ByCodename("my_workflow");

        await client.DeleteWorkflowAsync(identifier);
    }

    // DocSection: cm_api_v2_delete_environment
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void DeleteEnvironment()
    {
        var client = _fileSystemFixture.CreateMockClient(Substitute.For<IManagementHttpClient>());

        await client.DeleteEnvironmentAsync();
    }

    // DocSection: cm_api_v2_get_asset
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetAsset()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");

        var identifier = Reference.ById(Guid.Parse("fcbb12e6-66a3-4672-85d9-d502d16b8d9c"));
        // var identifier = Reference.ByCodename("which-brewing-fits-you");

        var response = await client.GetAssetAsync(identifier);

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_get_assets
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetAssets()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Assets.json");

        var response = await client.ListAssetsAsync();

        Assert.Single(response);
    }

    // DocSection: cm_api_v2_get_rendition
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetRendition()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("AssetRendition.json");

        var assetReference = Reference.ById(Guid.Parse("fcbb12e6-66a3-4672-85d9-d502d16b8d9c"));
        // var assetReference = Reference.ByExternalId("which-brewing-fits-you");
        var renditionReference = Reference.ById(Guid.Parse("ce559491-0fc1-494b-96f3-244bc095de57"));
        // var renditionReference = Reference.ByExternalId("hero-image-rendition");

        var identifier = new AssetRenditionIdentifier(assetReference, renditionReference);

        var response = await client.GetAssetRenditionAsync(identifier);

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_get_renditions
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetRenditions()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("AssetRenditions.json");

        var assetReference = Reference.ById(Guid.Parse("fcbb12e6-66a3-4672-85d9-d502d16b8d9c"));
        // var assetReference = Reference.ByExternalId("which-brewing-fits-you");

        // Gets the first page of results
        var response = await client.ListAssetRenditionsAsync(assetReference);

        Assert.Single(response);
    }

    // DocSection: cm_api_v2_get_components_of_type
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetComponentsOfType()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("ContentItemsWithComponents.json");

        var identifier = Reference.ById(Guid.Parse("6434e475-5a29-4866-9fd1-6d1ca873f5be"));
        // var identifier = Reference.ByCodename("article");
        // var identifier = Reference.ByExternalId("my-article-id");

        var response = await client.ListLanguageVariantsOfContentTypeWithComponentsAsync(identifier);

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_get_content_collections
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetContentCollections()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Collections.json");

        var response = await client.ListCollectionsAsync();

        Assert.Equal(2, response.Collections.Count());
    }

    // DocSection: cm_api_v2_get_asset_folders
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetFolders()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("AssetFolders.json");

        var response = await client.GetAssetFoldersAsync();

        Assert.Equal(2, response.Folders.Count());
        Assert.Single(response.Folders.First().Folders);
    }

    // DocSection: cm_api_v2_get_item
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetItem()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("ContentItem.json");

        var identifier = Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474"));
        // var identifier = Reference.ByCodename("my_article");
        // var identifier = Reference.ByExternalId("59713");

        var response = await client.GetContentItemAsync(identifier);

        Assert.NotNull(response);
    }


    // DocSection: cm_api_v2_get_items
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetItems()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("ContentItems.json");

        var response = await client.ListContentItemsAsync();

        Assert.Single(response);
    }

    // DocSection: cm_api_v2_get_language
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetLanguage()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Language.json");

        var identifier = Reference.ById(Guid.Parse("2ea66788-d3b8-5ff5-b37e-258502e4fd5d"));
        // var identifier = Reference.ByCodename("de-DE");
        // var identifier = Reference.ByExternalId("standard-german");

        var response = await client.GetLanguageAsync(identifier);

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_get_languages
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetLanguages()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Languages.json");

        var response = await client.ListLanguagesAsync();

        Assert.Single(response);
    }

    // DocSection: cm_api_v2_get_project_information
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetProjectInformation()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Project.json");

        var response = await client.GetEnvironmentInformationAsync();

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_get_snippet
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetSnippet()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Snippet.json");

        var identifier = Reference.ById(Guid.Parse("baf884be-531f-441f-ae88-64205efdd0f6"));
        // var identifier = Reference.ByCodename("metadata");
        // var identifier = Reference.ByExternalId("snippet-type-123");

        var response = await client.GetContentTypeSnippetAsync(identifier);

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_get_snippets
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetSnippets()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Snippets.json");

        var response = await client.ListContentTypeSnippetsAsync();

        Assert.Single(response);
    }

    // DocSection: cm_api_v2_get_taxonomy_group
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetTaxonomyGroup()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("TaxonomyGroup.json");

        var identifier = Reference.ById(Guid.Parse("0be13600-e57c-577d-8108-c8d860330985"));
        // var identifier = Reference.ByCodename("personas");
        // var identifier = Reference.ByExternalId("Tax-Group-123");

        var response = await client.GetTaxonomyGroupAsync(identifier);

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_get_taxonomy_groups
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetTaxonomyGroups()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("TaxonomyGroups.json");


        var response = await client.ListTaxonomyGroupsAsync();

        Assert.Single(response);
    }

    // DocSection: cm_api_v2_get_type
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetContentType()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("ContentType.json");

        var identifier = Reference.ById(Guid.Parse("269202ad-1d9d-47fd-b3e8-bdb05b3e3cf0"));
        // var identifier = Reference.ByCodename("new_article");
        // var identifier = Reference.ByExternalId("article");

        var response = await client.GetContentTypeAsync(identifier);

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_get_types
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetContentTypes()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("ContentTypes.json");


        var response = await client.ListContentTypesAsync();

        Assert.Single(response);
    }

    // DocSection: cm_api_v2_get_variant
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetLanguageVariant()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariant.json");

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
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetLanguageVariants()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariants.json");

        var identifier = Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474"));
        // var identifier = Reference.ByCodename("on_roasts");
        // var identifier = Reference.ByExternalId("59713");

        var response = await client.ListLanguageVariantsByItemAsync(identifier);

        Assert.Single(response);
    }

    // DocSection: cm_api_v2_get_variants_of_type
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetLanguageVariantsByType()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariantsOfType.json");

        var identifier = Reference.ById(Guid.Parse("b7aa4a53-d9b1-48cf-b7a6-ed0b182c4b89"));
        // var identifier = Reference.ByCodename("article");
        // var identifier = Reference.ByExternalId("my-article-id");

        var response = await client.ListLanguageVariantsByTypeAsync(identifier);

        Assert.Single(response);
    }

    // DocSection: cm_api_v2_get_components_of_type
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetVariantsWithComponentsOfType()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariantsOfType.json");

        var identifier = Reference.ById(Guid.Parse("6434e475-5a29-4866-9fd1-6d1ca873f5be"));
        // var identifier = Reference.ByCodename("article");
        // var identifier = Reference.ByExternalId("my-article-id");

        var response = await client.ListLanguageVariantsOfContentTypeWithComponentsAsync(identifier);

        Assert.Single(response);
    }
    
    // DocSection: cm_api_v2_get_legacy_webhook
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetLegacyWebhook()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("LegacyWebhook.json");

        var identifier = Reference.ById(Guid.Parse("5df74e27-1213-484e-b9ae-bcbe90bd5990"));

        var response = await client.GetLegacyWebhookAsync(identifier);

        Assert.NotNull(response);
    }
    
    // DocSection: cm_api_v2_get_webhook
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetWebhook()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Webhook.json");

        var identifier = Reference.ById(Guid.Parse("5df74e27-1213-484e-b9ae-bcbe90bd5990"));

        var response = await client.GetWebhookAsync(identifier);

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_get_legacy_webhooks
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetLegacyWebhooks()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("LegacyWebhooks.json");

        var response = await client.ListLegacyWebhooksAsync();

        Assert.Single(response);
    }
    
    // DocSection: cm_api_v2_get_webhooks
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetWebhooks()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Webhooks.json");

        var response = await client.ListWebhooksAsync();

        Assert.Single(response);
    }

    // DocSection: cm_api_v2_get_workflows
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetWorkflows()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Workflows.json");

        var response = await client.ListWorkflowsAsync();

        Assert.Equal(2, response.Count());
    }


    // DocSection: cm_api_v2_get_role
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetRole()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("ProjectRole.json");

        var identifier = Reference.ById(Guid.Parse("a23d3727-3b16-4d94-9eb0-85225d29cfef"));
        //var identifier = Reference.ByCodename("project-manager");

        var response = await client.GetEnvironmentRoleAsync(identifier);

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_get_roles
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetRoles()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("ProjectRoles.json");

        var response = await client.ListEnvironmentRolesAsync();

        Assert.Equal(2, response.Roles.Count());
    }

    // DocSection: cm_api_v2_get_subscription_user
    // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
    [Fact]
    public async void GetSubscriptionUser()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("SubscriptionUser.json");

        var identifier = UserIdentifier.ByEmail("Joe.Joe@kontent.ai");
        //var identifier = UserIdentifier.ById("usr_0vKjTCH2TkO687K3y3bKNS");

        var response = await client.GetSubscriptionUserAsync(identifier);

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_get_subscription_users
    // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
    [Fact]
    public async void GetSubscriptionUsers()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("SubscriptionUsers.json");

        var response = await client.ListSubscriptionUsersAsync();

        Assert.Equal(2, response.Count());
    }

    // DocSection: cm_api_v2_get_subscription_projects
    // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
    [Fact]
    public async void GetSubscriptionProjects()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("SubscriptionProjects.json");

        var response = await client.ListSubscriptionProjectsAsync();

        Assert.Equal(2, response.Count());
    }

    // DocSection: cm_api_v2_get_environment_status
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetEnvironmentCloningState()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("EnvironmentCloningState.json");

        var response = await client.GetEnvironmentCloningStateAsync();

        Assert.NotNull(response);
    }

    // DocSection: mapi_v2_get_validation_task
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetValidationTask()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("AsyncValidationTask.json");

        var response = await client.GetAsyncValidationTaskAsync(Guid.Parse("88d94fed-4899-4944-9b4b-c919b11a9db0"));

        Assert.NotNull(response);
    }

    // DocSection: mapi_v2_get_validation_issues
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void GetValidationIssues()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("AsyncValidationTaskIssues.json");

        var response = await client.ListAsyncValidationTaskIssuesAsync(Guid.Parse("88d94fed-4899-4944-9b4b-c919b11a9db0"));

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_patch_asset_folders
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PatchAssetFolders()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("PatchAssetsFolderResponse.json");

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

    // DocSection: cm_api_v2_patch_content_collections
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PatchContentCollections()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Collections.json");

        var response = await client.ModifyCollectionAsync(new CollectionOperationBaseModel[]
        {
            new CollectionAddIntoPatchModel
            {
                Value = new CollectionCreateModel
                {
                    ExternalId = "another-collection",
                    Name = "Another collection",
                    Codename = "another_collection_codename"
                },
                After = Reference.ByCodename("second_collection")
            },
            new CollectionMovePatchModel
            {
                Reference = Reference.ByCodename("important_collection"),
                Before = Reference.ByCodename("first_collection")
            },
            new CollectionRemovePatchModel
            {
                CollectionIdentifier = Reference.ByCodename("extra_collection")
            },
            new CollectionReplacePatchModel
            {
                PropertyName = Models.Collections.Patch.PropertyName.Name,
                Value = "A new name",
                Reference = Reference.ByCodename("second_collection")
            }
        });

        Assert.Equal(2, response.Collections.Count());
    }

    // DocSection: cm_api_v2_patch_language
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PatchLanguage()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("PatchLanguageResponse.json");

        var identifier = Reference.ById(Guid.Parse("2ea66788-d3b8-5ff5-b37e-258502e4fd5d"));
        // var identifier = Reference.ByCodename("de-DE");
        // var identifier = Reference.ByExternalId("standard-german");


        var response = await client.ModifyLanguageAsync(identifier, new[]
        {
            new LanguagePatchModel
            {
                PropertyName = LanguagePropertyName.FallbackLanguage,
                Value = Reference.ByCodename("en-US")
            },
            new LanguagePatchModel
            {
                PropertyName = LanguagePropertyName.Name,
                Value = "Deutsch"
            },
        });

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_patch_snippet
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PatchSnippet()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("PatchSnippetResponse.json");

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
            new ContentTypeSnippetPatchRemoveModel
            {
                Path = "/elements/id:0b2015d0-16ae-414a-85f9-7e1a4b3a3eae"
            },
            new ContentTypeSnippetPatchRemoveModel
            {
                Path = "/elements/external_id:my-multiple-choice-id/options/codename:my_option"
            },
            new ContentTypeSnippetPatchMoveModel
            {
                Path = "/elements/codename:my_metadata_snippet__my_meta_title",
                After = Reference.ByCodename("my_metadata_snippet__my_meta_description")
            },
            new ContentTypeSnippetPatchMoveModel
            {
                Path = "/elements/external_id:my-multiple-choice-id/options/id:8e6ec8b1-6510-4b9b-b4be-6c977f4bdfbc",
                Before = Reference.ById(Guid.Parse("6bfe5a60-5cc2-4303-8f72-9cc53431046b"))
            }
        });

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_patch_taxonomy_group
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PatchTaxonomyGroup()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("PatchTaxonomyGroupResponse.json");

        var identifier = Reference.ById(Guid.Parse("0be13600-e57c-577d-8108-c8d860330985"));
        // var identifier = Reference.ByCodename("personas");
        // var identifier = Reference.ByExternalId("Tax-Group-123");

        var response = await client.ModifyTaxonomyGroupAsync(identifier, new TaxonomyGroupOperationBaseModel[]
        {
            new TaxonomyGroupReplacePatchModel
            {
                PropertyName = Models.TaxonomyGroups.Patch.PropertyName.Name,
                Value = "Categories"
            },
            new TaxonomyGroupReplacePatchModel
            {
                PropertyName = Models.TaxonomyGroups.Patch.PropertyName.Codename,
                Value = "category"
            },
            new TaxonomyGroupReplacePatchModel
            {
                Reference = Reference.ByCodename("first_term"),
                PropertyName = Models.TaxonomyGroups.Patch.PropertyName.Terms,
                Value = new TaxonomyGroupCreateModel[]
                {
                    new TaxonomyGroupCreateModel
                    {
                        Name = "Second-level taxonomy term",
                        Codename = "second_term",
                        Terms = new TaxonomyTermCreateModel[]
                        {
                            new TaxonomyTermCreateModel
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
                Value = new TaxonomyTermCreateModel
                {
                    Name = "New taxonomy term",
                    ExternalId = "my-new-term",
                    Terms = Array.Empty<TaxonomyTermCreateModel>()
                }
            },
            new TaxonomyGroupMovePatchModel
            {
                Reference = Reference.ByExternalId("my-new-term"),
                Before = Reference.ByCodename("first_term")
            }
        });

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_patch_type
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PatchContentType()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("PatchContentTypeResponse.json");

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
            new ContentTypeReplacePatchModel
            {
                Path = "/elements/codename:my_text_element/default",
                Value = new TextElementDefaultValueModel {
                    Global = new() {
                        Value = "This is a default value of the text element."
                    }
                }
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
            },
            new ContentTypeMovePatchModel
            {
                Path = "/elements/codename:my_text_element",
                After = Reference.ByExternalId("my-title-id")
            },
            new ContentTypeMovePatchModel
            {
                Path = "/elements/external_id:my-multiple-choice-id/options/id:d66ffa49-86ff-eeaa-c33b-e5d9eefe8b81",
                Before = Reference.ById(Guid.Parse("523e6231-8d80-a158-3601-dffde4e64a78"))
            }
        });

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_patch_environment
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PatchEnvironment()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Environment.json");

        var response = await client.ModifyEnvironmentAsync(new[]
        {
            new EnvironmentRenamePatchModel
            {
                Value = "My Little Production"
            }
        });

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_post_asset
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PostAsset()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("PostAssetResponse.json");

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
            },
            Elements = ElementBuilder.GetElementsAsDynamic(
                new TaxonomyElement
                {
                    Element = Reference.ByCodename("taxonomy-categories"),
                    Value = new[]
                    {
                        Reference.ByCodename("coffee"),
                        Reference.ByCodename("brewing"),
                    }
                })
        });

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_post_asset_folders
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PostAssetFolders()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("PostAssetFoldersResponse.json");

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

    // DocSection: cm_api_v2_post_rendition
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PostAssetRendition()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("AssetRendition.json");

        var assetReference = Reference.ById(Guid.Parse("fcbb12e6-66a3-4672-85d9-d502d16b8d9c"));
        // var assetReference = Reference.ByExternalId("which-brewing-fits-you");

        var response = await client.CreateAssetRenditionAsync(assetReference, new AssetRenditionCreateModel
        {
            ExternalId = "hero-image-rendition",
            Transformation = new RectangleResizeTransformation
            {
                CustomWidth = 120,
                CustomHeight = 240,
                X = 300,
                Y = 200,
                Width = 360,
                Height = 720,
            }
        });

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_post_file
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PostFile()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("PostFileResponse.json");

        var filePath = Path.Combine(Environment.CurrentDirectory, "Data", "which-brewing-fits-you-1080px.jpg");
        var contentType = "image/jpeg";

        // Binary file reference to be used when adding a new asset
        var response = await client.UploadFileAsync(new FileContentSource(filePath, contentType));

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_post_item
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PostItem()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("PostItemResponse.json");

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
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PostLanguage()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("PostLanguageResponse.json");

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
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PostSnippet()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("PostSnippetResponse.json");

        var response = await client.CreateContentTypeSnippetAsync(new ContentTypeSnippetCreateModel
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
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PostTaxonomyGroup()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("PostTaxonomyGroupResponse.json");

        var response = await client.CreateTaxonomyGroupAsync(new TaxonomyGroupCreateModel
        {
            Name = "Personas",
            ExternalId = "Tax-Group-123",
            Codename = "people",
            Terms = new TaxonomyTermCreateModel[]
                {
                    new TaxonomyTermCreateModel
                    {
                        Name = "Coffee expert",
                        Codename = "expert",
                        ExternalId = "Tax-term-456",
                        Terms = new TaxonomyTermCreateModel[]
                        {
                            new TaxonomyTermCreateModel
                            {
                                Name = "Barista",
                                ExternalId = "Tax-term-789",
                                Terms = Enumerable.Empty<TaxonomyTermCreateModel>()
                            },
                            new TaxonomyTermCreateModel
                            {
                                Name = "Cafe owner",
                                ExternalId = "Tax-term-101",
                                Terms = Enumerable.Empty<TaxonomyTermCreateModel>()
                            }
                        }
                    },
                    new TaxonomyTermCreateModel
                    {
                        Name = "Coffee enthusiast",
                        Codename = "enthusiast",
                        ExternalId = "Tax-term-112",
                        Terms = new TaxonomyTermCreateModel[]
                        {
                            new TaxonomyTermCreateModel
                            {
                                Name = "Coffee lover",
                                ExternalId = "Tax-term-131",
                                Codename = "lover",
                                Terms = Enumerable.Empty<TaxonomyTermCreateModel>()
                            },
                            new TaxonomyTermCreateModel
                            {
                                Name = "Coffee blogger",
                                ExternalId = "Tax-term-145",
                                Codename = "blogger",
                                Terms = Enumerable.Empty<TaxonomyTermCreateModel>()
                            }
                        }
                    }
                }
        });

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_post_type
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PostType()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("PostTypeResponse.json");

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
                    DefaultValue = new TextElementDefaultValueModel {
                        Global = new() {
                            Value = "This is the default value of the text element."
                        }
                    }
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
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PostValidate()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("PostValidateResponse.json");

        var response = await client.ValidateEnvironmentAsync();

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_post_legacy_webhook
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PostLegacyWebhook()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("PostLegacyWebhookResponse.json");

        var response = await client.CreateLegacyWebhookAsync(new LegacyWebhookCreateModel
        {
            Name = "Example webhook",
            Url = "https://example.com/webhook",
            Secret = "secret_key",
            Triggers = new LegacyWebhookTriggersModel
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
    
    // DocSection: cm_api_v2_post_webhook
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PostWebhook()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("PostWebhookResponse.json");

        var response = await client.CreateWebhookAsync(new WebhookCreateModel
        {
            Name = "Example webhook",
            Url = "https://example.com/webhook",
            Secret = "secret_key",
            Headers = new []
            {
                new CustomHeaderModel
                {
                    Key = "key1",
                    Value = "value1"
                },
                new CustomHeaderModel
                {
                    Key = "key2",
                    Value = "value2"
                }
            },
            DeliveryTriggers = new DeliveryTriggersModel
            {
                ContentType = new ContentTypeTriggerModel
                {
                    Enabled = true,
                    Actions = new []
                    {
                       new ContentTypeActionModel { Action = ContentTypeAction.Created },
                       new ContentTypeActionModel { Action = ContentTypeAction.Changed },
                       new ContentTypeActionModel { Action = ContentTypeAction.Deleted }
                    },
                    Filters = new ContentTypeFiltersModel {
                        ContentTypes = new [] {
                            Reference.ById(Guid.Parse("dd1439d5-4ee2-4895-a4e4-5b0d9d8c754e"))
                        }
                    }
                },
                ContentItem = new ContentItemTriggerModel
                {
                    Enabled = true,
                    Actions = new []
                    {
                        new ContentItemActionModel
                        {
                            Action = ContentItemAction.Deleted,
                            TransitionTo = new []
                            {
                                new ContentItemWorkflowTransition {
                                    WorkflowReference = Reference.ById(Guid.Parse("88ac5e6e-1c5c-4638-96e1-0d61221ad5bf")),
                                    WorkflowStepReference = Reference.ById(Guid.Parse("b4363ccd-8f21-45fd-a840-5843d7b7f008"))
                                }
                            }
                        }
                    },
                    Filters = new ContentItemFiltersModel
                    {
                        Languages = new []
                        {
                            Reference.ById(Guid.Parse("1aeb9220-f167-4f8e-a7db-1bfec365fa80"))
                        }
                    }
                },
                Taxonomy = new TaxonomyTriggerModel
                {
                    Enabled = true,
                    Actions = new []
                    {
                        new TaxonomyActionModel { Action = TaxonomyAction.TermChanged },
                        new TaxonomyActionModel { Action = TaxonomyAction.MetadataChanged }
                    },
                    Filters = new TaxonomyFiltersModel
                    {
                        Taxonomies = new[] {
                            Reference.ById(Guid.Parse("dd1439d5-4ee2-4895-a4e4-5b0d9d8c754e"))
                        }
                    }
                },
                Asset = new AssetTriggerModel
                {
                    Enabled = true,
                    Actions = new []
                    {
                        new AssetActionModel { Action = AssetAction.Created },
                        new AssetActionModel { Action = AssetAction.Changed }
                    }
                },
                Language = new LanguageTriggerModel
                {
                    Enabled = true,
                    Actions = new []
                    {
                        new LanguageActionModel { Action = LanguageAction.Created }
                    },
                    Filters = new LanguageFiltersModel
                    {
                        Languages = new[] {
                            Reference.ById(Guid.Parse("1aeb9220-f167-4f8e-a7db-1bfec365fa80"))
                        }
                    }
                },
                Slot = DeliverySlot.Preview,
                Events =  WebhookEvents.Specific
            }
        });

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_post_workflow
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PostWorkflow()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Workflow.json");

        var response = await client.CreateWorkflowAsync(new WorkflowUpsertModel
        {
            Name = "My workflow",
            Scopes = new List<WorkflowScopeUpsertModel>
            {
                new()
                {
                    Collections = new List<Reference>{ Reference.ById(Guid.Parse("d29d1904-9011-45ca-8ed3-0f2737a28024")) },
                    ContentTypes = new List<Reference>{ Reference.ById(Guid.Parse("1aeb9220-f167-4f8e-a7db-1bfec365fa80")) }
                }
            },
            Steps = new List<WorkflowStepUpsertModel>
            {
                new()
                {
                    Name = "First step",
                    Codename = "first_step",
                    Color = WorkflowStepColorModel.SkyBlue,
                    RoleIds = new List<Guid>(),
                    TransitionsTo = new List<WorkflowStepTransitionToUpsertModel>
                    {
                        new()
                        {
                            Step = Reference.ByCodename("second_step")
                        }
                    }
                },
                new()
                {
                    Name = "Second step",
                    Codename = "second_step",
                    Color = WorkflowStepColorModel.Rose,
                    RoleIds = new List<Guid> { Guid.Parse("e796887c-38a1-4ab2-a999-c40861bb7a4b") },
                    TransitionsTo = new List<WorkflowStepTransitionToUpsertModel>
                    {
                        new()
                        {
                            Step = Reference.ByCodename("published")
                        }
                    }
                }
            },
            PublishedStep = new WorkflowPublishedStepUpsertModel
            {
                RoleCreateNewVersionIds = new List<Guid>(),
                RolesUnpublishArchivedCancelSchedulingIds = new List<Guid> { Guid.Parse("e796887c-38a1-4ab2-a999-c40861bb7a4b") }
            },
            ArchivedStep = new WorkflowArchivedStepUpsertModel
            {
                RoleIds = new List<Guid>()
            }
        });

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_post_user
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PostUser()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("ProjectUser.json");

        var response = await client.InviteUserIntoEnvironmentAsync(new UserInviteModel
        {
            CollectionGroup = new List<UserCollectionGroup>
            {
                new UserCollectionGroup
                {
                    Collections = new List<Reference>
                    {
                        Reference.ById(Guid.Empty),
                        Reference.ById(Guid.Parse("28b68213-d636-4b01-9fd1-988b93789e17"))
                    },
                    Roles = new List<RoleModel>
                    {
                        new RoleModel
                        {
                            Id = Guid.Parse("f58733b9-520b-406b-9d45-eb15a2baee96"),
                            Languages = new List<Reference>() { Reference.ById(Guid.Parse("7df9a691-cf29-402d-9598-66273e7561b7")) }
                        }
                    }
                }
            }
        });

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_clone_environment
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PostCloneEnvironment()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("ClonedEnvironment.json");

        var response = await client.CloneEnvironmentAsync(new EnvironmentCloneModel
        {
            Name = "New environment",
            RolesToActivate = new[]
            {
                Guid.Parse("2f925111-1457-49d4-a595-0958feae8ae4")
            }
        });

        Assert.NotNull(response);
    }

    // DocSection: mapi_v2_post_validate_async
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PostValidateEnvironment()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("AsyncValidationTask.json");

        var response = await client.InitiateEnvironmentAsyncValidationTaskAsync();

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_put_asset
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PutAsset()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("PutAssetResponse.json");

        var identifier = Reference.ByExternalId("which-brewing-fits-you");
        // var identifier = Reference.ById(Guid.Parse("fcbb12e6-66a3-4672-85d9-d502d16b8d9c"));

        // Used when updating an existing asset
        var updatedAssetResponse = await client.UpsertAssetAsync(identifier, new AssetUpsertModel
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
            },
            Elements = ElementBuilder.GetElementsAsDynamic(
                new TaxonomyElement
                {
                    Element = Reference.ByCodename("taxonomy-categories"),
                    Value = new[]
                    {
                        Reference.ByCodename("coffee"),
                        Reference.ByCodename("brewing"),
                    }
                })
        });

        // Used when creating a new asset or updating an existing one
        var createdAssetResponse = await client.UpsertAssetAsync(Reference.ByExternalId("which-brewing-fits-you"), new AssetUpsertModel
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
            },
            Elements = ElementBuilder.GetElementsAsDynamic(
                new TaxonomyElement
                {
                    Element = Reference.ByCodename("taxonomy-categories"),
                    Value = new[]
                    {
                        Reference.ByCodename("coffee"),
                        Reference.ByCodename("brewing"),
                    }
                })
        });

        Assert.NotNull(createdAssetResponse);
        Assert.NotNull(updatedAssetResponse);
    }

    // DocSection: cm_api_v2_put_rendition
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PutAssetRendition()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("AssetRendition.json");

        var assetReference = Reference.ById(Guid.Parse("fcbb12e6-66a3-4672-85d9-d502d16b8d9c"));
        // var assetReference = Reference.ByExternalId("which-brewing-fits-you");
        var renditionReference = Reference.ById(Guid.Parse("ce559491-0fc1-494b-96f3-244bc095de57"));
        // var renditionReference = Reference.ByExternalId("hero-image-rendition");

        var identifier = new AssetRenditionIdentifier(assetReference, renditionReference);

        var response = await client.UpdateAssetRenditionAsync(identifier, new AssetRenditionUpdateModel()
        {
            Transformation = new RectangleResizeTransformation
            {
                CustomWidth = 120,
                CustomHeight = 240,
                X = 300,
                Y = 200,
                Width = 360,
                Height = 720,
            }
        });

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_put_item
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PutItem()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("PutItemResponse.json");

        var identifier = Reference.ByExternalId("59713");
        // var identifier = Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474"));
        // var identifier = Reference.ByCodename("my_article");

        var upsertedItemResponse = await client.UpsertContentItemAsync(identifier, new ContentItemUpsertModel
        {
            Name = "On Roasts",
            Codename = "my_article_my_article",
            Collection = Reference.ByCodename("default"),
            // 'Type' is only required when creating a new content item
            Type = Reference.ByCodename("article"),
        });

        Assert.NotNull(upsertedItemResponse);
    }

    // DocSection: cm_api_v2_put_variant
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PutLanguageVariant()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("PutLanguageVariantResponse.json");

        var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
        // var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ByCodename("es-ES"));
        // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
        // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ByCodename("es-ES"));
        // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
        // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ByCodename("es-ES"));

        var response = await client.UpsertLanguageVariantAsync(
            identifier,
            new LanguageVariantUpsertModel
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
                        Value = DateTime.Parse("2014-11-07T00:00:00Z"),
                        DisplayTimeZone = "Australia/Sydney"
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
                }),
                DueDate = new DueDateModel
                {
                    Value = DateTime.Parse("2092-01-07T06:04:00.7069564Z")
                }
            },
            new WorkflowStepIdentifier(Reference.ByCodename("default"), Reference.ByCodename("review")));

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_put_variant_cancel_schedule
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PutLanguageVariantCancelSchedule()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

        var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
        // var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ByCodename("es-ES"));
        // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
        // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ByCodename("es-ES"));
        // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
        // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ByCodename("es-ES"));

        var exception = await Record.ExceptionAsync(async () => await client.CancelPublishingOfLanguageVariantAsync(identifier));

        Assert.Null(exception);
    }

    // DocSection: cm_api_v2_put_var_cancel_sched_unpublish
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PutCancelUnpublishingOfLanguageVariant()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

        var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
        // var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ByCodename("es-ES"));
        // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
        // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ByCodename("es-ES"));
        // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
        // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ByCodename("es-ES"));

        var exception = await Record.ExceptionAsync(async () => await client.CancelUnpublishingOfLanguageVariantAsync(identifier));

        Assert.Null(exception);
    }

    // DocSection: cm_api_v2_put_variant_create_new_version
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PutLanguageVariantNewVersion()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

        var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
        // var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ByCodename("es-ES"));
        // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
        // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ByCodename("es-ES"));
        // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
        // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ByCodename("es-ES"));

        var exception = await Record.ExceptionAsync(async () => await client.CreateNewVersionOfLanguageVariantAsync(identifier));
        Assert.Null(exception);
    }

    // DocSection: cm_api_v2_put_variant_publish_or_schedule
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PutPublishLanguageVariant()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

        var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
        // var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ByCodename("es-ES"));
        // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
        // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ByCodename("es-ES"));
        // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
        // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ByCodename("es-ES"));

        // Immediate publish
        var immediateException = await Record.ExceptionAsync(async () => await client.PublishLanguageVariantAsync(identifier));

        // Scheduled publish
        var scheduledPublishException = await Record.ExceptionAsync(async () => await client.SchedulePublishingOfLanguageVariantAsync(identifier, new ScheduleModel
        {
            ScheduleTo = DateTime.Parse("2038-01-19T04:14:08"),
            DisplayTimeZone = "Europe/London"
        }));

        Assert.Null(immediateException);
        Assert.Null(scheduledPublishException);
    }

    // DocSection: cm_api_v2_put_variant_unpublish_archive
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PutUnpublishLanguageVariant()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

        var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
        // var identifier = new LanguageVariantIdentifier(Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474")), Reference.ByCodename("es-ES"));
        // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
        // var identifier = new LanguageVariantIdentifier(Reference.ByCodename("my_article"), Reference.ByCodename("es-ES"));
        // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8")));
        // var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("59713"), Reference.ByCodename("es-ES"));

        // Immediate unpublish
        var immediateException = await Record.ExceptionAsync(async () => await client.UnpublishLanguageVariantAsync(identifier));

        // Scheduled unpublish
        var scheduledUnpublishException = await Record.ExceptionAsync(async () => await client.ScheduleUnpublishingOfLanguageVariantAsync(identifier, new ScheduleModel
        {
            ScheduleTo = DateTime.Parse("2038-01-19T04:14:08"),
            DisplayTimeZone = "Europe/London"
        }));

        Assert.Null(immediateException);
        Assert.Null(scheduledUnpublishException);
    }

    // DocSection: cm_api_v2_put_variant_workflow
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PutVariantWorkflow()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

        var itemIdentifier = Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474"));
        // var itemIdentifier = Reference.ByCodename("my_article");
        // var itemIdentifier = Reference.ByExternalId("59713");

        var languageIdentifier = Reference.ById(Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8"));
        // var languageIdentifier = Reference.ByCodename("es-ES");

        var workflowStepIdentifier = Reference.ById(Guid.Parse("16221cc2-bd22-4414-a513-f3e555c0fc93"));

        var exception = await Record.ExceptionAsync(async () =>
                await client.ChangeLanguageVariantWorkflowAsync(
                    new LanguageVariantIdentifier(itemIdentifier, languageIdentifier),
                    new WorkflowStepIdentifier(Reference.ById(Guid.Empty), workflowStepIdentifier)
                    ));
        Assert.Null(exception);
    }

    // DocSection: mapi_v2_disable_legacy_webhook
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PutDisableLegacyWebhook()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

        var exception = await Record.ExceptionAsync(async () =>
            await client.DisableLegacyWebhookAsync(Reference.ById(Guid.Parse("5df74e27-1213-484e-b9ae-bcbe90bd5990"))));
        Assert.Null(exception);
    }
    
    // DocSection: mapi_v2_disable_webhook
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PutDisableWebhook()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

        var exception = await Record.ExceptionAsync(async () =>
            await client.DisableWebhookAsync(Reference.ById(Guid.Parse("5df74e27-1213-484e-b9ae-bcbe90bd5990"))));
        Assert.Null(exception);
    }

    // DocSection: mapi_v2_enable_legacy_webhook
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PutEnableLegacyWebhook()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

        var exception = await Record.ExceptionAsync(async () =>
            await client.EnableLegacyWebhookAsync(Reference.ById(Guid.Parse("5df74e27-1213-484e-b9ae-bcbe90bd5990"))));
        Assert.Null(exception);
    }
    
    // DocSection: mapi_v2_enable_webhook
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PutEnableWebhook()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

        var exception = await Record.ExceptionAsync(async () =>
            await client.EnableWebhookAsync(Reference.ById(Guid.Parse("5df74e27-1213-484e-b9ae-bcbe90bd5990"))));
        Assert.Null(exception);
    }

    // DocSection: cm_api_v2_put_workflow
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PutWorkflow()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Workflow.json");

        var identifier = Reference.ByCodename("my_workflow");
        // var identifier = Reference.ById(Guid.Parse("f4b3fc05-e988-4dae-9ac1-a94aba566474"));

        var response = await client.UpdateWorkflowAsync(identifier, new WorkflowUpsertModel
        {
            Name = "My workflow",
            Scopes = new List<WorkflowScopeUpsertModel>
            {
                new()
                {
                    Collections = new List<Reference>{ Reference.ById(Guid.Parse("d29d1904-9011-45ca-8ed3-0f2737a28024")) },
                    ContentTypes = new List<Reference>{ Reference.ById(Guid.Parse("1aeb9220-f167-4f8e-a7db-1bfec365fa80")) }
                }
            },
            Steps = new List<WorkflowStepUpsertModel>
            {
                new()
                {
                    Name = "First step",
                    Codename = "first_step",
                    Color = WorkflowStepColorModel.SkyBlue,
                    RoleIds = new List<Guid>(),
                    TransitionsTo = new List<WorkflowStepTransitionToUpsertModel>
                    {
                        new()
                        {
                            Step = Reference.ByCodename("second_step")
                        }
                    }
                },
                new()
                {
                    Name = "Second step",
                    Codename = "second_step",
                    Color = WorkflowStepColorModel.Rose,
                    RoleIds = new List<Guid> { Guid.Parse("e796887c-38a1-4ab2-a999-c40861bb7a4b") },
                    TransitionsTo = new List<WorkflowStepTransitionToUpsertModel>
                    {
                        new()
                        {
                            Step = Reference.ByCodename("published")
                        }
                    }
                }
            },
            PublishedStep = new WorkflowPublishedStepUpsertModel
            {
                RoleCreateNewVersionIds = new List<Guid>(),
                RolesUnpublishArchivedCancelSchedulingIds = new List<Guid> { Guid.Parse("e796887c-38a1-4ab2-a999-c40861bb7a4b") }
            },
            ArchivedStep = new WorkflowArchivedStepUpsertModel
            {
                RoleIds = new List<Guid>()
            }
        });

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_put_user
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PutUser()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("ProjectUser.json");

        var identifier = UserIdentifier.ByEmail("user@kontentai");
        //var identifier = UserIdentifier.ById("d94bc87a-c066-48a1-a910-4f991ccc1fb5");

        var response = await client.ModifyUsersRolesAsync(
            identifier,
            new UserModel
            {
                CollectionGroup = new List<UserCollectionGroup>
                {
                    new UserCollectionGroup
                    {
                        Collections = new List<Reference>
                        {
                            Reference.ById(Guid.Empty),
                        },
                        Roles = new List<RoleModel>
                        {
                            new RoleModel
                            {
                                Id = Guid.Parse("f58733b9-520b-406b-9d45-eb15a2baee96"),
                                Languages = new List<Reference>() { Reference.ByCodename("english") }
                            }
                        }
                    }
                }
            });

        Assert.NotNull(response);
    }

    // DocSection: cm_api_v2_put_subscription_user_activate
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PutSubscriptionUserActivate()
    {
        var client = _fileSystemFixture.CreateMockClientWithoutResponse();

        var identifier = UserIdentifier.ByEmail("user@kontent.ai");
        //var identifier = UserIdentifier.ById("d94bc87a-c066-48a1-a910-4f991ccc1fb5");

        var exception = await Record.ExceptionAsync(
            async () => await client.ActivateSubscriptionUserAsync(identifier));

        Assert.Null(exception);
    }

    // DocSection: cm_api_v2_put_subscription_user_deactivate
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PutSubscriptionUserDeactivate()
    {
        var client = _fileSystemFixture.CreateMockClientWithoutResponse();

        var identifier = UserIdentifier.ByEmail("user@kontent.ai");
        //var identifier = UserIdentifier.ById("d94bc87a-c066-48a1-a910-4f991ccc1fb5");

        var exception = await Record.ExceptionAsync(
            async () => await client.DeactivateSubscriptionUserAsync(identifier));

        Assert.Null(exception);
    }

    // DocSection: cm_api_v2_mark_environment_as_production
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PutMarkEnvironmentAsProduction()
    {
        var client = _fileSystemFixture.CreateMockClientWithoutResponse();

        var exception = await Record.ExceptionAsync(
            async () => await client.MarkEnvironmentAsProductionAsync(new MarkAsProductionModel
            {
                EnableWebhooks = true
            }));

        Assert.Null(exception);
    }
}
