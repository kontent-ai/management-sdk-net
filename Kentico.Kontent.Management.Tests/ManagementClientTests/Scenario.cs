using System;
using System.Collections.Generic;
using System.Reflection;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.LanguageVariants.Elements;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Types;
using Kentico.Kontent.Management.Models.Types.Elements;
using Kentico.Kontent.Management.Modules.Extensions;
using Kentico.Kontent.Management.Tests.Data;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Xunit;
using static Kentico.Kontent.Management.Tests.TestUtils;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    internal class Scenario
    {
        private readonly ManagementClient _client;

        private readonly ManagementOptions _options;

        /// <summary>
        /// Allows to adjust the test run type to achieve the desired behavior (see the <see cref="TestRunType"/> enum for more details).
        /// IMPORTANT: Commit always with TestRunType.MockFromFileSystem
        /// </summary>
        private static readonly TestRunType _runType = TestRunType.MockFromFileSystem;

        public Scenario(string testName)
        {
            // Load configuration from user secrets
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Scenario>()
                .Build();

            // Configurations with TestRunType.LiveEndpoint* require the ApiKey property set in the user secrets
            // Dummy_API_key fallback is needed e.g. for running tests on mocked data - we need to properly init client
            _options = new ManagementOptions() { ApiKey = configuration.GetValue<string>("ApiKey") ?? "Dummy_API_key", ProjectId = configuration.GetValue<string>("ProjectId") ?? PROJECT_ID };

            _client = CreateManagementClient(testName);
        }

        private ManagementClient CreateManagementClient(string testName)
        {
            return TestUtils.CreateManagementClient(_options, _runType, testName);
        }

        /// <summary>
        /// ID of the project the test data are generated from.
        /// </summary>
        public const string PROJECT_ID = "a9931a80-9af4-010b-0590-ecb1273cf1b8";

        public ManagementClient Client => _client;

        public static TestRunType RunType => _runType;

        // Test constants, existing data references leverage the Dancing Goat sample site project that is generated for everyone
        public static readonly Guid EXISTING_ITEM_ID = Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515");
        public const string EXISTING_ITEM_CODENAME = "on_roasts";

        public static readonly Guid EXISTING_ITEM_ID2 = Guid.Parse("0f79b41f-53d3-41b5-8fa0-153c87f60bc1");

        public static readonly Guid EXISTING_LANGUAGE_ID = Guid.Parse("78dbefe8-831b-457e-9352-f4c4eacd5024");
        public const string EXISTING_LANGUAGE_CODENAME = "es-ES";

        public static readonly Guid EXISTING_CONTENT_TYPE_ID = Guid.Parse("ba5cd79d-6d4b-5bed-a681-6aa3a366c8f7");
        public const string EXISTING_CONTENT_TYPE_CODENAME = "article";

        public static readonly Guid EXISTING_TAXONOMY_TERM_ID = Guid.Parse("6e8b18d5-c5e3-5fc1-9014-44c18ef5f5d8");
        public const string EXISTING_TAXONOMY_TERM_CODENAME = "barista";

        public static Guid EXISTING_TAXONOMY_GROUP_ID = Guid.Parse("f6851a6e-a342-5253-8bc6-e8abc8f56b15");
        public const string EXISTING_TAXONOMY_GROUP_CODENAME = "manufacturer";

        public const string EXISTING_WEBHOOK_NAME = "Webhook_all_triggers";
        public static readonly Guid EXISTING_WEBHOOK_ID = Guid.Parse("ef6eb5e6-24f6-4a3a-a291-1cff8d4eecc5");

        public const string EXISTING_COLLECTION_CODENAME = "testcollection";

        public static Guid EXISTING_SNIPPET_ID = Guid.Parse("5482e7b6-9c79-5e81-8c4b-90e172e7ab48");
        public const string EXISTING_SNIPPET_CODENAME = "metadata";

        public static readonly Guid CUSTOM_WORKFLOW_STEP_ID = Guid.Parse("0fabe3b3-d366-4bf9-be41-d37d4b4c2bcb");
        public static readonly Guid DRAFT_WORKFLOW_STEP_ID = Guid.Parse("eee6db3b-545a-4785-8e86-e3772c8756f9");
        public static readonly Guid PUBLISHED_WORKFLOW_STEP_ID = Guid.Parse("c199950d-99f0-4983-b711-6c4c91624b22");
        public static readonly Guid ARCHIVED_WORKFLOW_STEP_ID = Guid.Parse("7a535a69-ad34-47f8-806a-def1fdf4d391");
        public static readonly Guid SCHEDULED_WORKFLOW_STEP_ID = Guid.Parse("9d2b0228-4d0d-4c23-8b49-01a698857709");


        public const string EXISTING_MULTIPLE_CHOICE_OPTION_CODENAME_PAID = "paid";
        public static readonly Guid EXISTING_MULTIPLE_CHOICE_OPTION_ID_PAID = Guid.Parse("00c0f86a-7c51-4e60-abeb-a150e9092e53");
        public const string EXISTING_MULTIPLE_CHOICE_OPTION_CODENAME_FEATURED = "featured";
        public static readonly Guid EXISTING_MULTIPLE_CHOICE_OPTION_ID_FEATURED = Guid.Parse("8972dc90-ae2e-416e-995d-95df6c77e3b2");

        // Root -> 1e5203d8-ae2c-483b-b59b-0defebecf49a > 7194dda7-c5b3-4e85-91a2-026ba2c07e8d > 92c20b68-8f50-4b62-b630-eca6d9b512b3 -> 3b34af2a-526a-47bc-8a27-a40bb37dd3e2
        public const string ASSET_FOLDER_ID_1ST_LEVEL = "1e5203d8-ae2c-483b-b59b-0defebecf49a";
        public const string ASSET_FOLDER_ID_2ND_LEVEL = "7194dda7-c5b3-4e85-91a2-026ba2c07e8d";
        public const string ASSET_FOLDER_ID_3RD_LEVEL = "92c20b68-8f50-4b62-b630-eca6d9b512b3";
        public const string ASSET_FOLDER_ID_4TH_LEVEL = "3b34af2a-526a-47bc-8a27-a40bb37dd3e2";
        public static readonly Guid EXISTING_ASSET_ID = Guid.Parse("5c08a538-5b58-44eb-81ef-43fb37eeb815");

        public static readonly Guid RICH_TEXT_TWEET_COMPONENT_ID = Guid.Parse("46c05bd9-d418-4507-836c-9accc5a39db3");

        public static readonly Guid TWEET_TYPE_ID = Guid.Parse("17ff8a28-ebe6-5c9d-95ea-18fe1ff86d2d");
        public static readonly Guid RICH_TEXT_COMPONENT_TWEET_TYPE_ID = TWEET_TYPE_ID;
        public static readonly string TWEET_TYPE_CODENAME = "tweet";
        public static readonly string EXISTING_COMPONENT_TYPE_CODENAME = TWEET_TYPE_CODENAME;
        public static readonly Guid TWEET_THEME_ELEMENT_DARK_OPTION_ID = Guid.Parse("061e69f7-0965-5e37-97bc-29963cfaebe8");
        public const string TWEET_THEME_ELEMENT_DARK_OPTION_CODENAME = "dark";
        public static readonly Guid TWEET_DISPLAY_OPTIONS_HIDE_THREAD_OPTION_ID = Guid.Parse("dd78b09e-4337-599c-9701-20a0a165c63b");
        public const string TWEET_DISPLAY_OPTIONS_HIDE_THREAD_OPTION_CODENAME = "hide_thread";

        public static IReadOnlyList<BaseElement> Elements => new BaseElement[]
        {
            new TextElement
            {
                Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Title)).GetKontentElementId()),
                Value = "On Roasts",
            },
            new DateTimeElement
            {
                Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.PostDate)).GetKontentElementId()),
                Value = new DateTime(2017, 7, 4),
            },
            new RichTextElement
            {
                Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.BodyCopy)).GetKontentElementId()),
                Value = $@"
                        <h1>Light Roasts</h1>
                        <p>Usually roasted for 6 - 8 minutes or simply until achieving a light brown color.This method is used for milder coffee varieties and for coffee tasting.This type of roasting allows the natural characteristics of each coffee to show.The aroma of coffees produced from light roasts is usually more intense.The cup itself is more acidic and the concentration of caffeine is higher.</p>
                        <object type=""application/kenticocloud"" data-type=""component"" data-id=""{RICH_TEXT_TWEET_COMPONENT_ID}""></object>
                        ",
                Components = new ComponentModel[]
                {
                    new ComponentModel
                    {

                        Id = RICH_TEXT_TWEET_COMPONENT_ID,
                        Type = Reference.ById(TWEET_TYPE_ID),
                        Elements = new dynamic[]
                        {
                            new
                            {
                                element = new {
                                    id = typeof(TweetTestModel).GetProperty(nameof(TweetTestModel.TweetLink)).GetKontentElementId()
                                },
                                value =  "https://twitter.com/ChrastinaOndrej/status/1417105245935706123"
                            },
                            new
                            {
                                element = new {
                                    id = typeof(TweetTestModel).GetProperty(nameof(TweetTestModel.Theme)).GetKontentElementId()
                                },
                                value = new[] {
                                        Reference.ByCodename(TWEET_THEME_ELEMENT_DARK_OPTION_CODENAME)
                                }
                            },
                            new
                            {
                                element = new {
                                    id = typeof(TweetTestModel).GetProperty(nameof(TweetTestModel.DisplayOptions)).GetKontentElementId()
                                },
                                value = new[] {
                                    Reference.ByCodename(TWEET_DISPLAY_OPTIONS_HIDE_THREAD_OPTION_CODENAME)
                                }
                            }
                        }
                    }
                }
            },
            new LinkedItemsElement
            {
                Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.RelatedArticles)).GetKontentElementId()),
                Value = new[]
                {
                    Reference.ById(EXISTING_ITEM_ID),
                },
            },
            new UrlSlugElement
            {
                Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.UrlPattern)).GetKontentElementId()),
                Value = "on-roasts",
                Mode = "custom"
            },
            new TaxonomyElement
            {
                Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Personas)).GetKontentElementId()),
                Value = new[]
                {
                    Reference.ByCodename(EXISTING_TAXONOMY_TERM_CODENAME),
                },
            },
            new AssetElement
            {
                Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.TeaserImage)).GetKontentElementId()),
                Value = new[]
                {
                    Reference.ById(EXISTING_ASSET_ID),
                }
            },
            new MultipleChoiceElement
            {
                Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Options)).GetKontentElementId()),
                Value = new[]
                {
                    Reference.ByCodename(EXISTING_MULTIPLE_CHOICE_OPTION_CODENAME_PAID),
                    Reference.ByCodename(EXISTING_MULTIPLE_CHOICE_OPTION_CODENAME_FEATURED)
                },
            },
        };

        public static ComplexTestModel StronglyTypedElements => new()
        {
            BodyCopy = new RichTextElement
            {
                Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.BodyCopy)).GetKontentElementId()),
                Value = $"<h1>Light Roasts</h1> <p>Usually roasted for 6 - 8 minutes or simply until achieving a light brown color.This method is used for milder coffee varieties and for coffee tasting.This type of roasting allows the natural characteristics of each coffee to show.The aroma of coffees produced from light roasts is usually more intense.The cup itself is more acidic and the concentration of caffeine is higher.</p><object type=\"application/kenticocloud\" data-type=\"component\" data-id=\"{RICH_TEXT_TWEET_COMPONENT_ID}\"></object>",
                Components = new ComponentModel[]
                {
                    new ComponentModel
                    {
                        Id = RICH_TEXT_TWEET_COMPONENT_ID,
                        Type = Reference.ById(TWEET_TYPE_ID),
                        Elements = new BaseElement[]
                        {
                            new TextElement
                            {
                                Element = Reference.ById(typeof(TweetTestModel).GetProperty(nameof(TweetTestModel.TweetLink)).GetKontentElementId()),
                                Value = "https://twitter.com/ChrastinaOndrej/status/1417105245935706123"
                            },
                            new MultipleChoiceElement
                            {
                                Element = Reference.ById(typeof(TweetTestModel).GetProperty(nameof(TweetTestModel.Theme)).GetKontentElementId()),
                                Value = new[] { Reference.ById(TWEET_THEME_ELEMENT_DARK_OPTION_ID) },
                            },
                            new MultipleChoiceElement
                            {
                                Element = Reference.ById(typeof(TweetTestModel).GetProperty(nameof(TweetTestModel.DisplayOptions)).GetKontentElementId()),
                                Value = new[] { Reference.ById(TWEET_DISPLAY_OPTIONS_HIDE_THREAD_OPTION_ID) },
                            }
                        }
                    }
                }
            },
            MetaDescription = new TextElement
            {
                Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.MetaDescription)).GetKontentElementId()),
                Value = "MetaDescription"
            },
            MetaKeywords = new TextElement
            {
                Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.MetaKeywords)).GetKontentElementId()),
                Value = "MetaKeywords"
            },
            Options = new MultipleChoiceElement
            {
                Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Options)).GetKontentElementId()),
                Value = new[]
                {
                    Reference.ById(EXISTING_MULTIPLE_CHOICE_OPTION_ID_PAID),
                    Reference.ById(EXISTING_MULTIPLE_CHOICE_OPTION_ID_FEATURED)
                }
            },
            Personas = new TaxonomyElement
            {
                Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Personas)).GetKontentElementId()),
                Value = new[] { Reference.ByCodename(EXISTING_TAXONOMY_TERM_CODENAME) }
            },
            PostDate = new DateTimeElement
            {
                Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.PostDate)).GetKontentElementId()),
                Value = new DateTime(2017, 7, 4)
            },
            RelatedArticles = new LinkedItemsElement
            {
                Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.RelatedArticles)).GetKontentElementId()),
                Value = new[] { Reference.ById(EXISTING_ITEM_ID) }
            },
            Rating = new NumberElement
            {
                Value = 3.14m,
                Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Rating)).GetKontentElementId()),
            },
            SelectedForm = new CustomElement
            {
                Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.SelectedForm)).GetKontentElementId()),
                Value = "{\"formId\": 42}",
                SearchableValue = "Almighty form!"
            },
            Summary = new TextElement
            {
                Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Summary)).GetKontentElementId()),
                Value = "Summary"
            },
            TeaserImage = new AssetElement
            {
                Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.TeaserImage)).GetKontentElementId()),
                Value = new[] { Reference.ById(EXISTING_ASSET_ID) },
            },
            Title = new TextElement
            {
                Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Title)).GetKontentElementId()),
                Value = "On Roast"
            },
            UrlPattern = new UrlSlugElement
            {
                Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.UrlPattern)).GetKontentElementId()),
                Value = "on-roasts",
                Mode = "custom"
            }
        };


        //todo add customElement
        public static List<ElementMetadataBase> GetElementMetadata() => new List<ElementMetadataBase>
                {
                    new AssetElementMetadataModel
                    {
                        Name = "MyAsset",
                        AllowedFileTypes = FileType.Any,
                        Codename = ""
                    },
                    new ContentTypeSnippetElementMetadataModel
                    {
                        Codename = "contenttypesnippet_codename",
                        ExternalId = "contentTypeSnippet_externalId",
                        SnippetIdentifier = Reference.ByCodename(EXISTING_SNIPPET_CODENAME),
                    },
                    new DateTimeElementMetadataModel
                    {
                        Codename = "datetimeelement_codename",
                        ExternalId = "DateTimeElementSnippet_externalId",
                        IsRequired = false,
                        Name = "DateTimename",
                    },
                    new GuidelinesElementMetadataModel
                    {
                        Codename = "guidelines_codename",
                        ExternalId = "guidelines_external_id",
                        Guidelines = "<h3>Guidelines</h3>",
                    },
                    new LinkedItemsElementMetadataModel
                    {
                        Codename = "linkeditemselementcodename",
                        IsRequired = true,
                        ItemCountLimit = new LimitModel { Value = 10, Condition = LimitType.AtMost },
                        Name = "LinkedItemsElementName",
                    },
                    new MultipleChoiceElementMetadataModel
                    {
                        Name = "Is special Delivery",
                        Codename = "multiple_choice_element_codename",
                        IsRequired = false,
                        Mode = MultipleChoiceMode.Single,
                        Options = new[] {
                            new MultipleChoiceOptionModel
                            {
                                Name = "Yes",
                                Codename = "yes"
                            }
                        },
                    },
                    new NumberElementMetadataModel
                    {
                        Codename = "numberrlementcodename",
                        ExternalId = "NumberElementexternal_id",
                        Guidelines = "<h3>NumberElement</h3>",
                        Name = "NumberElementName",
                    },
                    new RichTextElementMetadataModel
                    {
                        Codename = "richtextelementcodename",
                        ExternalId = "RichTextElementexternal_id",
                        Guidelines = "<h3>RichTextElement</h3>",
                        Name = "RichTextElementName",
                    },
                    new TaxonomyElementMetadataModel
                    {
                        Codename = "taxonomyelementcodename",
                        ExternalId = "TaxonomyElementMetadata_id",
                        Guidelines = "<h3>TaxonomyElement</h3>",
                        TaxonomyGroup = Reference.ById(EXISTING_TAXONOMY_GROUP_ID),
                    },
                    new TextElementMetadataModel
                    {
                        Codename = "textelementmetadatacodename",
                        Name = "TextElementMetadataName",
                        IsRequired = false,
                    },
                    new UrlSlugElementMetadataModel
                    {
                        Codename = "urlslugrlementcodename",
                        Name = "UrlSlugElementMetadataName",
                        IsRequired = false,
                        DependsOn = new UrlSlugDependency { Element = Reference.ByCodename("textelementmetadatacodename") },
                    }
                };
    }
}
