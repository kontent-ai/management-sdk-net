using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Conversion;
using Kontent.Ai.Management.Models.Assets;
using Kontent.Ai.Management.Models.Items;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Tests.Base;
using MyProject.Models;
using RichardSzalay.MockHttp;
using System.Text;

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
    public async void UpsertDynamicLanguageVariant()
    {
        // Remove next line in codesample
        var client = MockClientFactory.CreateForSample(SampleFolder, "ArticleLanguageVariantUpdatedResponse.json");

        var itemIdentifier = Reference.ById(Guid.Parse("9539c671-d578-4fd3-aa5c-b2d8e486c9b8"));
        var languageIdentifier = Reference.ByCodename("en-US");
        var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

        // Elements to update. Each element is identified by its codename;
        // you can also identify it by `id` or `external_id`.
        var elements = new object[]
        {
            new
            {
                element = new { codename = "title" },
                value = "On Roasts - changed",
            },
            new
            {
                element = new { codename = "post_date" },
                value = new DateTime(2018, 7, 4),
            }
        };

        var upsertModel = new LanguageVariantUpsertModel() { Elements = elements };

        // Upserts a language variant of a content item
        var response = await client.UpsertLanguageVariantAsync(identifier, upsertModel);
    }

    [Fact]
    public async void UpsertStronglyTypedLanguageVariant()
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
    public async void UpdateAssetWithElementBuilder()
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
