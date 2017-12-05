using KenticoCloud.ContentManagement.Models.Items;

using Xunit;

namespace KenticoCloud.ContentManagement.Tests
{
    public class ContentManagementClientTestsLiveEndpointOnly : ClientTestsBase
    {
        private static ContentManagementClient _client;

        /// <summary>
        /// Setup
        /// </summary>
        public ContentManagementClientTestsLiveEndpointOnly()
        {
            _client = GetContentManagementClient(TestRunType.LiveEndPoint_SaveToFileSystem);
        }

        [Fact]
        [Trait("Category", "LiveEndpointOnly")]
        public async void DeleteContentAsync_ById_DeletesContentItem()
        {
            var itemToDelete = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME);

            var identifier = ContentItemIdentifier.ById(itemToDelete.Id);

            await _client.DeleteContentItemAsync(identifier);
        }

        [Fact]
        [Trait("Category", "LiveEndpointOnly")]
        // Caused by DEL-1460
        public async void DeleteContentAsync_ByCodename_DeletesContentItem()
        {
            var itemToDelete = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME);

            var identifier = ContentItemIdentifier.ByCodename(itemToDelete.CodeName);

            await _client.DeleteContentItemAsync(identifier);
        }

        [Fact]
        [Trait("Category", "LiveEndpointOnly")]
        public async void DeleteContentItemVariant_ByCodename_LanguageId_DeletesVariant()
        {
            // Prepare item
            var itemResponse = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME);
            await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, _elements, itemResponse);

            var itemIdentifier = ContentItemIdentifier.ByCodename(itemResponse.CodeName);
            var languageIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.DeleteContentItemVariantAsync(identifier);
        }

        [Fact]
        [Trait("Category", "LiveEndpointOnly")]
        public async void DeleteContentItemVariant_ByCodename_LanguageCodename_DeletesVariant()
        {
            // Prepare item
            var itemResponse = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME);
            await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, _elements, itemResponse);

            var itemIdentifier = ContentItemIdentifier.ByCodename(itemResponse.CodeName);
            var languageIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.DeleteContentItemVariantAsync(identifier);
        }

        [Fact]
        [Trait("Category", "LiveEndpointOnly")]
        public async void DeleteContentItemVariant_ById_LanguageCodename_DeletesVariant()
        {
            var itemResponse = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME);
            await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, _elements, itemResponse);

            var itemIdentifier = ContentItemIdentifier.ById(itemResponse.Id);
            var languageIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.DeleteContentItemVariantAsync(identifier);
        }
    }
}
