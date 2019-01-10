using KenticoCloud.ContentManagement.Exceptions;
using KenticoCloud.ContentManagement.Modules.ActionInvoker;
using KenticoCloud.ContentManagement.Modules.HttpClient;
using KenticoCloud.ContentManagement.Modules.ResiliencePolicy;
using NSubstitute;
using Polly;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace KenticoCloud.ContentManagement.Tests
{
    public class ContentManagementHttpClientTests
    {
        private readonly IMessageCreator messageCreator = new MessageCreator(string.Empty);
        private readonly string endpointUrl = string.Empty;
        private readonly HttpMethod method = HttpMethod.Get;

        private ContentManagementHttpClient _defaultClient;
        private IHttpClient httpClient = Substitute.For<IHttpClient>();

        public ContentManagementHttpClientTests()
        {
            _defaultClient = new ContentManagementHttpClient(
                httpClient,
                new DefaultResiliencePolicyProvider(Constants.DEFAULT_MAX_RETRIES),
                Constants.ENABLE_RESILIENCE_POLICY);
        }


        [Fact]
        public void CreateMessage_AddCorrectSDKTreackingHeader()
        {
            var assembly = typeof(ContentManagementHttpClient).Assembly;
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            var sdkVersion = fileVersionInfo.ProductVersion;
            var sdkPackageId = assembly.GetName().Name;

            var msg = messageCreator.CreateMessage(HttpMethod.Post, string.Empty);

            IEnumerable<string> headerContent = new List<string>();
            msg.Headers.TryGetValues("X-KC-SDKID", out headerContent);
            Assert.True(msg.Headers.Contains("X-KC-SDKID"));
            Assert.Contains($"nuget.org;{sdkPackageId};{sdkVersion}", headerContent);
        }

        [Fact]
        public async Task SendAsync_SendsMessageOnSuccessReturnsResponse()
        {
            var successfulMessage = new HttpRequestMessage();
            var successfulResponse = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
            httpClient.SendAsync(successfulMessage).ReturnsForAnyArgs(successfulResponse);

            var response = await _defaultClient.SendAsync(messageCreator, endpointUrl, method);

            await httpClient.ReceivedWithAnyArgs().SendAsync(successfulMessage);
            Assert.Equal(successfulResponse, response);
        }

        [Fact]
        public async Task SendAsync_SendsMessageOnFailureThrows()
        {
            var failfulMessage = new HttpRequestMessage();
            var failfulResponseMessage = "Internal server error";
            var content = new StringContent("Content");
            var failfulResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError, ReasonPhrase = failfulResponseMessage, Content = content };
            httpClient.SendAsync(failfulMessage).ReturnsForAnyArgs(failfulResponse);

            await Assert.ThrowsAsync<ContentManagementException>(async () => { await _defaultClient.SendAsync(messageCreator, endpointUrl, method); });
            await httpClient.ReceivedWithAnyArgs().SendAsync(failfulMessage);
        }

        [Fact]
        public async void Retries_WithDefaultSettings_Retries()
        {
            int expectedAttempts = Constants.DEFAULT_MAX_RETRIES + 1;
            var failfulMessage = new HttpRequestMessage();
            httpClient
                .SendAsync(failfulMessage)
                .ReturnsForAnyArgs((request) => new HttpResponseMessage(HttpStatusCode.RequestTimeout));

            httpClient.ClearReceivedCalls();
            await Assert.ThrowsAsync<ContentManagementException>(async () => { await _defaultClient.SendAsync(messageCreator, endpointUrl, method); });
            await httpClient.ReceivedWithAnyArgs(expectedAttempts).SendAsync(failfulMessage);
        }

        [Fact]
        public async void Retries_EnableResilienceLogicDisabled_DoesNotRetry()
        {
            var failfulMessage = new HttpRequestMessage();
            httpClient
                .SendAsync(failfulMessage)
                .ReturnsForAnyArgs((request) => new HttpResponseMessage(HttpStatusCode.RequestTimeout));

            var customClient = new ContentManagementHttpClient(
                httpClient,
                new DefaultResiliencePolicyProvider(Constants.DEFAULT_MAX_RETRIES),
                Constants.DISABLE_RESILIENCE_POLICY);

            httpClient.ClearReceivedCalls();
            await Assert.ThrowsAsync<ContentManagementException>(async () => { await customClient.SendAsync(messageCreator, endpointUrl, method); });
            await httpClient.ReceivedWithAnyArgs(1).SendAsync(failfulMessage);
        }

        [Fact]
        public async void Retries_WithMaxRetrySet_SettingReflected()
        {
            int retryAttempts = 3;
            int expectedAttempts = retryAttempts + 1;

            var failfulMessage = new HttpRequestMessage();
            httpClient
                .SendAsync(failfulMessage)
                .ReturnsForAnyArgs((request) => new HttpResponseMessage(HttpStatusCode.RequestTimeout));

            var customClient = new ContentManagementHttpClient(
                httpClient,
                new DefaultResiliencePolicyProvider(retryAttempts),
                Constants.ENABLE_RESILIENCE_POLICY);

            httpClient.ClearReceivedCalls();
            await Assert.ThrowsAsync<ContentManagementException>(async () => { await customClient.SendAsync(messageCreator, endpointUrl, method); });
            await httpClient.ReceivedWithAnyArgs(expectedAttempts).SendAsync(failfulMessage);
        }

        [Fact]
        public async void Retries_WithCustomResilencePolicy_PolicyUsed()
        {
            int retryAttempts = 1;
            int expectedAttempts = retryAttempts + 1;

            var failfulMessage = new HttpRequestMessage();
            httpClient
                .SendAsync(failfulMessage)
                .ReturnsForAnyArgs((request) => new HttpResponseMessage(HttpStatusCode.RequestTimeout));

            var mockResilencePolicyProvider = Substitute.For<IResiliencePolicyProvider>();
            mockResilencePolicyProvider.Policy
               .Returns(Policy.HandleResult<HttpResponseMessage>(result => true).RetryAsync(retryAttempts));

            var customClient = new ContentManagementHttpClient(
                httpClient,
                mockResilencePolicyProvider,
                Constants.ENABLE_RESILIENCE_POLICY);

            httpClient.ClearReceivedCalls();
            await Assert.ThrowsAsync<ContentManagementException>(async () => { await customClient.SendAsync(messageCreator, endpointUrl, method); });
            var policy = mockResilencePolicyProvider.Received(expectedAttempts).Policy;
        }

        [Fact]
        public async void Retries_WithCustomResilencePolicyAndPolicyDisabled_PolicyIgnored()
        {
            int retryAttempts = 2;

            var failfulMessage = new HttpRequestMessage();
            httpClient
                .SendAsync(failfulMessage)
                .ReturnsForAnyArgs((request) => new HttpResponseMessage(HttpStatusCode.RequestTimeout));

            var mockResilencePolicyProvider = Substitute.For<IResiliencePolicyProvider>();
            mockResilencePolicyProvider.Policy
               .Returns(Policy.HandleResult<HttpResponseMessage>(result => true).RetryAsync(retryAttempts));

            var customClient = new ContentManagementHttpClient(
                httpClient,
                mockResilencePolicyProvider,
                Constants.DISABLE_RESILIENCE_POLICY);

            httpClient.ClearReceivedCalls();
            await Assert.ThrowsAsync<ContentManagementException>(async () => { await customClient.SendAsync(messageCreator, endpointUrl, method); });
            var policy = mockResilencePolicyProvider.DidNotReceive().Policy;
        }
    }
}
