using System;
using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.LanguageVariants.Elements;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Types;
using Kentico.Kontent.Management.Models.Types.Elements;
using Kentico.Kontent.Management.Modules.ModelBuilders;
using Kentico.Kontent.Management.Tests.Base;
using Xunit;

namespace Kentico.Kontent.Management.Tests.CodeSamples
{
    /// <summary>
    /// Source for Code examples being store in https://github.com/KenticoDocs/kontent-docs-samples/tree/master/net/importing-rich-text
    /// </summary>
    public class ImportingRichText : IClassFixture<FileSystemFixture>
    {
        // IF YOU MAKE ANY CHANGE TO THIS FILE - ADJUST THE CODE SAMPLES

        private FileSystemFixture _fileSystemFixture;

        public ImportingRichText(FileSystemFixture fileSystemFixture)
        {
            _fileSystemFixture = fileSystemFixture;
            _fileSystemFixture.SetSubFolder("CodeSamples");
        }

        // DocSection: import_rich_create_button_type
        // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
        [Fact]
        public async void CreateButtonType()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

            var response = await client.CreateContentTypeAsync(new ContentTypeCreateModel
            {
                ExternalId = "button",
                Name = "Button",
                Elements = new ElementMetadataBase[]
                {
                    new TextElementMetadataModel
                    {
                        Name = "Text",
                        ExternalId = "button-text",
                    },
                    new TextElementMetadataModel
                    {
                        Name = "Link",
                        ExternalId = "button-link",
                    },
                }
            });
        }

        // DocSection: import_rich_create_item
        // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
        [Fact]
        public async void CreateItem()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

            await client.UpsertContentItemAsync(Reference.ByExternalId("simple-example"), new ContentItemUpsertModel
            {
                Name = "Simple example",
                Type = Reference.ByExternalId("simple-rich-text"),
            });
        }

        // DocSection: import_rich_create_simple_type
        // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
        [Fact]
        public async void CreateCreateSimpleType()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

            var response = await client.CreateContentTypeAsync(new ContentTypeCreateModel
            {
                Name = "Simple Rich Text",
                Codename = "simple-rich-text",
                Elements = new ElementMetadataBase[]
                {
                    new RichTextElementMetadataModel
                    {
                        Name = "Rich Text",
                        ExternalId = "rich-text",
                    },
                }
            });
        }

        // DocSection: import_rich_upsert_variant
        // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
        [Fact]
        public async void UpsertVariant()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

            var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("123"), Reference.ByCodename("en-US"));

            await client.UpsertLanguageVariantAsync(identifier, new LanguageVariantUpsertModel
            {
                Elements = ElementBuilder.GetElementsAsDynamic(new BaseElement[]
                {
                    new RichTextElement
                    {
                        Element = Reference.ByExternalId("rich-text"),
                        Value = "<h1>Lorem Ipsum</h1>\n<p>Lorem ipsum dolor sit amet, consectetur <a href=\"https://wikipedia.org\">adipiscing elit</a>, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>\n<object type=\"application/kenticocloud\" data-type=\"component\" data-id=\"a2ee7bac-15ff-4dce-a244-012b9f98dd7b\"></object>\n<p>Ut enim ad minim veniam, <a data-item-external-id=\"second-page\">quis nostrud</a> exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>\n<ul>\n  <li>Lorem ipsum dolor sit amet</li>\n  <li>Consectetur adipiscing elit</li>\n  <li>Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua</li>\n</ul><figure data-asset-external-id=\"rich-text-asset\"></figure>",
                        Components = new ComponentModel[]
                        {
                            new ComponentModel
                            {
                                Id = Guid.Parse("a2ee7bac-15ff-4dce-a244-012b9f98dd7b"),
                                Type = Reference.ByExternalId("button"),
                                Elements = ElementBuilder.GetElementsAsDynamic(new BaseElement[]
                                {
                                    new TextElement
                                    {
                                        Element = Reference.ByExternalId("button-text"),
                                        Value = "Buy me",
                                    },
                                    new TextElement
                                    {
                                        Element = Reference.ByExternalId("button-link"),
                                        Value = "https://kontent.a",
                                    }
                                })
                            }
                        }
                    },
                }),
            });
        }
    }
}