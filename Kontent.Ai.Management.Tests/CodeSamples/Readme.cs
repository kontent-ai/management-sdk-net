using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Conversion;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Assets;
using Kontent.Ai.Management.Models.Content;
using Kontent.Ai.Management.Models.Items;
using Kontent.Ai.Management.Models.Languages;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.LanguageVariants.Elements;
using Kontent.Ai.Management.Models.Publishing;
using Kontent.Ai.Management.Models.TaxonomyGroups;
using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management.Models.Types.Elements;
using Kontent.Ai.Management.Models.Types.Patch;
using Kontent.Ai.Management.Tests.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using MyProject.Models;
using Polly;
using RichardSzalay.MockHttp;
using System.Text;

// The parameterized members below are compile-only mirrors of README snippets, not runnable tests.
// xUnit1013's code fix would otherwise add an invalid [Theory] to each, breaking the build under `dotnet format`.
#pragma warning disable xUnit1013 // Public test method should be marked as test

namespace Kontent.Ai.Management.Tests.CodeSamples;

/// <summary>
/// Source for Code examples being store in README.md
/// </summary>
public class Readme
{
    // IF YOU MAKE ANY CHANGE TO THIS FILE - ADJUST THE README OF THIS REPO
    private const string SampleFolder = "CodeSamples/Readme";

    [Fact]
    public void CreateManagementClient()
    {
        // Initializes an instance of the ManagementClient client with specified options.
        var client = new ManagementClient(new ManagementOptions
        {
            EnvironmentId = "cbbe2d5c-17c6-0128-be26-e997ba7c1619",
            ApiKey = "ew0...1eo"
        });
    }

    [Fact]
    public void ReferenceCreation()
    {
        var codenameIdentifier = Reference.ByCodename("on_roasts");
        var idIdentifier = Reference.ById(Guid.Parse("9539c671-d578-4fd3-aa5c-b2d8e486c9b8"));
        var externalIdIdentifier = Reference.ByExternalId("Ext-Item-456-Brno");
    }

    [Fact]
    public async Task UpsertDynamicLanguageVariant()
    {
        // Remove next line in codesample
        var client = MockClientFactory.CreateForSample(SampleFolder, "ArticleLanguageVariantUpdatedResponse.json");

        var itemIdentifier = Reference.ById(Guid.Parse("9539c671-d578-4fd3-aa5c-b2d8e486c9b8"));
        var languageIdentifier = Reference.ByCodename("en-US");
        var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

        // Elements to update. Each element is identified by its codename;
        // you can also identify it by `id` or `external_id`.
        var elements = new BaseElement[]
        {
            new TextElement { Element = Reference.ByCodename("title"), Value = "On Roasts - changed" },
            new DateTimeElement { Element = Reference.ByCodename("post_date"), Value = new DateTimeOffset(2018, 7, 4, 0, 0, 0, TimeSpan.Zero) },
        };

        var upsertModel = new LanguageVariantUpsertModel { Elements = elements };

        // Upserts a language variant of a content item
        var response = await client.UpsertLanguageVariantAsync(identifier, upsertModel);
    }

