using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using KenticoCloud.ContentManagement.Exceptions;
using KenticoCloud.ContentManagement.Modules.HttpClient;

using NSubstitute;
using Xunit;

namespace KenticoCloud.ContentManagement.Tests
{
    public class ContentManagementHttpClientTests
    {
        private ContentManagementHttpClient _client;
        private IHttpClient httpClient = Substitute.For<IHttpClient>();
        private IDelay delay = Substitute.For<IDelay>();

        public ContentManagementHttpClientTests()
        {
            _client = new ContentManagementHttpClient(delay, httpClient);
        }

        [Fact]
        public async Task SendAsync_SendsMessageOnSuccessReturnsResponse()
        {
            var successfulMessage = new HttpRequestMessage();
            var successfulResponse = new HttpResponseMessage {StatusCode = System.Net.HttpStatusCode.OK};
            httpClient.SendAsync(Arg.Is(successfulMessage)).Returns(successfulResponse);

            var response = await _client.SendAsync(successfulMessage);

            await httpClient.Received().SendAsync(successfulMessage);
            Assert.Equal(successfulResponse, response);

        }

        [Fact]
        public async Task SendAsync_SendsMessageOnFailureThrows()
        {
            var failfulMessage = new HttpRequestMessage();
            var failfulResponseMessage = "Internal server error";
            var content = new StringContent("Content");
            var failfulResponse = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError, ReasonPhrase = failfulResponseMessage, Content = content };
            httpClient.SendAsync(Arg.Is(failfulMessage)).Returns(failfulResponse);

            await Assert.ThrowsAsync<ContentManagementException>(async () => { await _client.SendAsync(failfulMessage); });
            await httpClient.Received().SendAsync(failfulMessage);
        }

        [Fact]
        public async Task SendAsync_SendsMessageOnTooManyRequestsWaitsRetriesThenSuccess()
        {
            var retryDeltaMilliseconds = 1000;
            var retryDelta = System.TimeSpan.FromMilliseconds(retryDeltaMilliseconds);
            delay.DelayByTimeSpan(Arg.Any<System.TimeSpan>()).Returns(Task.CompletedTask);
            var successfulMessage = new HttpRequestMessage();
            var tooManyRequestsResponse = new HttpResponseMessage { StatusCode = (HttpStatusCode) 429 };
            tooManyRequestsResponse.Headers.RetryAfter = new RetryConditionHeaderValue(retryDelta);
            var successResponse = new HttpResponseMessage { StatusCode =  HttpStatusCode.OK };
            httpClient.SendAsync(Arg.Is(successfulMessage)).Returns(x => tooManyRequestsResponse, x => tooManyRequestsResponse, x => successResponse);

            var response = await _client.SendAsync(successfulMessage);

            await httpClient.Received(3).SendAsync(successfulMessage);
            await delay.Received(2).DelayByTimeSpan(retryDelta);
            Assert.Equal(successResponse, response);
        }
    }
}
