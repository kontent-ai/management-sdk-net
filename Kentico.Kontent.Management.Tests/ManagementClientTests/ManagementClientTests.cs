using System;
using Microsoft.Extensions.Configuration;


namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    public partial class ManagementClientTests
    {
        /// <summary>
        /// ID of the project the test data are generated from.
        /// </summary>
        private const string PROJECT_ID = "a9931a80-9af4-010b-0590-ecb1273cf1b8";

        private readonly ManagementOptions _options;

        /// <summary>
        /// Allows to adjust the test run type to achieve the desired behavior (see the <see cref="TestUtils.TestRunType"/> enum for more details).
        /// IMPORTANT: Commit always with TestRunType.MockFromFileSystem
        /// </summary>
        private static readonly TestUtils.TestRunType _runType = TestUtils.TestRunType.MockFromFileSystem;

        public ManagementClientTests()
        {
            // Load configuration from user secrets
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<ManagementClientTests>()
                .Build();

            // Configurations with TestRunType.LiveEndpoint* require the ApiKey property set in the user secrets
            // Dummy_API_key fallback is needed e.g. for running tests on mocked data - we need to properly init client
            _options = new ManagementOptions() { ApiKey = configuration.GetValue<string>("ApiKey") ?? "Dummy_API_key", ProjectId = configuration.GetValue<string>("ProjectId") ?? PROJECT_ID };
        }

        // Test constants, existing data references leverage the Dancing Goat sample site project that is generated for everyone
        protected static readonly Guid EXISTING_ITEM_ID = Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515");
        protected const string EXISTING_ITEM_CODENAME = "on_roasts";

        protected static readonly Guid EXISTING_ITEM_ID2 = Guid.Parse("0f79b41f-53d3-41b5-8fa0-153c87f60bc1");

        protected static readonly Guid EXISTING_LANGUAGE_ID = Guid.Parse("78dbefe8-831b-457e-9352-f4c4eacd5024");
        protected const string EXISTING_LANGUAGE_CODENAME = "es-ES";

        protected static readonly Guid EXISTING_CONTENT_TYPE_ID = Guid.Parse("ba5cd79d-6d4b-5bed-a681-6aa3a366c8f7");
        protected const string EXISTING_CONTENT_TYPE_CODENAME = "article";

        protected static readonly Guid EXISTING_TAXONOMY_TERM_ID = Guid.Parse("6e8b18d5-c5e3-5fc1-9014-44c18ef5f5d8");
        protected const string EXISTING_TAXONOMY_TERM_CODENAME = "barista";

        protected static Guid EXISTING_TAXONOMY_GROUP_ID = Guid.Parse("f6851a6e-a342-5253-8bc6-e8abc8f56b15");
        protected const string EXISTING_TAXONOMY_GROUP_CODENAME = "manufacturer";

        protected const string EXISTING_WEBHOOK_NAME = "Webhook_all_triggers";
        protected const string EXISTING_WEBHOOK_ID = "ef6eb5e6-24f6-4a3a-a291-1cff8d4eecc5";


        protected static readonly Guid CUSTOM_WORKFLOW_STEP_ID = Guid.Parse("0fabe3b3-d366-4bf9-be41-d37d4b4c2bcb");
        protected static readonly Guid DRAFT_WORKFLOW_STEP_ID = Guid.Parse("eee6db3b-545a-4785-8e86-e3772c8756f9");
        protected static readonly Guid PUBLISHED_WORKFLOW_STEP_ID = Guid.Parse("c199950d-99f0-4983-b711-6c4c91624b22");
        protected static readonly Guid ARCHIVED_WORKFLOW_STEP_ID = Guid.Parse("7a535a69-ad34-47f8-806a-def1fdf4d391");
        protected static readonly Guid SCHEDULED_WORKFLOW_STEP_ID = Guid.Parse("9d2b0228-4d0d-4c23-8b49-01a698857709");


        protected const string EXISTING_MULTIPLE_CHOICE_OPTION_CODENAME_PAID = "paid";
        protected static readonly Guid EXISTING_MULTIPLE_CHOICE_OPTION_ID_PAID = Guid.Parse("00c0f86a-7c51-4e60-abeb-a150e9092e53");
        protected const string EXISTING_MULTIPLE_CHOICE_OPTION_CODENAME_FEATURED = "featured";
        protected static readonly Guid EXISTING_MULTIPLE_CHOICE_OPTION_ID_FEATURED = Guid.Parse("8972dc90-ae2e-416e-995d-95df6c77e3b2");

        // Root -> 0ce98752-a614-51a9-bf69-9539deb6532d > 04bf910c-bcac-5faf-ac32-a1f7169fdc0f > e2fe0a21-eb4c-5fba-8a28-697aeab81f83 -> ae11f9dd-ec34-5ecc-9b83-d4a3ae1d8c6b
        protected const string ASSET_FOLDER_ID_1ST_LEVEL = "0ce98752-a614-51a9-bf69-9539deb6532d";
        protected const string ASSET_FOLDER_ID_2ND_LEVEL = "04bf910c-bcac-5faf-ac32-a1f7169fdc0f";
        protected const string ASSET_FOLDER_ID_3RD_LEVEL = "e2fe0a21-eb4c-5fba-8a28-697aeab81f83";
        protected const string ASSET_FOLDER_ID_4TH_LEVEL = "ae11f9dd-ec34-5ecc-9b83-d4a3ae1d8c6b";
        protected static readonly Guid EXISTING_ASSET_ID = Guid.Parse("5c08a538-5b58-44eb-81ef-43fb37eeb815");
        protected static readonly Guid RICH_TEXT_COMPONENT_ID = Guid.Parse("46c05bd9-d418-4507-836c-9accc5a39db3");
        protected static readonly Guid TWEET_TYPE_ID = Guid.Parse("17ff8a28-ebe6-5c9d-95ea-18fe1ff86d2d");
        protected static readonly Guid TWEET_THEME_ELEMENT_DARK_OPTION_ID = Guid.Parse("061e69f7-0965-5e37-97bc-29963cfaebe8");
        protected const string TWEET_THEME_ELEMENT_DARK_OPTION_CODENAME = "dark";
        protected static readonly Guid TWEET_DISPLAY_OPTIONS_HIDE_THREAD_OPTION_ID = Guid.Parse("dd78b09e-4337-599c-9701-20a0a165c63b");
        protected const string TWEET_DISPLAY_OPTIONS_HIDE_THREAD_OPTION_CODENAME = "hide_thread";

        private ManagementClient CreateManagementClient(string testName)
        {
            return TestUtils.CreateManagementClient(_options, _runType, testName);
        }
    }
}
