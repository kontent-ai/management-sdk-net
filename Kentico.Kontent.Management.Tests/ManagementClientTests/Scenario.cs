using System;
using System.Collections.Generic;
using System.Reflection;
using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.LanguageVariants.Elements;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Modules.Extensions;
using Kentico.Kontent.Management.Tests.Data;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using static Kentico.Kontent.Management.Tests.TestUtils;

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
        public const string EXISTING_WEBHOOK_ID = "ef6eb5e6-24f6-4a3a-a291-1cff8d4eecc5";

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

        public static readonly Guid RICH_TEXT_COMPONENT_ID = Guid.Parse("46c05bd9-d418-4507-836c-9accc5a39db3");

        public static readonly Guid TWEET_TYPE_ID = Guid.Parse("17ff8a28-ebe6-5c9d-95ea-18fe1ff86d2d");
        public static readonly Guid TWEET_THEME_ELEMENT_DARK_OPTION_ID = Guid.Parse("061e69f7-0965-5e37-97bc-29963cfaebe8");
        public const string TWEET_THEME_ELEMENT_DARK_OPTION_CODENAME = "dark";
        public static readonly Guid TWEET_DISPLAY_OPTIONS_HIDE_THREAD_OPTION_ID = Guid.Parse("dd78b09e-4337-599c-9701-20a0a165c63b");
        public const string TWEET_DISPLAY_OPTIONS_HIDE_THREAD_OPTION_CODENAME = "hide_thread";

        public static IReadOnlyList<dynamic> Elements => new object[]
        {
            new
            {
                element = new {
                    id = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Title)).GetKontentElementId()
                },
                value = "On Roasts",
                codename = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Title)).GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName
            },
            new {
                 element = new {
                    id = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.PostDate)).GetKontentElementId()
                },
                value = new DateTime(2017, 7, 4),
                codename = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.PostDate)).GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName
            },
            new {
                element = new {
                    id = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.BodyCopy)).GetKontentElementId()
                },
                value = $@"
                        <h1>Light Roasts</h1>
                        <p>Usually roasted for 6 - 8 minutes or simply until achieving a light brown color.This method is used for milder coffee varieties and for coffee tasting.This type of roasting allows the natural characteristics of each coffee to show.The aroma of coffees produced from light roasts is usually more intense.The cup itself is more acidic and the concentration of caffeine is higher.</p>
                        <object type=""application/kenticocloud"" data-type=""component"" data-id=""{RICH_TEXT_COMPONENT_ID}""></object>
                        ",
                components = new[]
                {
                    new
                    {
                        id = RICH_TEXT_COMPONENT_ID,
                        type = new {
                            id = TWEET_TYPE_ID
                        },
                        elements = new dynamic[]
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
                                        NoExternalIdIdentifier.ByCodename(TWEET_THEME_ELEMENT_DARK_OPTION_CODENAME)
                                }
                            },
                            new
                            {
                                element = new {
                                    id = typeof(TweetTestModel).GetProperty(nameof(TweetTestModel.DisplayOptions)).GetKontentElementId()
                                },
                                value = new[] {
                                    NoExternalIdIdentifier.ByCodename(TWEET_DISPLAY_OPTIONS_HIDE_THREAD_OPTION_CODENAME)
                                }
                            }
                        }
                    }
                },
                codename = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.BodyCopy)).GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName
            },
            new {
                element = new {
                    id = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.RelatedArticles)).GetKontentElementId()
                },
                value = new[] { Reference.ById(EXISTING_ITEM_ID) },
                codename = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.RelatedArticles)).GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName
            },
            new {
                element = new {
                    id = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.UrlPattern)).GetKontentElementId()
                },
                mode = "custom",
                value = "on-roasts",
                codename = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.UrlPattern)).GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName
            },
            new {
                element = new {
                    id = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Personas)).GetKontentElementId()
                },
                value = new[] { NoExternalIdIdentifier.ByCodename(EXISTING_TAXONOMY_TERM_CODENAME) } ,
                codename = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Personas)).GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName
            },
            new {
                element = new {
                    id = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.TeaserImage)).GetKontentElementId()
                },
                value = new[]
                {
                    AssetIdentifier.ById(EXISTING_ASSET_ID),
                },
                codename = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.TeaserImage)).GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName
            },
            new {
                element = new {
                    id = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Options)).GetKontentElementId()
                },
                value = new[]
                {
                    NoExternalIdIdentifier.ByCodename(EXISTING_MULTIPLE_CHOICE_OPTION_CODENAME_PAID),
                    NoExternalIdIdentifier.ByCodename(EXISTING_MULTIPLE_CHOICE_OPTION_CODENAME_FEATURED)
                },
                codename = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Options)).GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName
            },
        };

        public static ComplexTestModel StronglyTypedElements => new()
        {
            Title = new TextElement
            {
                Element = ObjectIdentifier.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Title)).GetKontentElementId()),
                Value = "On Roast"
            },
            Rating = new NumberElement
            {
                Value = 3.14m,
                Element = ObjectIdentifier.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Rating)).GetKontentElementId()),
            },
            SelectedForm = new CustomElement
            {
                Element = ObjectIdentifier.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.SelectedForm)).GetKontentElementId()),
                Value = "{\"formId\": 42}",
                SearchableValue = "Almighty form!"
            },
            PostDate = new DateTimeElement
            {
                Element = ObjectIdentifier.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.PostDate)).GetKontentElementId()),
                Value = new DateTime(2017, 7, 4)
            },
            BodyCopy = new RichTextElement
            {
                Element = ObjectIdentifier.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.BodyCopy)).GetKontentElementId()),
                Value = $"<h1>Light Roasts</h1> <p>Usually roasted for 6 - 8 minutes or simply until achieving a light brown color.This method is used for milder coffee varieties and for coffee tasting.This type of roasting allows the natural characteristics of each coffee to show.The aroma of coffees produced from light roasts is usually more intense.The cup itself is more acidic and the concentration of caffeine is higher.</p><object type=\"application/kenticocloud\" data-type=\"component\" data-id=\"{RICH_TEXT_COMPONENT_ID}\"></object>",
                Components = new ComponentModel[]
                {
                    new ComponentModel
                    {
                        Id = RICH_TEXT_COMPONENT_ID,
                        Type = Reference.ById(TWEET_TYPE_ID),
                        Elements = new BaseElement[]
                        {
                            // TODO use exact Tweet values like in _elements (unify IDs to constants)
                            new TextElement
                            {
                                Element = ObjectIdentifier.ById(typeof(TweetTestModel).GetProperty(nameof(TweetTestModel.TweetLink)).GetKontentElementId()),
                                Value = "https://twitter.com/ChrastinaOndrej/status/1417105245935706123"
                            },
                            new MultipleChoiceElement
                            {
                                Element = ObjectIdentifier.ById(typeof(TweetTestModel).GetProperty(nameof(TweetTestModel.Theme)).GetKontentElementId()),
                                Value = new[] { NoExternalIdIdentifier.ById(TWEET_THEME_ELEMENT_DARK_OPTION_ID) },
                            },
                            new MultipleChoiceElement
                            {
                                Element = ObjectIdentifier.ById(typeof(TweetTestModel).GetProperty(nameof(TweetTestModel.DisplayOptions)).GetKontentElementId()),
                                Value = new[] { NoExternalIdIdentifier.ById(TWEET_DISPLAY_OPTIONS_HIDE_THREAD_OPTION_ID) },
                            }
                        }
                    }
                }
            },
            RelatedArticles = new LinkedItemsElement
            {
                Element = ObjectIdentifier.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.RelatedArticles)).GetKontentElementId()),
                Value = new[] { Reference.ById(EXISTING_ITEM_ID) }
            },
            UrlPattern = new UrlSlugElement
            {
                Element = ObjectIdentifier.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.UrlPattern)).GetKontentElementId()),
                Value = "on-roasts",
                Mode = "custom"
            },
            Personas = new TaxonomyElement
            {
                Element = ObjectIdentifier.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Personas)).GetKontentElementId()),
                Value = new[] { NoExternalIdIdentifier.ByCodename(EXISTING_TAXONOMY_TERM_CODENAME) }
            },
            TeaserImage = new AssetElement
            {
                Element = ObjectIdentifier.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.TeaserImage)).GetKontentElementId()),
                Value = new[] { AssetIdentifier.ById(EXISTING_ASSET_ID) },
            },
            Options = new MultipleChoiceElement
            {
                Element = ObjectIdentifier.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Options)).GetKontentElementId()),
                Value = new[]
                {
                    NoExternalIdIdentifier.ById(EXISTING_MULTIPLE_CHOICE_OPTION_ID_PAID),
                    NoExternalIdIdentifier.ById(EXISTING_MULTIPLE_CHOICE_OPTION_ID_FEATURED)
                }
            },
        };
    }
}
