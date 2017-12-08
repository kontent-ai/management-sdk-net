using System.Threading.Tasks;
using System.Collections.Generic;

using KenticoCloud.ContentManagement.Models.Items;
using KenticoCloud.ContentManagement.Tests.Mocks;
using KenticoCloud.ContentManagement.Modules.ActionInvoker;

namespace KenticoCloud.ContentManagement.Tests
{
    public class TestUtils
    {
        internal enum TestRunType
        {
            LiveEndPoint,
            LiveEndPoint_SaveToFileSystem,
            MockFromFileSystem
        }
        
        internal static ContentManagementClient CreateContentManagementClient(ContentManagementOptions options, TestRunType runType, string testName)
        {
            if (runType != TestRunType.LiveEndPoint)
            {
                var saveToFileSystem = runType == TestRunType.LiveEndPoint_SaveToFileSystem;
                var httpClient = new FileSystemHttpClientMock(options, saveToFileSystem, testName);

                var urlBuilder = new EndpointUrlBuilder(options);
                var actionInvoker = new ActionInvoker(httpClient, new MessageCreator(options.ApiKey));

                return new ContentManagementClient(urlBuilder, actionInvoker);
            }

            return new ContentManagementClient(options);
        }

        internal static async Task<ContentItemModel> PrepareTestItem(ContentManagementClient client, string typeCodename, string externalId = null)
        {
            var type = ContentTypeIdentifier.ByCodename(typeCodename);
            var item = new ContentItemCreateModel() {
                Name = "Hooray!",
                Type = type,
                ExternalId = externalId
            };

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
