using Kentico.Kontent.Management.Modules.HttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace Kentico.Kontent.Management.Modules.ActionInvoker
{
    internal class ActionInvoker : IActionInvoker
    {
        private IManagementHttpClient _cmHttpClient;
        private MessageCreator _messageCreator;

        private JsonSerializerSettings _serializeSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<JsonConverter> { new DecimalObjectConverter(), new StringEnumConverter() }
        };

        private JsonSerializerSettings _deserializeSettings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new DynamicObjectJsonConverter() }
        };


        public ActionInvoker(IManagementHttpClient cmHttpClient, MessageCreator messageCreator)
        {
            _cmHttpClient = cmHttpClient;
            _messageCreator = messageCreator;
        }


        private async Task<T> ReadResultAsync<T>(HttpResponseMessage response)
        {
            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseString, _deserializeSettings);
        }

        public async Task<TResponse> InvokeMethodAsync<TPayload, TResponse>(string endpointUrl, HttpMethod method, TPayload body)
        {
            var message = _messageCreator.CreateMessage(method, endpointUrl);

            HttpContent content = null;

            if (body != null)
            {
                string json = JsonConvert.SerializeObject(body, Formatting.None, _serializeSettings);
                content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var response = await _cmHttpClient.SendAsync(_messageCreator, endpointUrl, method, content);

            return await ReadResultAsync<TResponse>(response);
        }

        public async Task<TResponse> InvokeReadOnlyMethodAsync<TResponse>(string endpointUrl, HttpMethod method, Dictionary<string, string> headers = null)
        {
            var message = _messageCreator.CreateMessage(method, endpointUrl, null, headers);

            var response = await _cmHttpClient.SendAsync(_messageCreator, endpointUrl, method, null, headers);

            return await ReadResultAsync<TResponse>(response);
        }

        public async Task InvokeMethodAsync(string endpointUrl, HttpMethod method, Dictionary<string, string> headers = null)
        {
            var message = _messageCreator.CreateMessage(method, endpointUrl, null, headers);
            await _cmHttpClient.SendAsync(_messageCreator, endpointUrl, method, null, headers);
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

            var response = await _cmHttpClient.SendAsync(_messageCreator, endpointUrl, HttpMethod.Post, content);

            return await ReadResultAsync<TResponse>(response);
        }
    }
}