    [Fact]
    public async Task UpsertStronglyTypedLanguageVariant()
    {
        // Remove next line in codesample
        var (client, mock) = MockClientFactory.Create(ArticleConverter());
        // Remove next line in codesample
        mock.Fallback.Respond("application/json", File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", SampleFolder, "ArticleLanguageVariantUpdatedResponse.json")));

        var itemIdentifier = Reference.ById(Guid.Parse("9539c671-d578-4fd3-aa5c-b2d8e486c9b8"));
        var languageIdentifier = Reference.ByCodename("en-US");
        var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

        // Builds the variant from the generated content-type record
        var variant = new Article
        {
            Title = "On Roasts - changed",
            PublishingDate = new DateTimeOffset(2018, 7, 4, 0, 0, 0, TimeSpan.Zero),
        };

        // Upserts a language variant of a content item
        var response = await client.UpsertLanguageVariantAsync(identifier, variant);
    }


    [Fact]
    public async Task QuickStartCreateContentItem()
    {
        // Remove next line in codesample
        var client = MockClientFactory.CreateForSample(SampleFolder, "ArticleContentItemResponse.json");

        var item = new ContentItemCreateModel
        {
            Codename = "on_roasts",
            Name = "On Roasts",
            Type = Reference.ByCodename("article")
        };

        var responseItem = await client.CreateContentItemAsync(item);
    }

    [Fact]
    public async Task CreateStronglyTypedAsset()
    {
        // Remove next line in codesample
        var client = MockClientFactory.CreateForSample(SampleFolder, "FileReferenceResponse.json");

        var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK"));
        var fileName = "Hello.txt";
        var contentType = "text/plain";

        // Returns a reference that you can later use to create an asset
        var fileResult = await client.UploadFileAsync(new FileContentSource(stream, fileName, contentType));

        // Defines the content elements to create
        var taxonomyElements = new[]
        {
            new Models.Assets.AssetElement
            {
                Element = Reference.ByCodename("taxonomy-categories"),
                Value = new[] { "hello", "SDK" }.Select(Reference.ByCodename)
            }
        };

        // Defines the asset to create
        var asset = new AssetCreateModel
        {
            FileReference = fileResult.Value,
            Elements = taxonomyElements
        };

        // Remove next line in codesample
        client = MockClientFactory.CreateForSample(SampleFolder, "AssetResponse.json");
        // Creates an asset
        var response = await client.CreateAssetAsync(asset);
    }

    [Fact]
    public async Task UpdateAssetWithElementBuilder()
    {
        // Remove next line in codesample
        var client = MockClientFactory.CreateForSample(SampleFolder, "AssetResponse.json");

        // Elements to update
        var taxonomyElements = new[]
        {
            new Models.Assets.AssetElement
            {
                Element = Reference.ByCodename("taxonomy-categories"),
                Value = new[]
                {
                    Reference.ByCodename("hello"),
                    Reference.ByCodename("SDK"),
                }
            }
        };

        // Defines the asset to update
        var asset = new AssetUpsertModel
        {
            Elements = taxonomyElements
        };

        var assetReference = Reference.ById(Guid.Parse("6d1c8ee9-76bc-474f-b09f-8a54a98f06ea"));

        // Updates asset metadata
        var response = await client.UpsertAssetAsync(assetReference, asset);
    }

    // ---------------------------------------------------------------------------------------------------------------
    // The members below mirror the remaining README.md code blocks for compile-time safety only: if a model shape or
    // client signature drifts, the build breaks here. They are intentionally not [Fact]s and are never executed —
    // they take the dependencies each snippet assumes already exist.
    // ---------------------------------------------------------------------------------------------------------------

    public async Task QuickStart(IManagementClient client)
    {
        var result = await client.GetContentItemAsync(Reference.ByCodename("on_roasts"));

        if (result.IsSuccess)
        {
            Console.WriteLine(result.Value.Name);
        }
        else
        {
            Console.WriteLine($"{result.StatusCode}: {result.Error?.Message}");
        }
    }

    public void RegisterWithDependencyInjection(IServiceCollection services)
    {
        services.AddManagementClient(options =>
        {
            options.EnvironmentId = "<YOUR_ENVIRONMENT_ID>";
            options.ApiKey = "<YOUR_API_KEY>";
        });
    }

    public async Task BuildStandalone()
    {
        await using var client = new ManagementClient(new ManagementOptions
        {
            EnvironmentId = "<YOUR_ENVIRONMENT_ID>",
            ApiKey = "<YOUR_API_KEY>"
        });
    }

    public async Task BuildWithBuilder()
    {
        await using var client = ManagementClientBuilder
            .WithOptions(options =>
            {
                options.EnvironmentId = "<YOUR_ENVIRONMENT_ID>";
                options.ApiKey = "<YOUR_API_KEY>";
            })
            .WithResilience(pipeline => pipeline.AddTimeout(TimeSpan.FromSeconds(30)))
            .Build();
    }

    public void RegisterFromConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        services.AddManagementClient(configuration);
        services.AddManagementClient(configuration, "MyManagementSection");
    }

    public void RegisterNamedClients(IServiceCollection services)
    {
        services.AddManagementClient("production", options =>
        {
            options.EnvironmentId = "<PRODUCTION_ENVIRONMENT_ID>";
            options.ApiKey = "<PRODUCTION_API_KEY>";
        });

        services.AddManagementClient("staging", options =>
        {
            options.EnvironmentId = "<STAGING_ENVIRONMENT_ID>";
            options.ApiKey = "<STAGING_API_KEY>";
        });
    }

