using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.LanguageVariants.Elements;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Types;
using Kentico.Kontent.Management.Models.Types.Elements;
using Kentico.Kontent.Management.Modules.ModelBuilders;
using Kentico.Kontent.Management.Tests.Unit.Base;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Unit.CodeSamples
{
        /// <summary>
    /// Source for Code examples being store in https://github.com/KenticoDocs/kontent-docs-samples/tree/master/net/importing-to-kentico-kontent
    /// </summary>
    public class ImportingToKenticoKontent : IClassFixture<FileSystemFixture>
    {
        // IF YOU MAKE ANY CHANGE TO THIS FILE - ADJUST THE CODE SAMPLES

        private FileSystemFixture _fileSystemFixture;

        public ImportingToKenticoKontent(FileSystemFixture fileSystemFixture)
        {
            _fileSystemFixture = fileSystemFixture;
            _fileSystemFixture.SetSubFolder("CodeSamples");
        }

        // DocSection: importing_create_item
        // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
        [Fact]
        public async void CreateContentItem()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

            await client.UpsertContentItemAsync(
                Reference.ByExternalId("ext-cafe-brno"),
                new ContentItemUpsertModel { Name = "Brno", Type = Reference.ByExternalId("cafe") });
        }

        // DocSection: importing_create_type
        // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
        [Fact]
        public async void CreateContentType()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

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
        public async void UpsertLanguageVariant()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

            var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("ext-cafe-brno"), Reference.ByCodename("en-US"));

            var response = await client.UpsertLanguageVariantAsync(identifier, new LanguageVariantUpsertModel
            {
                Elements = ElementBuilder.GetElementsAsDynamic(new BaseElement[]
                {
                    new TextElement
                    {
                        Element = Reference.ByExternalId("street"),
                        Value = "Nove Sady 25",
                    },
                    new TextElement
                    {
                        Element = Reference.ByExternalId("city"),
                        Value = "Brno",
                    },
                    new TextElement
                    {
                        Element = Reference.ByExternalId("country"),
                        Value = "Czech republic",
                    },
                    new TextElement
                    {
                        Element = Reference.ByExternalId("state"),
                        Value = "Jihomoravsky kraj",
                    },
                    new TextElement
                    {
                        Element = Reference.ByExternalId("zip_code"),
                        Value = "60200",
                    },
                    new TextElement
                    {
                        Element = Reference.ByExternalId("phone"),
                        Value = "+420 555 555 555",
                    },
                    new TextElement
                    {
                        Element = Reference.ByExternalId("email"),
                        Value = "brnocafe@kontent.ai",
                    },
                })
            });
        }
    }
}