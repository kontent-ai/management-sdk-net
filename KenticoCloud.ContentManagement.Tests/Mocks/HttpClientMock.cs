using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;

namespace KenticoCloud.ContentManagement.Tests.Mocks
{
    public class HttpClientMock
    {
        private readonly string _endpointUrl;

        private MockHttpMessageHandler itemsHandler => new MockHttpMessageHandler();
        private MockHttpMessageHandler mockHandler;

        public HttpClientMock(string endpointUrl)
        {
            _endpointUrl = endpointUrl;
            mockHandler = new MockHttpMessageHandler();
        }

        public HttpRequestMessage lastMessage;

        public HttpClientMock SetUpGetItemToSuccess()
        {
            mockHandler.When($"{_endpointUrl}/items/*").With(matcher => matcher.Method == HttpMethod.Get).Respond(x =>
            {
                lastMessage = x;
                var response = new HttpResponseMessage
                {
                    Content = new StringContent(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Data\\contentItem.json")))
                };
                return response;
            });
            return this;
        }

        public HttpClientMock SetUpGetItemToFail()
        {

            mockHandler.When($"{_endpointUrl}/items/*").With(matcher => matcher.Method == HttpMethod.Get).Respond(x =>
            {
                lastMessage = x;
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound

                };
            });
            return this;
        }

        public HttpClientMock SetUpUpdateItemToSuccess()
        {
            mockHandler.When($"{_endpointUrl}/items/*").With(matcher => matcher.Method == HttpMethod.Put).Respond(x =>
            {
                lastMessage = x;
                var response = new HttpResponseMessage
                {
                    Content = new StringContent(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Data\\contentItem.json")))
                };
                return response;
            });
            return this;
        }

        public HttpClientMock SetRetry()
        {
            var retried = 2;
            var retryMessage = new HttpResponseMessage() {StatusCode = (HttpStatusCode) 429};
            retryMessage.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromMilliseconds(1000));
            mockHandler.When("*").With(x => --retried > 0).Respond(x => retryMessage);
            return this;
        }

        public HttpClient Build()
        {
            return mockHandler.ToHttpClient();
        }

    }
}