    public void RegisterWithResilience(IServiceCollection services)
    {
        services.AddManagementClient(
            options => { options.EnvironmentId = "..."; options.ApiKey = "..."; },
            configureHttpClient: null,
            configureResilience: pipeline => pipeline
                .AddRetry(new HttpRetryStrategyOptions { MaxRetryAttempts = 5 })
                .AddTimeout(TimeSpan.FromSeconds(30)));
    }

    public async Task ResultPattern(IManagementClient client)
    {
        var result = await client.CreateContentItemAsync(new ContentItemCreateModel
        {
            Name = "On Roasts",
            Codename = "on_roasts",
            Type = Reference.ByCodename("article")
        });

        if (result.IsSuccess)
        {
            ContentItemModel item = result.Value;
        }
    }

    public async Task ErrorHandling(IManagementClient client, ContentItemCreateModel model)
    {
        var result = await client.CreateContentItemAsync(model);

        if (!result.IsSuccess)
        {
            Console.WriteLine($"Request failed ({result.StatusCode}): {result.Error?.Message}");
            Console.WriteLine($"Request ID: {result.Error?.RequestId}");

            foreach (var validationError in result.Error?.ValidationErrors ?? [])
            {
                Console.WriteLine(validationError.Message);
            }
        }
    }

    public void Identifiers()
    {
        var byEmail = UserIdentifier.ByEmail("user@example.com");
        var byUserId = UserIdentifier.ById("usr_0vKjTCH2TkO687K3y3bKNS");

        var variantIdentifier = new LanguageVariantIdentifier(
            Reference.ByCodename("on_roasts"),
            Reference.ByCodename("en-US"));
    }

    public async Task Pagination(IManagementClient client)
    {
        var result = await client.ListContentItemsAsync();

        if (!result.IsSuccess)
        {
            Console.WriteLine($"Failed to list content items: {result.Error?.Message}");
            return;
        }

        IReadOnlyList<ContentItemModel> all = result.Value;
        Console.WriteLine($"{all.Count} items");
        foreach (var item in all)
        {
            Console.WriteLine(item.Name);
        }
    }

    public async Task StreamLargeListing(IManagementClient client)
    {
        await foreach (var page in client.EnumerateContentItemPagesAsync())
        {
            if (!page.IsSuccess)
            {
                Console.WriteLine($"A page failed: {page.Error?.Message}");
                break;
            }

            foreach (var item in page.Value)
            {
                Console.WriteLine(item.Name);
            }
        }
    }

    public async Task ContentItems(IManagementClient client)
    {
        var item = await client.GetContentItemAsync(Reference.ByCodename("on_roasts"));

        var created = await client.CreateContentItemAsync(new ContentItemCreateModel
        {
            Name = "On Roasts",
            Codename = "on_roasts",
            Type = Reference.ByCodename("article"),
            Collection = Reference.ByCodename("default")
        });

        var upserted = await client.UpsertContentItemAsync(
            Reference.ByExternalId("59713"),
            new ContentItemUpsertModel
            {
                Name = "On Roasts",
                Type = Reference.ByCodename("article")
            });

        await client.DeleteContentItemAsync(Reference.ByCodename("on_roasts"));
    }

    public async Task StronglyTypedGet(IManagementClient client, LanguageVariantIdentifier identifier)
    {
        var result = await client.GetLanguageVariantAsync<Article>(identifier);
        LanguageVariantModel<Article> variant = result.Value;

        Article elements = variant.Elements;    // the strongly-typed element values
        // every other property is variant metadata, shared with the untyped LanguageVariantModel:
        Reference item = variant.Item;
        Reference language = variant.Language;
        DateTime lastModified = variant.LastModified;
    }

    public void ElementValueTypes()
    {
        var article = new Article
        {
            PublishingDate = new DateTimeValue
            {
                Value = new DateTimeOffset(2018, 7, 4, 0, 0, 0, TimeSpan.Zero),
                DisplayTimeZone = "Europe/Prague"
            },
            Slug = new UrlSlugValue { Value = "on-roasts", Mode = UrlSlugMode.Custom },
            Rating = new CustomValue { Value = "{\"stars\":5}", SearchableValue = "5 stars" }
        };

        var shorthand = new Article
        {
            Slug = "on-roasts",
            Rating = "{\"stars\":5}"
        };
    }

