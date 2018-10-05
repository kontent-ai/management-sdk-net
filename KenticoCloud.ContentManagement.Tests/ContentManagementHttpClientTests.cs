using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using KenticoCloud.ContentManagement.Exceptions;
using KenticoCloud.ContentManagement.Modules.HttpClient;
using KenticoCloud.ContentManagement.Modules.Extensions;

using NSubstitute;
using Xunit;
using System.Collections.Generic;

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
        public void CorrectSdkVersionHeaderAdded()
        {
            var assembly = typeof(ContentManagementHttpClient).Assembly;
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            var sdkVersion = fileVersionInfo.ProductVersion;
            var sdkPackageId = assembly.GetName().Name;
            var httpRequestMessage = new HttpRequestMessage();

            httpRequestMessage.Headers.AddSdkTrackingHeader();

            IEnumerable<string> headerContent = new List<string>();
            httpRequestMessage.Headers.TryGetValues("X-KC-SDKID", out headerContent);

            Assert.True(httpRequestMessage.Headers.Contains("X-KC-SDKID"));
            Assert.Contains($"nuget.org;{sdkPackageId};{sdkVersion}", headerContent);
        }


        [Fact]
        public async Task SendAsync_AddCorrectSDKTreackingHeader()
        {
            var assembly = typeof(ContentManagementHttpClient).Assembly;
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            var sdkVersion = fileVersionInfo.ProductVersion;
            var sdkPackageId = assembly.GetName().Name;

            IHttpClient httpClient = Substitute.For<IHttpClient>();
            IDelay delay = Substitute.For<IDelay>();
            HttpRequestMessage mockRequestMessage = Substitute.For<HttpRequestMessage>();
            var successfulResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
            httpClient.SendAsync(Arg.Is(mockRequestMessage)).Returns(successfulResponse);

            ContentManagementHttpClient client = new ContentManagementHttpClient(delay, httpClient);
            var response = await client.SendAsync(mockRequestMessage);

            IEnumerable<string> headerContent = new List<string>();
            mockRequestMessage.Headers.TryGetValues("X-KC-SDKID", out headerContent);
            Assert.True(mockRequestMessage.Headers.Contains("X-KC-SDKID"));
            Assert.Contains($"nuget.org;{sdkPackageId};{sdkVersion}", headerContent);
        }


        [Fact]
        public async Task SendAsync_SendsMessageOnSuccessReturnsResponse()
        {
            var successfulMessage = new HttpRequestMessage();
            var successfulResponse = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
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
            var tooManyRequestsResponse = new HttpResponseMessage { StatusCode = (HttpStatusCode)429 };
            tooManyRequestsResponse.Headers.RetryAfter = new RetryConditionHeaderValue(retryDelta);
            var successResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
            httpClient.SendAsync(Arg.Is(successfulMessage)).Returns(x => tooManyRequestsResponse, x => tooManyRequestsResponse, x => successResponse);

            var response = await _client.SendAsync(successfulMessage);

            await httpClient.Received(3).SendAsync(successfulMessage);
            await delay.Received(2).DelayByTimeSpan(retryDelta);
            Assert.Equal(successResponse, response);
        }
    }
}
