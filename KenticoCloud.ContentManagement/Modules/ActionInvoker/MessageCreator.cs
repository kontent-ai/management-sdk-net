using System.Net.Http;
using System.Net.Http.Headers;

namespace KenticoCloud.ContentManagement.Modules.ActionInvoker
{
    internal class MessageCreator
    {
        private readonly string _apiKey;

        public MessageCreator(string apiKey)
        {
            _apiKey = apiKey;
        }

        public HttpRequestMessage CreateMessage(HttpMethod method, string url)
        {
            var message = new HttpRequestMessage(method, url);
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            return message;
        } 
    }
}