    public async Task PublishingAndScheduling(IManagementClient client, LanguageVariantIdentifier identifier)
    {
        await client.PublishLanguageVariantAsync(identifier);

        await client.SchedulePublishingOfLanguageVariantAsync(identifier, new ScheduleModel
        {
            ScheduleTo = new DateTimeOffset(2038, 1, 19, 4, 14, 8, TimeSpan.Zero),
            DisplayTimeZone = "Europe/London"
        });

        await client.UnpublishLanguageVariantAsync(identifier);
        await client.CreateNewVersionOfLanguageVariantAsync(identifier);

        await client.ChangeLanguageVariantWorkflowAsync(identifier, new ChangeLanguageVariantWorkflowModel(
            workflow: Reference.ByCodename("default"),
            step: Reference.ByCodename("review")));
    }

    public async Task ContentModel(IManagementClient client)
    {
        await client.CreateContentTypeAsync(new ContentTypeCreateModel
        {
            Name = "Article",
            Codename = "article",
            Elements = new ElementMetadataBase[]
            {
                new TextElementMetadataModel
                {
                    Name = "Title",
                    Codename = "title",
                    IsRequired = true
                },
                new RichTextElementMetadataModel
                {
                    Name = "Body",
                    Codename = "body"
                }
            }
        });

        await client.CreateTaxonomyGroupAsync(new TaxonomyGroupCreateModel
        {
            Name = "Categories",
            Codename = "categories",
            Terms = new[]
            {
                new TaxonomyTermCreateModel { Name = "Coffee", Codename = "coffee" },
                new TaxonomyTermCreateModel { Name = "Brewing", Codename = "brewing" }
            }
        });

        await client.ModifyContentTypeAsync(Reference.ByCodename("article"), new ContentTypeOperationBaseModel[] { });
    }

    public async Task Workflows(IManagementClient client)
    {
        var workflows = await client.ListWorkflowsAsync();
        await client.DeleteWorkflowAsync(Reference.ByCodename("editorial"));
    }

    public async Task EnvironmentAdmin(IManagementClient client)
    {
        await client.CreateLanguageAsync(new LanguageCreateModel
        {
            Name = "German",
            Codename = "de-DE",
            IsActive = true,
            FallbackLanguage = Reference.ByCodename("en-US")
        });
    }

    public async Task TypedElements(IManagementClient client, LanguageVariantIdentifier identifier)
    {
        var result = await client.UpsertLanguageVariantAsync(identifier, new LanguageVariantUpsertModel
        {
            Elements =
            [
                new TextElement { Element = Reference.ByCodename("title"), Value = "On Roasts" },
                new DateTimeElement
                {
                    Element = Reference.ByCodename("post_date"),
                    Value = new DateTimeOffset(2018, 7, 4, 0, 0, 0, TimeSpan.Zero),
                    DisplayTimeZone = "Europe/Prague"
                },
                new UrlSlugElement { Element = Reference.ByCodename("slug"), Value = "on-roasts", Mode = UrlSlugMode.Custom },
            ]
        });
    }

    public async Task ErrorCodeBranching(IManagementClient client, LanguageVariantIdentifier identifier, Article article)
    {
        var result = await client.UpsertLanguageVariantAsync(identifier, article);

        if (!result.IsSuccess && result.Error?.ErrorCode == ManagementErrorCodes.PublishedOrScheduledVariantCannotBeUpdated)
        {
            await client.CreateNewVersionOfLanguageVariantAsync(identifier);
            result = await client.UpsertLanguageVariantAsync(identifier, article);
        }
    }

    public async Task CreateItemWithVariant(IManagementClient client)
    {
        var result = await client.CreateContentItemWithVariantAsync(
            new ContentItemCreateModel { Name = "On Roasts", Type = Reference.ByCodename("article") },
            Reference.ByCodename("en-US"),
            new LanguageVariantUpsertModel { Elements = [] });
    }

    // The test assembly carries deliberately colliding generated-model fixtures; scope the converter to the single
    // record under test so its construction doesn't trip the codename collision. Real consumers don't need this —
    // the client auto-scans the consumer's own models assembly.
    private static ContentItemEnvelopeConverter ArticleConverter()
    {
        var registry = new ContentTypeRegistry();
        registry.Register(typeof(Article));
        return new ContentItemEnvelopeConverter(registry);
    }
}
