using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Assets;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.LanguageVariants.Elements;
using Kontent.Ai.Management.Tests.Base;
using AssetReference = Kontent.Ai.Management.Models.Content.AssetReference;

namespace Kontent.Ai.Management.Tests.CodeSamples;

/// <summary>
/// Source for Code examples being store in https://github.com/Kontent-ai-Learn/kontent-ai-learn-code-samples/tree/master/net/import-assets
/// </summary>
public class ImportAssets
{
    // IF YOU MAKE ANY CHANGE TO THIS FILE - ADJUST THE CODE SAMPLES

    private const string SampleFolder = "CodeSamples";

    // DocSection: importing_assets_create_asset
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async Task CreateAsset()
    {
        var client = MockClientFactory.CreateForSample(SampleFolder, "Empty.json");

        var filePath = Path.Combine(Environment.CurrentDirectory, "Data", "brno-cafe-1080px.jpg");
        var contentType = "image/jpg";

        // Uploads the file and creates or updates the asset that references it in a single call
        var createdAssetResponse = await client.UpsertAssetAsync(
            Reference.ByExternalId("which-brewing-fits-you"),
            new FileContentSource(filePath, contentType),
            new AssetUpsertModel
            {
                Title = "Brno Cafe",
                Descriptions =
                [
                    new AssetDescription
                    {
                        Description = "Cafe in Brno",
                        Language = Reference.ByCodename("en-US")
                    },
                    new AssetDescription
                    {
                        Description = "Café en Brno",
                        Language = Reference.ByCodename("es-ES")
                    }
                ]
            });
    }

    // DocSection: importing_assets_upload_file
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async Task UploadingFiles()
    {
        var client = MockClientFactory.CreateForSample(SampleFolder, "Empty.json");

        var filePath = Path.Combine(Environment.CurrentDirectory, "Data", "brno-cafe-1080px.jpg");
        var contentType = "image/jpg";

        // Binary file reference to be used when adding a new asset
        var response = await client.UploadFileAsync(new FileContentSource(filePath, contentType));
    }

    // DocSection: importing_assets_upload_file
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async Task UseAsset()
    {
        var client = MockClientFactory.CreateForSample(SampleFolder, "Empty.json");

        var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("ext-cafe-brno"), Reference.ByCodename("en-US"));

        var response = await client.UpsertLanguageVariantAsync(identifier, new LanguageVariantUpsertModel
        {
            Elements =
            [
                new AssetElement
                {
                    Element = Reference.ByCodename("photo"),
                    Value =
                    [
                        new AssetReference { ExternalId = "brno-cafe-image" },
                    ],
                },
            ]
        });
    }

    // DocSection: importing_assets_upload_file
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async Task UseAssetRichText()
    {
        var client = MockClientFactory.CreateForSample(SampleFolder, "Empty.json");

        var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("new-cafes"), Reference.ByCodename("en-US"));

        var response = await client.UpsertLanguageVariantAsync(identifier, new LanguageVariantUpsertModel
        {
            Elements =
            [
                new RichTextElement
                {
                    Element = Reference.ByCodename("body_copy"),
                    Value = "<p>...</p> <figure data-asset-external-id=\"brno-cafe-image\"></figure>",
                },
            ]
        });
    }
}
