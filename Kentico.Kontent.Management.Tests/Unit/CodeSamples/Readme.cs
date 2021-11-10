using System;
using System.Linq;
using Kentico.Kontent.Management.Exceptions;
using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.LanguageVariants.Elements;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Modules.Extensions;
using Kentico.Kontent.Management.Modules.ModelBuilders;
using Kentico.Kontent.Management.Tests.Unit.Base;
using Newtonsoft.Json;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Unit.CodeSamples
{
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

    public class Readme : IClassFixture<FileSystemFixture>
    {
        private FileSystemFixture _fileSystemFixture;

        public Readme(FileSystemFixture fileSystemFixture)
        {
            _fileSystemFixture = fileSystemFixture;
        }

        [Fact]
        public void CreateManagementClient()
        {
            // Initializes an instance of the ManagementClient client with specified options.
            var client = new ManagementClient(new ManagementOptions
            {
                ProjectId = "cbbe2d5c-17c6-0128-be26-e997ba7c1619",
                ApiKey = "ew0...1eo"
            });
        }

        [Fact]
        public void ReferenceCreation()
        {
            Reference codenameIdentifier = Reference.ByCodename("on_roasts");
            Reference idIdentifier = Reference.ById(Guid.Parse("9539c671-d578-4fd3-aa5c-b2d8e486c9b8"));
            Reference externalIdIdentifier = Reference.ByExternalId("Ext-Item-456-Brno");
        }

        [Fact]
        public async void RetrieveAndUpsertStronglyTypedModel()
        {
            // Remove next line in codesample
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("ReadmeArticleLanguageVariant.json");

            var itemIdentifier = Reference.ById(Guid.Parse("9539c671-d578-4fd3-aa5c-b2d8e486c9b8"));
            var languageIdentifier = Reference.ByCodename("en-US");
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await client.GetLanguageVariantAsync<ArticleModel>(identifier);

            response.Elements.Title = new TextElement() { Value = "On Roasts - changed" };
            response.Elements.PostDate = new DateTimeElement() { Value = new DateTime(2018, 7, 4) };

            // Remove next line in codesample
            client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("ReadmeArticleLanguageVariantUpdated.json");
            var responseVariant = await client.UpsertLanguageVariantAsync(identifier, response.Elements);
        }

        [Fact]
        public async void UpsertDynamicLanguageVariant()
        {
            // Remove next line in codesample
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("ReadmeArticleLanguageVariantUpdated.json");

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
        public async void QuickStartCreateContentItem()
        {
            // Remove next line in codesample
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("ReadmeArticleContentItem.json");

            var item = new ContentItemCreateModel
            {
                Codename = "on_roasts",
                Name = "On Roasts",
                Type = Reference.ByCodename("article")
            };

            var responseItem = await client.CreateContentItemAsync(item);
        }

        [Fact]
        public async void QuickStartAddLanguageVariant()
        {
            // Remove next line in codesample
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("ReadmeArticleLanguageVariant.json");

            var componentId = "04bc8d32-97ab-431a-abaa-83102fc4c198";
            var contentTypeCodename = "article";
            var relatedArticle1Guid = Guid.Parse("b4e7bfaa-593c-4ae4-a231-5136b10757b8");
            var relatedArticle2Guid = Guid.Parse("6d1c8ee9-76bc-474f-b09f-8a54a98f06ea");
            var taxonomyTermGuid1 = Guid.Parse("5c060bf3-ed38-4c77-acfa-9868e6e2b5dd");
            var taxonomyTermGuid2 = Guid.Parse("5c060bf3-ed38-4c77-acfa-9868e6e2b5dd");

            // Defines the content elements to update
            ArticleModel stronglyTypedElements = new ArticleModel
            {
                Title = new TextElement() { Value = "On Roasts" },
                PostDate = new DateTimeElement() { Value = new DateTime(2017, 7, 4) },
                BodyCopy = new RichTextElement
                {
                    Value = $"<p>Rich Text</p><object type=\"application/kenticocloud\" data-type=\"component\" data-id=\"{componentId}\"></object>",
                    Components = new ComponentModel[]
                    {
                        new ComponentModel
                        {
                            Id = Guid.Parse(componentId),
                            Type = Reference.ByCodename(contentTypeCodename),
                            Elements = new dynamic[]
                            {
                                new
                                {
                                    element = new
                                    {
                                        id = typeof(ArticleModel).GetProperty(nameof(ArticleModel.Title)).GetKontentElementId()
                                    },
                                    value = "Article component title",
                                }
                            }
                        }
                    }
                },
                RelatedArticles = new LinkedItemsElement
                {
                    Value = new[] { relatedArticle1Guid, relatedArticle2Guid }.Select(Reference.ById)
                },
                Personas = new TaxonomyElement
                {
                    Value = new[] { taxonomyTermGuid1, taxonomyTermGuid2 }.Select(Reference.ById)
                },
                UrlPattern = new UrlSlugElement { Value = "on-roasts", Mode = "custom" },
            };

            // Specifies the content item and the language variant
            var itemIdentifier = Reference.ByCodename("on_roasts");
            var languageIdentifier = Reference.ByCodename("en-US");
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            // Upserts a language variant of your content item
            var response = await client.UpsertLanguageVariantAsync<ArticleModel>(identifier, stronglyTypedElements);
        }
    }
}