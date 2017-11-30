using System;
using System.Net.Http;
using System.Threading.Tasks;

using KenticoCloud.ContentManagement.Exceptions;

namespace KenticoCloud.ContentManagement.Modules.HttpClient
{
    internal class ContentManagementHttpClient : IContentManagementHttpClient
    {
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
            var response = await _httpClient.SendAsync(message);

            while ((int) response.StatusCode == 429)
            {
                var retryAfter = response.Headers.RetryAfter.Delta ?? TimeSpan.FromSeconds(1);
                await _delay.DelayByMs(retryAfter);

                response = await _httpClient.SendAsync(message);
            }

            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            throw new ContentManagementException(response, await response.Content.ReadAsStringAsync());
        }

    }
}
