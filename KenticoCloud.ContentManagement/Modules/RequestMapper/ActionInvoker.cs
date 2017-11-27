using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using KenticoCloud.ContentManagement.Modules.HttpClient;
using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Modules.RequestMapper
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

        public async Task<TU> InvokeMethodAsync<T, TU>(string endpointUrl, HttpMethod method, T body)
        {
            var message = _messageCreator.CreateMessage(method, endpointUrl);
            if (body != null)
            {
                string json = JsonConvert.SerializeObject(body, Formatting.None, _serializeSettings);
                message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var response = await _cmHttpClient.SendAsync(message);

            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TU>(responseString);        // TODO: handle exception
        }

        public async Task<T> InvokeReadOnlyMethodAsync<T>(string endpointUrl, HttpMethod method)
        {
            var message = _messageCreator.CreateMessage(method, endpointUrl);

            var response = await _cmHttpClient.SendAsync(message);

            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseString);        // TODO: handle exception
        }

        public async Task InvokeMethodAsync(string endpointUrl, HttpMethod method)
        {
            var message = _messageCreator.CreateMessage(method, endpointUrl);
            await _cmHttpClient.SendAsync(message);
        }

    }
}
