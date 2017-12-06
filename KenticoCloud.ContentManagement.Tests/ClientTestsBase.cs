using System;
using System.Collections.Generic;

using KenticoCloud.ContentManagement.Modules.ActionInvoker;
using KenticoCloud.ContentManagement.Modules.ModelBuilders;
using KenticoCloud.ContentManagement.Tests.Mocks;

namespace KenticoCloud.ContentManagement.Tests
{
    public class ClientTestsBase
    {   
        // Tests configuration
        private const string PROJECT_ID = "49f108e5-2e7e-4405-8369-7e0cf92576f2";
        private const string API_KEY = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1aWQiOiJ1c3JfMHZGa1Zva3M3cm9qa0ZkdkpZM3NGUCIsImp0aSI6ImRjZmFkYzA2OTQ4YTQwNTk4MGI0YTIwYmEzNmIyODZmIiwiaWF0IjoiMTUxMjQ3NDc4OSIsImV4cCI6IjE1MTUwNjY3ODkiLCJwcm9qZWN0X2lkIjoiNDlmMTA4ZTUyZTdlNDQwNTgzNjk3ZTBjZjkyNTc2ZjIiLCJ2ZXIiOiIyLjAuMCIsInBlcm1pc3Npb25zIjpbInZpZXctY29udGVudCIsImNvbW1lbnQiLCJ1cGRhdGUtd29ya2Zsb3ciLCJ1cGRhdGUtY29udGVudCIsInB1Ymxpc2giLCJjb25maWd1cmUtc2l0ZW1hcCIsImNvbmZpZ3VyZS10YXhvbm9teSIsImNvbmZpZ3VyZS1jb250ZW50X3R5cGVzIiwiY29uZmlndXJlLXdpZGdldHMiLCJjb25maWd1cmUtd29ya2Zsb3ciLCJtYW5hZ2UtcHJvamVjdHMiLCJtYW5hZ2UtdXNlcnMiLCJjb25maWd1cmUtcHJldmlldy11cmwiLCJjb25maWd1cmUtY29kZW5hbWVzIiwiYWNjZXNzLWFwaS1rZXlzIiwibWFuYWdlLWFzc2V0cyIsIm1hbmFnZS1sYW5ndWFnZXMiLCJtYW5hZ2Utd2ViaG9va3MiLCJtYW5hZ2UtdHJhY2tpbmciXSwiYXVkIjoibWFuYWdlLmtlbnRpY29jbG91ZC5jb20ifQ.x4_dk2ECfhKZLayxrYOEvwMiArM21CNFdKTMmxI4tiA";
        private static ContentManagementOptions _options = new ContentManagementOptions() { ApiKey = API_KEY, ProjectId = PROJECT_ID };

        // Test constants
        protected static Guid EXISTING_ITEM_ID = Guid.Parse("3120ec15-a4a2-47ec-8ccd-c85ac8ac5ba5");
        protected const string EXISTING_ITEM_CODENAME = "which_brewing_fits_you_";
        protected static Guid EXISTING_LANGUAGE_ID = Guid.Parse("d1f95fde-af02-b3b5-bd9e-f232311ccab8");
        protected const string EXISTING_LANGUAGE_CODENAME = "es-ES";
        protected const string EXISTING_CONTENT_TYPE_CODENAME = "article";
        protected static Dictionary<string, object> _elements = new Dictionary<string, object> { { "title", "On Roasts" } };

        internal static ContentManagementClient GetContentManagementClient(TestRunType runType, IModelProvider modelProvider = null)
        {
            if (runType != TestRunType.LiveEndPoint)
            {
                var urlBuilder = new EndpointUrlBuilder(_options);
                var httpClient = new FileSystemHttpClientMock(runType == TestRunType.LiveEndPoint_SaveToFileSystem);
                var actionInvoker = new ActionInvoker(httpClient, new MessageCreator(_options.ApiKey));

                return new ContentManagementClient(urlBuilder, actionInvoker, modelProvider);
            }

            return new ContentManagementClient(_options);
        }

        internal enum TestRunType
        {
            LiveEndPoint,
            LiveEndPoint_SaveToFileSystem,
            MockFromFileSystem
        }
    }
}
