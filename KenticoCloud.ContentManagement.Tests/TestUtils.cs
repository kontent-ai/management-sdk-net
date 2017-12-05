using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using KenticoCloud.ContentManagement.Models.Items;

namespace KenticoCloud.ContentManagement.Tests
{
    public class TestUtils
    {
        internal static async Task<ContentItemModel> PrepareTestItem(ContentManagementClient client, string typeCodename, string externalId = null)
        {
            var externalIdentifier = string.IsNullOrEmpty(externalId) ? Guid.NewGuid().ToString() : externalId;
            var type = ContentTypeIdentifier.ByCodename(typeCodename);
            var item = new ContentItemCreateModel() { Name = "Hooray!", Type = type, ExternalId = externalIdentifier };

            return await client.CreateContentItemAsync(item);
        }

        internal static async Task<ContentItemVariantModel> PrepareTestVariant(ContentManagementClient client, string languageCodename, Dictionary<string, object> elements, ContentItemModel item)
        {
            var addedItemIdentifier = ContentItemIdentifier.ByCodename(item.CodeName);
            var addedLanguageIdentifier = LanguageIdentifier.ByCodename(languageCodename);
            var addedContentItemLanguageIdentifier = new ContentItemVariantIdentifier(addedItemIdentifier, addedLanguageIdentifier);
            var variantUpdateModel = new ContentItemVariantUpsertModel() { Elements = elements };

            return await client.UpsertContentItemVariantAsync(addedContentItemLanguageIdentifier, variantUpdateModel);
        }
    }
}
