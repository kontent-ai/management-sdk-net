using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using KenticoCloud.ContentManagement.Models.Items;
using KenticoCloud.ContentManagement.Tests.Mocks;
using KenticoCloud.ContentManagement.Modules.ActionInvoker;

namespace KenticoCloud.ContentManagement.Tests
{
    public class TestUtils
    {
        private const string PROJECT_ID = "49f108e5-2e7e-4405-8369-7e0cf92576f2";
        private const string API_KEY = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1aWQiOiJ1c3JfMHZGa1Zva3M3cm9qa0ZkdkpZM3NGUCIsImp0aSI6ImRjZmFkYzA2OTQ4YTQwNTk4MGI0YTIwYmEzNmIyODZmIiwiaWF0IjoiMTUxMjQ3NDc4OSIsImV4cCI6IjE1MTUwNjY3ODkiLCJwcm9qZWN0X2lkIjoiNDlmMTA4ZTUyZTdlNDQwNTgzNjk3ZTBjZjkyNTc2ZjIiLCJ2ZXIiOiIyLjAuMCIsInBlcm1pc3Npb25zIjpbInZpZXctY29udGVudCIsImNvbW1lbnQiLCJ1cGRhdGUtd29ya2Zsb3ciLCJ1cGRhdGUtY29udGVudCIsInB1Ymxpc2giLCJjb25maWd1cmUtc2l0ZW1hcCIsImNvbmZpZ3VyZS10YXhvbm9teSIsImNvbmZpZ3VyZS1jb250ZW50X3R5cGVzIiwiY29uZmlndXJlLXdpZGdldHMiLCJjb25maWd1cmUtd29ya2Zsb3ciLCJtYW5hZ2UtcHJvamVjdHMiLCJtYW5hZ2UtdXNlcnMiLCJjb25maWd1cmUtcHJldmlldy11cmwiLCJjb25maWd1cmUtY29kZW5hbWVzIiwiYWNjZXNzLWFwaS1rZXlzIiwibWFuYWdlLWFzc2V0cyIsIm1hbmFnZS1sYW5ndWFnZXMiLCJtYW5hZ2Utd2ViaG9va3MiLCJtYW5hZ2UtdHJhY2tpbmciXSwiYXVkIjoibWFuYWdlLmtlbnRpY29jbG91ZC5jb20ifQ.x4_dk2ECfhKZLayxrYOEvwMiArM21CNFdKTMmxI4tiA";

        private static ContentManagementOptions _options = new ContentManagementOptions() { ApiKey = API_KEY, ProjectId = PROJECT_ID };
        internal static ContentManagementClient client { get; private set; } = CreateContentManagementClient(TestRunType.MockFromFileSystem);

        private static ContentManagementClient CreateContentManagementClient(TestRunType runType)
        {
            if (runType != TestRunType.LiveEndPoint)
            {
                var urlBuilder = new EndpointUrlBuilder(_options);
                var httpClient = new FileSystemHttpClientMock(runType == TestRunType.LiveEndPoint_SaveToFileSystem);
                var actionInvoker = new ActionInvoker(httpClient, new MessageCreator(_options.ApiKey));

                return new ContentManagementClient(urlBuilder, actionInvoker);
            }

            return new ContentManagementClient(_options);
        }

        private enum TestRunType
        {
            LiveEndPoint,
            LiveEndPoint_SaveToFileSystem,
            MockFromFileSystem
        }

        internal static async Task<ContentItemModel> PrepareTestItem(string typeCodename, string externalId = null)
        {
            var externalIdentifier = string.IsNullOrEmpty(externalId) ? Guid.NewGuid().ToString() : externalId;
            var type = ContentTypeIdentifier.ByCodename(typeCodename);
            var item = new ContentItemCreateModel() { Name = "Hooray!", Type = type, ExternalId = externalIdentifier };

            return await client.CreateContentItemAsync(item);
        }

        internal static async Task<ContentItemVariantModel> PrepareTestVariant(string languageCodename, Dictionary<string, object> elements, ContentItemModel item)
        {
            var addedItemIdentifier = ContentItemIdentifier.ByCodename(item.CodeName);
            var addedLanguageIdentifier = LanguageIdentifier.ByCodename(languageCodename);
            var addedContentItemLanguageIdentifier = new ContentItemVariantIdentifier(addedItemIdentifier, addedLanguageIdentifier);
            var variantUpdateModel = new ContentItemVariantUpsertModel() { Elements = elements };

            return await client.UpsertContentItemVariantAsync(addedContentItemLanguageIdentifier, variantUpdateModel);
        }
    }
}
