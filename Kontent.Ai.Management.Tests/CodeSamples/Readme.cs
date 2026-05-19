using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Models.Assets;
using Kontent.Ai.Management.Models.Items;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.LanguageVariants.Elements;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.StronglyTyped;
using Kontent.Ai.Management.Modules.Extensions;
using Kontent.Ai.Management.Modules.ModelBuilders;
using Kontent.Ai.Management.Tests.Base;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Kontent.Ai.Management.Tests.CodeSamples;

internal class ArticleModel
{
    [JsonProperty("title")]
    [KontentElementId("35a9faae-e502-4e26-a824-26b90b9b2ecd")]
    public TextElement Title { get; set; }

    [JsonProperty("post_date")]
    [KontentElementId("abe785d6-9146-4cab-8096-cba555d3840f")]
    public DateTimeElement PostDate { get; set; }

    [JsonProperty("body_copy")]
    [KontentElementId("bc872953-8507-4c98-9bb7-e9e2a546edb9")]
    public RichTextElement BodyCopy { get; set; }

    [JsonProperty("related_articles")]
    [KontentElementId("3ba9d793-c544-4336-925d-69c3dc485445")]
    public LinkedItemsElement RelatedArticles { get; set; }

    [JsonProperty("personas")]
    [KontentElementId("9ec81a7d-c93a-4d62-adbb-c28fd8a9f3c8")]
    public TaxonomyElement Personas { get; set; }

    [JsonProperty("url_pattern")]
    [KontentElementId("b76e39e8-d3b4-4ed4-87d8-56fb90e0e342")]
    public UrlSlugElement UrlPattern { get; set; }
}

internal class AssetMetadataModel
{
    [JsonProperty("taxonomy_categories")]
    [KontentElementId("c76e39e8-d3b4-4ed4-87d8-56fb90e0e342")]
    public TaxonomyElement TaxonomyCategories { get; set; }
}

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
    public async void UpsertDynamicLanguageVariant()
    {
        // Remove next line in codesample
        var client = MockClientFactory.CreateForSample(SampleFolder, "ArticleLanguageVariantUpdatedResponse.json");

        var itemIdentifier = Reference.ById(Guid.Parse("9539c671-d578-4fd3-aa5c-b2d8e486c9b8"));
        var languageIdentifier = Reference.ByCodename("en-US");
        var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

        // Elements to update
        var elements = new dynamic[]
        {
            new
            {
                element = new
                {
                    // You can use `Reference.ById` if you don't have the model
                    id = typeof(ArticleModel).GetProperty(nameof(ArticleModel.Title)).GetKontentElementId()
                },
                value = "On Roasts - changed",
            },
            new
            {
                element = new
                {
                    // You can use `Reference.ById` if you don't have the model
                    id = typeof(ArticleModel).GetProperty(nameof(ArticleModel.PostDate)).GetKontentElementId()
                },
                value = new DateTime(2018, 7, 4),
            }
        };

        var upsertModel = new LanguageVariantUpsertModel() { Elements = elements };

        // Upserts a language variant of a content item
        var response = await client.UpsertLanguageVariantAsync(identifier, upsertModel);
    }

    [Fact]
    public async void UpsertLanguageVariantWithElementBuilder()
    {
        // Remove next line in codesample
        var client = MockClientFactory.CreateForSample(SampleFolder, "ArticleLanguageVariantUpdatedResponse.json");

        var itemIdentifier = Reference.ById(Guid.Parse("9539c671-d578-4fd3-aa5c-b2d8e486c9b8"));
        var languageIdentifier = Reference.ByCodename("en-US");
        var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

        // Elements to update
        var elements = ElementBuilder.GetElementsAsDynamic(new BaseElement[]
        {
            new TextElement()
            {
                // You can use `Reference.ById` if you don't have the model
                Element = Reference.ById(typeof(ArticleModel).GetProperty(nameof(ArticleModel.Title)).GetKontentElementId()),
                Value = "On Roasts - changed"
            },
            new DateTimeElement()
            {
                // You can use `Reference.ById` if you don't have the model
                Element = Reference.ById(typeof(ArticleModel).GetProperty(nameof(ArticleModel.PostDate)).GetKontentElementId()),
                Value = new DateTime(2018, 7, 4),
                DisplayTimeZone = "Europe/Prague"
            },
        });

        var upsertModel = new LanguageVariantUpsertModel() { Elements = elements };

        // Upserts a language variant of a content item
        var response = await client.UpsertLanguageVariantAsync(identifier, upsertModel);
    }


    [Fact]
    public async void QuickStartCreateContentItem()
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
    public async void CreateStronglyTypedAsset()
    {
        // Remove next line in codesample
        var client = MockClientFactory.CreateForSample(SampleFolder, "FileReferenceResponse.json");

        var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK"));
        var fileName = "Hello.txt";
        var contentType = "text/plain";

        // Returns a reference that you can later use to create an asset
        var fileResult = await client.UploadFileAsync(new FileContentSource(stream, fileName, contentType));

        // Defines the content elements to create
        var stronglyTypedTaxonomyElements = new AssetMetadataModel
        {
            TaxonomyCategories = new TaxonomyElement()
            {
                Value = new[] { "hello", "SDK" }.Select(Reference.ByCodename)
            },
        };

        // Defines the asset to create
        var asset = new AssetCreateModel<AssetMetadataModel>
        {
            FileReference = fileResult,
            Elements = stronglyTypedTaxonomyElements
        };

        // Remove next line in codesample
        client = MockClientFactory.CreateForSample(SampleFolder, "AssetResponse.json");
        // Creates an asset
        var response = await client.CreateAssetAsync(asset);
    }

    [Fact]
    public async void UpdateAssetWithElementBuilder()
    {
        // Remove next line in codesample
        var client = MockClientFactory.CreateForSample(SampleFolder, "AssetResponse.json");

        // Elements to update
        var taxonomyElements = ElementBuilder.GetElementsAsDynamic(
            new TaxonomyElement
            {
                Element = Reference.ByCodename("taxonomy-categories"),
                Value = new[]
                {
                    Reference.ByCodename("hello"),
                    Reference.ByCodename("SDK"),
                }
            });

        // Defines the asset to update
        var asset = new AssetUpsertModel
        {
            Elements = taxonomyElements
        };

        var assetReference = Reference.ById(Guid.Parse("6d1c8ee9-76bc-474f-b09f-8a54a98f06ea"));

        // Updates asset metadata
        var response = await client.UpsertAssetAsync(assetReference, asset);
    }
}
