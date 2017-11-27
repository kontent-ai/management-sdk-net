using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace KenticoCloud.ContentManagement.Modules.RequestMapper
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
