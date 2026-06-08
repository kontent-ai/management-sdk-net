using Kontent.Ai.Management.Models.Items;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management.Models.Types.Elements;
using Kontent.Ai.Management.Tests.Base;

namespace Kontent.Ai.Management.Tests.CodeSamples;

/// <summary>
/// Source for Code examples being store in https://github.com/Kontent-ai-Learn/kontent-ai-learn-code-samples/tree/master/net/import-content-items
/// </summary>
public class ImportContentItems
{
    // IF YOU MAKE ANY CHANGE TO THIS FILE - ADJUST THE CODE SAMPLES

    private const string SampleFolder = "CodeSamples";

    // DocSection: importing_create_item
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async Task CreateContentItem()
    {
        var client = MockClientFactory.CreateForSample(SampleFolder, "Empty.json");

        await client.UpsertContentItemAsync(
            Reference.ByExternalId("ext-cafe-brno"),
            new ContentItemUpsertModel { Name = "Brno", Type = Reference.ByExternalId("cafe") });
    }

    // DocSection: importing_create_type
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async Task CreateContentType()
    {
        var client = MockClientFactory.CreateForSample(SampleFolder, "Empty.json");

        var response = await client.CreateContentTypeAsync(new ContentTypeCreateModel
        {
            Codename = "cafe",
            Name = "Cafe",
            ExternalId = "cafe",
            Elements = new ElementMetadataBase[]
            {
                new NumberElementMetadataModel
                {
                    Name = "Price per uni",
                    Codename = "price_per_unit",
                },
                new GuidelinesElementMetadataModel
                {
                    Guidelines = "<h2>Keep Guidelines where the creative process happens.</h2>\n<p>These are sample guidelines that you can place for the whole content item. It’s a place where you can include your content brief, voice and tone recommendations or the URL to a wireframe, so the author will have all the relevant instructions at hand before writing a single line.</p>\n<p>Besides overview guidelines, you can include instructions for each particular content element, as you will see below.</p>",
                    Codename = "n2f836bce_e062_b2cd_5265_f5c3be3aa6f5",
                },
                new TextElementMetadataModel
                {
                    Name = "Street",
                    ExternalId = "street",
                },
                new TextElementMetadataModel
                {
                    Name = "City",
                    ExternalId = "city",
                },
                new TextElementMetadataModel
                {
                    Name = "Country",
                    ExternalId = "country",
                },
                new TextElementMetadataModel
                {
                    Name = "State",
                    ExternalId = "state",
                },
                new TextElementMetadataModel
                {
                    Name = "ZIP code",
                    ExternalId = "zip_code",
                },
                new TextElementMetadataModel
                {
                    Name = "Email",
                    ExternalId = "email",
                },
                new TextElementMetadataModel
                {
                    Name = "Phone",
                    ExternalId = "phone",
                },
                new AssetElementMetadataModel
                {
                    Name = "Photo",
                    Codename = "photo"
                }
            }
        });
    }

    // DocSection: importing_upsert_variant
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async Task UpsertLanguageVariant()
    {
        var client = MockClientFactory.CreateForSample(SampleFolder, "Empty.json");

        var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("ext-cafe-brno"), Reference.ByCodename("en-US"));

        var response = await client.UpsertLanguageVariantAsync(identifier, new LanguageVariantUpsertModel
        {
            Elements = new object[]
            {
                new
                {
                    element = new { external_id = "street" },
                    value = "Nove Sady 25",
                },
                new
                {
                    element = new { external_id = "city" },
                    value = "Brno",
                },
                new
                {
                    element = new { external_id = "country" },
                    value = "Czech republic",
                },
                new
                {
                    element = new { external_id = "state" },
                    value = "Jihomoravsky kraj",
                },
                new
                {
                    element = new { external_id = "zip_code" },
                    value = "60200",
                },
                new
                {
                    element = new { external_id = "phone" },
                    value = "+420 555 555 555",
                },
                new
                {
                    element = new { external_id = "email" },
                    value = "brnocafe@kontent.ai",
                },
            }
        });
    }
}
