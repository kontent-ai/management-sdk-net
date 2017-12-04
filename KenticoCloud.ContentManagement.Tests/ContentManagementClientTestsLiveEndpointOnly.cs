using System;
using System.Collections.Generic;

using KenticoCloud.ContentManagement.Models.Items;

using Xunit;

namespace KenticoCloud.ContentManagement.Tests
{
    public class ContentManagementClientTestsLiveEndpointOnly
    {
        private static ContentManagementClient _client = TestUtils.client;

        private static Guid EXISTING_ITEM_ID = Guid.Parse("ddc8f48a-6df3-43c6-9933-0d4ea0b2c701");
        private const string EXISTING_ITEM_CODENAME = "introduction";
        private static Guid EXISTING_LANGUAGE_ID = Guid.Parse("5f148588-613f-8e32-c023-da82f1308ede");
        private const string EXISTING_LANGUAGE_CODENAME = "Another_language";
        private const string EXISTING_CONTENT_TYPE_CODENAME = "writeapi";
        private const string EXISTING_EXTERNAL_ID = "354136c543gj3154354j1g";
        private static Dictionary<string, object> _elements = new Dictionary<string, object> { { "name", "Martinko Klingacik44" } };


        [Fact]
        [Trait("Category", "LiveEndpointOnly")]
        public async void DeleteContentAsync_ById_DeletesContentItem()
        {
            var itemToDelete = await TestUtils.PrepareItemToDelete("writeapi");

            var identifier = ContentItemIdentifier.ById(itemToDelete.Id);

            await _client.DeleteContentItemAsync(identifier);
        }

        [Fact]
        [Trait("Category", "LiveEndpointOnly")]
        public async void DeleteContentAsync_ByCodename_DeletesContentItem()
        {
            var itemToDelete = await TestUtils.PrepareItemToDelete("writeapi");

            var identifier = ContentItemIdentifier.ByCodename(itemToDelete.CodeName);

            await _client.DeleteContentItemAsync(identifier);
        }

        [Fact]
        [Trait("Category", "LiveEndpointOnly")]
        public async void DeleteContentItemVariant_ByCodename_LanguageId_DeletesVariant()
        {
            // Prepare item
            var itemResponse = await TestUtils.PrepareItemToDelete(EXISTING_CONTENT_TYPE_CODENAME);
            await TestUtils.PrepareVariantToDelete(EXISTING_LANGUAGE_CODENAME, _elements, itemResponse);

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
            var itemResponse = await TestUtils.PrepareItemToDelete(EXISTING_CONTENT_TYPE_CODENAME);
            await TestUtils.PrepareVariantToDelete(EXISTING_LANGUAGE_CODENAME, _elements, itemResponse);

            var itemIdentifier = ContentItemIdentifier.ByCodename(itemResponse.CodeName);
            var languageIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.DeleteContentItemVariantAsync(identifier);
        }

        [Fact]
        [Trait("Category", "LiveEndpointOnly")]
        public async void DeleteContentItemVariant_ById_LanguageCodename_DeletesVariant()
        {
            var itemResponse = await TestUtils.PrepareItemToDelete(EXISTING_CONTENT_TYPE_CODENAME);
            await TestUtils.PrepareVariantToDelete(EXISTING_LANGUAGE_CODENAME, _elements, itemResponse);

            var itemIdentifier = ContentItemIdentifier.ById(itemResponse.Id);
            var languageIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.DeleteContentItemVariantAsync(identifier);
        }
    }
}
