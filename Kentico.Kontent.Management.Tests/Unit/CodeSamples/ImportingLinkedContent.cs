using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.LanguageVariants.Elements;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Modules.ModelBuilders;
using Kentico.Kontent.Management.Tests.Unit.Base;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Unit.CodeSamples
{
    public class ImportingLinkedContent : IClassFixture<FileSystemFixture>
    {
        private FileSystemFixture _fileSystemFixture;

        public ImportingLinkedContent(FileSystemFixture fileSystemFixture)
        {
            _fileSystemFixture = fileSystemFixture;
        }

        // DocSection: import_linked_create_item
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void CreateItem()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Empty.json");

            await client.UpsertContentItemByExternalIdAsync("123", new ContentItemUpsertModel
            {
                Name = "On Roasts",
                Type = Reference.ByCodename("article"),
            });
        }

        // DocSection: import_linked_create_sec_item
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void CreateSecondItem()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Empty.json");

            await client.UpsertContentItemByExternalIdAsync("456", new ContentItemUpsertModel
            {
                Name = "Donate with us",
                Type = Reference.ByCodename("article"),
            });
        }

        // DocSection: import_linked_upsert_Sec_variant
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void UpsertSecondVariant()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Empty.json");

            var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("456"), Reference.ByCodename("en-US"));

            await client.UpsertLanguageVariantAsync(identifier, new LanguageVariantUpsertModel
            {
                Elements = ElementBuilder.GetElementsAsDynamic(new BaseElement[]
                {
                    new TextElement
                    {
                        Element = Reference.ByCodename("title"),
                        Value = "Donate with us"
                    },
                    new LinkedItemsElement
                    {
                        Element = Reference.ByCodename("related_articles"),
                        Value = new []
                        {
                            Reference.ByExternalId("123"),
                        }
                    }
                })
            });
        }

        // DocSection: import_linked_upsert_variant
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void UsertVariant()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Empty.json");

            var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("123"), Reference.ByCodename("en-US"));

            var response = await client.UpsertLanguageVariantAsync(identifier, new LanguageVariantUpsertModel
            {
                Elements = ElementBuilder.GetElementsAsDynamic(new BaseElement[]
                {
                    new TextElement
                    {
                        Element = Reference.ByCodename("title"),
                        Value = "On Roasts"
                    },
                    new LinkedItemsElement
                    {
                        Element = Reference.ByCodename("related_articles"),
                        Value = new []
                        {
                            Reference.ByExternalId("456"),
                        }
                    }
                })
            });
        }

        // DocSection: import_linked_validate_content
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void PostValidate()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Empty.json");

            var response = await client.ValidateProjectAsync();
        }
    }
}