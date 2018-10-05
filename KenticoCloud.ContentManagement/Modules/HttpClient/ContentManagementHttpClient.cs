using System;
using System.Net.Http;
using System.Threading.Tasks;

using KenticoCloud.ContentManagement.Exceptions;
using KenticoCloud.ContentManagement.Modules.Extensions;

namespace KenticoCloud.ContentManagement.Modules.HttpClient
{
    internal class ContentManagementHttpClient : IContentManagementHttpClient
    {
        private const int HTTP_CODE_TOO_MANY_REQUESTS = 429;

        private IHttpClient _httpClient;
        private IDelay _delay;

        public ContentManagementHttpClient(IDelay delay, IHttpClient httpClient)
        {
            _delay = delay;
            _httpClient = httpClient;
        }

        public ContentManagementHttpClient()
        {
            _delay = new Delay();
            _httpClient = new HttpClient();
        }


        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage message)
        {
            message.Headers.AddSdkTrackingHeader();
            var response = await _httpClient.SendAsync(message);

            while ((int)response.StatusCode == HTTP_CODE_TOO_MANY_REQUESTS)
            {
                var retryAfter = response.Headers.RetryAfter.Delta ?? TimeSpan.FromSeconds(1);
                await _delay.DelayByTimeSpan(retryAfter);

                response = await _httpClient.SendAsync(message);
            }

            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            var content = response.Content;

            throw (content != null) ?
                new ContentManagementException(response, await response.Content.ReadAsStringAsync()) :
                new ContentManagementException(response, "CM API returned server error.");
        }

    }
}
