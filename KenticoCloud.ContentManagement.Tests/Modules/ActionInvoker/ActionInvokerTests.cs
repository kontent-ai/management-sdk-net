using KenticoCloud.ContentManagement.Models.Items;
using KenticoCloud.ContentManagement.Modules.ActionInvoker;
using KenticoCloud.ContentManagement.Modules.HttpClient;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace KenticoCloud.ContentManagement.Tests
{
    internal class FakeContentManagementHttpClient : IContentManagementHttpClient
    {
        internal string requestBody;

        public async Task<HttpResponseMessage> SendAsync(IMessageCreator messageCreator, string endpointUrl, HttpMethod method, HttpContent content = null)
        {
            var message = messageCreator.CreateMessage(method, endpointUrl, content);
            requestBody = await message.Content.ReadAsStringAsync();
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
    }

    public class ActionInvokerTests
    {
        [Theory]
        [Trait("Issue", "29")]
        [InlineData(0.0, "0")]
        [InlineData(29.0, "29.0")]
        public void ActionInvokerSerializeElementContainingZero_SerializedJsonContainsZero(decimal d, string s)
        {
            var httpClient = new FakeContentManagementHttpClient();
            var actionInvoker = new ActionInvoker(httpClient, new MessageCreator("{api_key}"));

            var contentItemVariantUpsertModel = new ContentItemVariantUpsertModel()
            {
                Elements = new
                {
                    zero = d,
                    optZero = new decimal?(d),
                },
            };

            var result = actionInvoker.InvokeMethodAsync<ContentItemVariantUpsertModel, dynamic>("{endpoint_url}", HttpMethod.Get, contentItemVariantUpsertModel);
            Assert.Equal($"{{\"elements\":{{\"zero\":{s},\"optZero\":{s}}}}}", httpClient.requestBody);
        }
    }
}
