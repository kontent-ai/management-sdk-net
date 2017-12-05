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
        private const string PROJECT_ID = "bb6882a0-3088-405c-a6ac-4a0da46810b0";
        private const string API_KEY = "ew0KICAiYWxnIjogIkhTMjU2IiwNCiAgInR5cCI6ICJKV1QiDQp9.ew0KICAidWlkIjogInVzcl8wdkZrVm9rczdyb2prRmR2Slkzc0ZQIiwNCiAgImp0aSI6ICIwYzUyMTYxZGIwZTU0MDU5YjMwZjUwMGUyMTgxYmU1NiIsDQogICJpYXQiOiAiMTUxMTE3ODYwMCIsDQogICJleHAiOiAiMTUxMzc3MDYwMCIsDQogICJwcm9qZWN0X2lkIjogImJiNjg4MmEwMzA4ODQwNWNhNmFjNGEwZGE0NjgxMGIwIiwNCiAgInZlciI6ICIyLjAuMCIsDQogICJwZXJtaXNzaW9ucyI6IFsNCiAgICAidmlldy1jb250ZW50IiwNCiAgICAiY29tbWVudCIsDQogICAgInVwZGF0ZS13b3JrZmxvdyIsDQogICAgInVwZGF0ZS1jb250ZW50IiwNCiAgICAicHVibGlzaCIsDQogICAgImNvbmZpZ3VyZS1zaXRlbWFwIiwNCiAgICAiY29uZmlndXJlLXRheG9ub215IiwNCiAgICAiY29uZmlndXJlLWNvbnRlbnRfdHlwZXMiLA0KICAgICJjb25maWd1cmUtd2lkZ2V0cyIsDQogICAgImNvbmZpZ3VyZS13b3JrZmxvdyIsDQogICAgIm1hbmFnZS1wcm9qZWN0cyIsDQogICAgIm1hbmFnZS11c2VycyIsDQogICAgImNvbmZpZ3VyZS1wcmV2aWV3LXVybCIsDQogICAgImNvbmZpZ3VyZS1jb2RlbmFtZXMiLA0KICAgICJhY2Nlc3MtYXBpLWtleXMiLA0KICAgICJtYW5hZ2UtYXNzZXRzIiwNCiAgICAibWFuYWdlLWxhbmd1YWdlcyIsDQogICAgIm1hbmFnZS13ZWJob29rcyIsDQogICAgIm1hbmFnZS10cmFja2luZyINCiAgXSwNCiAgImF1ZCI6ICJtYW5hZ2Uua2VudGljb2Nsb3VkLmNvbSINCn0.qixIW1NOmyrbfsqoxzHG0NuMMg5yWtG9r0lpb5y31eo";

        private static ContentManagementOptions _options = new ContentManagementOptions() { ApiKey = API_KEY, ProjectId = PROJECT_ID };
        internal static ContentManagementClient client { get; private set; } = CreateContentManagementClient(TestRunType.LiveEndPoint_SaveToFileSystem);

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

        internal static async Task<ContentItemModel> PrepareItemToDelete(string typeCodename, string externalId = null)
        {
            var externalIdentifier = string.IsNullOrEmpty(externalId) ? Guid.NewGuid().ToString() : externalId;
            var type = ContentTypeIdentifier.ByCodename(typeCodename);
            var item = new ContentItemCreateModel() { Name = "Hooray!", Type = type, ExternalId = externalIdentifier };

            return await client.CreateContentItemAsync(item);
        }

        internal static async Task<ContentItemVariantModel> PrepareVariantToDelete(string languageCodename, Dictionary<string, object> elements, ContentItemModel item)
        {
            var addedItemIdentifier = ContentItemIdentifier.ByCodename(item.CodeName);
            var addedLanguageIdentifier = LanguageIdentifier.ByCodename(languageCodename);
            var addedContentItemLanguageIdentifier = new ContentItemVariantIdentifier(addedItemIdentifier, addedLanguageIdentifier);
            var variantUpdateModel = new ContentItemVariantUpsertModel() { Elements = elements };

            return await client.UpsertContentItemVariantAsync(addedContentItemLanguageIdentifier, variantUpdateModel);
        }
    }
}
