using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using KenticoCloud.ContentManagement.Models.Items;
using KenticoCloud.ContentManagement.Modules.ActionInvoker;
using KenticoCloud.ContentManagement.Modules.HttpClient;

using Xunit;

namespace KenticoCloud.ContentManagement.Tests
{
    internal class FakeContentManagementHttpClient : IContentManagementHttpClient {
        internal string requestBody;

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage message) {
            requestBody = await message.Content.ReadAsStringAsync();
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
    }

    public class ActionInvokerTests
    {
        [Fact]
        [Trait("Issue", "29")]
        public void ActionInvokerSerializeElementContainingZero_SerializedJsonContainsZero() {
            var httpClient = new FakeContentManagementHttpClient();
            var actionInvoker = new ActionInvoker(httpClient, new MessageCreator("{api_key}"));
            
            var contentItemVariantUpsertModel = new ContentItemVariantUpsertModel() {
                Elements = new {
                    number = new decimal(0),
                },
            };

            var result = actionInvoker.InvokeMethodAsync<ContentItemVariantUpsertModel, dynamic>("{endpoint_url}", HttpMethod.Get,contentItemVariantUpsertModel);
            Assert.Equal("{\"elements\":{\"number\":0}}", httpClient.requestBody);
        }
    }
}
