using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;

using KenticoCloud.ContentManagement.Modules.HttpClient;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Modules.ActionInvoker
{
    internal class ActionInvoker : IActionInvoker
    {
        private ContentManagementHttpClient _cmHttpClient;
        private MessageCreator _messageCreator;

        private JsonSerializerSettings _serializeSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

        public ActionInvoker(ContentManagementHttpClient cmHttpClient, MessageCreator messageCreator)
        {
            _cmHttpClient = cmHttpClient;
            _messageCreator = messageCreator;
        }


        public ActionInvoker()
        {
            _cmHttpClient = new ContentManagementHttpClient();
        }

        private async Task<T> ReadResultAsync<T>(HttpResponseMessage response)
        {
            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseString);
        }

        public async Task<TResponse> InvokeMethodAsync<TPayload, TResponse>(string endpointUrl, HttpMethod method, TPayload body)
        {
            var message = _messageCreator.CreateMessage(method, endpointUrl);

            if (body != null)
            {
                string json = JsonConvert.SerializeObject(body, Formatting.None, _serializeSettings);
                message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var response = await _cmHttpClient.SendAsync(message);

            return await ReadResultAsync<TResponse>(response);
        }

        public async Task<TResponse> InvokeReadOnlyMethodAsync<TResponse>(string endpointUrl, HttpMethod method)
        {
            var message = _messageCreator.CreateMessage(method, endpointUrl);

            var response = await _cmHttpClient.SendAsync(message);

            return await ReadResultAsync<TResponse>(response);
        }

        public async Task InvokeMethodAsync(string endpointUrl, HttpMethod method)
        {
            var message = _messageCreator.CreateMessage(method, endpointUrl);
            await _cmHttpClient.SendAsync(message);
        }

        public async Task<TResponse> UploadFileAsync<TResponse>(string endpointUrl, Stream stream, string contentType)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var message = _messageCreator.CreateMessage(HttpMethod.Post, endpointUrl);

            var content = new StreamContent(stream);

            content.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
            content.Headers.ContentLength = stream.Length;

            message.Content = content;

            var response = await _cmHttpClient.SendAsync(message);

            return await ReadResultAsync<TResponse>(response);
        }
    }
}
