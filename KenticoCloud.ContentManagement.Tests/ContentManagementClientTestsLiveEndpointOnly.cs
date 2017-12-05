using System;
using System.Collections.Generic;

using KenticoCloud.ContentManagement.Models.Items;

using Xunit;

namespace KenticoCloud.ContentManagement.Tests
{
    public class ContentManagementClientTestsLiveEndpointOnly
    {
        private static ContentManagementClient _client = TestUtils.client;

        private static Guid EXISTING_ITEM_ID = Guid.Parse("3120ec15-a4a2-47ec-8ccd-c85ac8ac5ba5");
        private const string EXISTING_ITEM_CODENAME = "which_brewing_fits_you_";
        private static Guid EXISTING_LANGUAGE_ID = Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8");
        private const string EXISTING_LANGUAGE_CODENAME = "es-ES";
        private const string EXISTING_CONTENT_TYPE_CODENAME = "article";

        private static Dictionary<string, object> _elements = new Dictionary<string, object> { { "title", "On Roasts" } };


        [Fact]
        [Trait("Category", "LiveEndpointOnly")]
        public async void DeleteContentAsync_ById_DeletesContentItem()
        {
            var itemToDelete = await TestUtils.PrepareTestItem(EXISTING_CONTENT_TYPE_CODENAME);

            var identifier = ContentItemIdentifier.ById(itemToDelete.Id);

            await _client.DeleteContentItemAsync(identifier);
        }

        [Fact]
        [Trait("Category", "LiveEndpointOnly")]
        // Caused by DEL-1460
        public async void DeleteContentAsync_ByCodename_DeletesContentItem()
        {
            var itemToDelete = await TestUtils.PrepareTestItem(EXISTING_CONTENT_TYPE_CODENAME);

            var identifier = ContentItemIdentifier.ByCodename(itemToDelete.CodeName);

            await _client.DeleteContentItemAsync(identifier);
        }

        [Fact]
        [Trait("Category", "LiveEndpointOnly")]
        public async void DeleteContentItemVariant_ByCodename_LanguageId_DeletesVariant()
        {
            // Prepare item
            var itemResponse = await TestUtils.PrepareTestItem(EXISTING_CONTENT_TYPE_CODENAME);
            await TestUtils.PrepareTestVariant(EXISTING_LANGUAGE_CODENAME, _elements, itemResponse);

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
            var itemResponse = await TestUtils.PrepareTestItem(EXISTING_CONTENT_TYPE_CODENAME);
            await TestUtils.PrepareTestVariant(EXISTING_LANGUAGE_CODENAME, _elements, itemResponse);

            var itemIdentifier = ContentItemIdentifier.ByCodename(itemResponse.CodeName);
            var languageIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.DeleteContentItemVariantAsync(identifier);
        }

        [Fact]
        [Trait("Category", "LiveEndpointOnly")]
        public async void DeleteContentItemVariant_ById_LanguageCodename_DeletesVariant()
        {
            var itemResponse = await TestUtils.PrepareTestItem(EXISTING_CONTENT_TYPE_CODENAME);
            await TestUtils.PrepareTestVariant(EXISTING_LANGUAGE_CODENAME, _elements, itemResponse);

            var itemIdentifier = ContentItemIdentifier.ById(itemResponse.Id);
            var languageIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.DeleteContentItemVariantAsync(identifier);
        }
    }
}
