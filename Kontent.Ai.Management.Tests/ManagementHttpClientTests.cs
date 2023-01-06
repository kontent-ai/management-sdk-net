using Kontent.Ai.Management.Exceptions;
using Kontent.Ai.Management.Modules.ActionInvoker;
using Kontent.Ai.Management.Modules.HttpClient;
using Kontent.Ai.Management.Modules.ResiliencePolicy;
using NSubstitute;
using Polly;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Kontent.Ai.Management.Tests;

public class ManagementHttpClientTests
{
    private readonly IMessageCreator messageCreator = new MessageCreator(string.Empty);
    private readonly string endpointUrl = string.Empty;
    private readonly HttpMethod method = HttpMethod.Get;

    private readonly ManagementHttpClient _defaultClient;
    private readonly IHttpClient httpClient = Substitute.For<IHttpClient>();

    public ManagementHttpClientTests()
    {
        _defaultClient = new ManagementHttpClient(
            httpClient,
            new DefaultResiliencePolicyProvider(Constants.DEFAULT_MAX_RETRIES),
            Constants.ENABLE_RESILIENCE_POLICY);
    }


    [Fact]
    public void CreateMessage_AddCorrectSDKTreackingHeader()
    {
        var assembly = typeof(ManagementHttpClient).Assembly;
        var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        var sdkVersion = fileVersionInfo.ProductVersion;
        var sdkPackageId = assembly.GetName().Name;

        var msg = messageCreator.CreateMessage(HttpMethod.Post, string.Empty);

        msg.Headers.TryGetValues("X-KC-SDKID", out var headerContent);
        Assert.True(msg.Headers.Contains("X-KC-SDKID"));
        Assert.Contains($"nuget.org;{sdkPackageId};{sdkVersion}", headerContent);
    }

    [Fact]
    public async Task SendAsync_SendsMessageOnSuccessReturnsResponse()
    {
        var successfulMessage = new HttpRequestMessage();
        var successfulResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
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

        await Assert.ThrowsAsync<ManagementException>(async () => { await _defaultClient.SendAsync(messageCreator, endpointUrl, method); });
        await httpClient.ReceivedWithAnyArgs().SendAsync(failfulMessage);
    }

    [Fact]
    public async void Retries_WithDefaultSettings_FailsAfterAllRetries()
    {
        var expectedAttempts = Constants.DEFAULT_MAX_RETRIES + 1;
        var failfulMessage = new HttpRequestMessage();
        httpClient
            .SendAsync(failfulMessage)
            .ReturnsForAnyArgs((request) => new HttpResponseMessage(HttpStatusCode.RequestTimeout));

        httpClient.ClearReceivedCalls();
        await Assert.ThrowsAsync<ManagementException>(async () => { await _defaultClient.SendAsync(messageCreator, endpointUrl, method); });
        await httpClient.ReceivedWithAnyArgs(expectedAttempts).SendAsync(failfulMessage);
    }

    [Fact]
    public async void Retries_WithDefaultSettings_NoRetryAfterValue_Succeeds()
    {
        var expectedAttempts = 2;
        var failfulMessage = new HttpRequestMessage();
        httpClient
            .SendAsync(failfulMessage)
            .ReturnsForAnyArgs(
            x =>
            {
                return new HttpResponseMessage(HttpStatusCode.TooManyRequests);
            },
            x =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            });

        await _defaultClient.SendAsync(messageCreator, endpointUrl, method);
        await httpClient.ReceivedWithAnyArgs(expectedAttempts).SendAsync(failfulMessage);
    }

    [Fact]
    public async void Retries_WithDefaultSettings_WithRetryAfterHeader_Succeeds()
    {
        var expectedAttempts = 2;
        var failfulMessage = new HttpRequestMessage();
        httpClient
            .SendAsync(failfulMessage)
            .ReturnsForAnyArgs(
            x =>
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.TooManyRequests,
                    Headers = { { "Retry-After", "2" } }
                };
            },
            x =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            });

        await _defaultClient.SendAsync(messageCreator, endpointUrl, method);
        await httpClient.ReceivedWithAnyArgs(expectedAttempts).SendAsync(failfulMessage);
    }

    [Fact]
    public async void Retries_WithDefaultSettings_WithRetryAfterHeader_RetryAfterPocilyIsApplied()
    {
        var expectedAttempts = 2 * (Constants.DEFAULT_MAX_RETRIES + 1);
        var failfulMessage = new HttpRequestMessage();
        httpClient
            .SendAsync(failfulMessage)
            .ReturnsForAnyArgs(
            x =>
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.TooManyRequests,
                    Headers = { { "Retry-After", "2" } }
                };
            });

        httpClient.ClearReceivedCalls();
        await Assert.ThrowsAsync<ManagementException>(async () => { await _defaultClient.SendAsync(messageCreator, endpointUrl, method); });
        await httpClient.ReceivedWithAnyArgs(expectedAttempts).SendAsync(failfulMessage);
    }

    [Fact]
    public async void Retries_WithDefaultSettings_WithoutRetryAfterHeader_RetryAfterPocilyIsSkipped()
    {
        //+1 for regular attempt
        var expectedAttempts = Constants.DEFAULT_MAX_RETRIES + 1;
        var failfulMessage = new HttpRequestMessage();
        httpClient
            .SendAsync(failfulMessage)
            .ReturnsForAnyArgs((request) => new HttpResponseMessage(HttpStatusCode.TooManyRequests));

        httpClient.ClearReceivedCalls();
        await Assert.ThrowsAsync<ManagementException>(async () => { await _defaultClient.SendAsync(messageCreator, endpointUrl, method); });
        await httpClient.ReceivedWithAnyArgs(expectedAttempts).SendAsync(failfulMessage);
    }

    [Fact]
    public async void Retries_EnableResilienceLogicDisabled_DoesNotRetry()
    {
        var failfulMessage = new HttpRequestMessage();
        httpClient
            .SendAsync(failfulMessage)
            .ReturnsForAnyArgs((request) => new HttpResponseMessage(HttpStatusCode.RequestTimeout));

        var customClient = new ManagementHttpClient(
            httpClient,
            new DefaultResiliencePolicyProvider(Constants.DEFAULT_MAX_RETRIES),
            Constants.DISABLE_RESILIENCE_POLICY);

        httpClient.ClearReceivedCalls();
        await Assert.ThrowsAsync<ManagementException>(async () => { await customClient.SendAsync(messageCreator, endpointUrl, method); });
        await httpClient.ReceivedWithAnyArgs(1).SendAsync(failfulMessage);
    }

    [Fact]
    public async void Retries_WithMaxRetrySet_SettingReflected()
    {
        var retryAttempts = 3;
        var expectedAttempts = retryAttempts + 1;

        var failfulMessage = new HttpRequestMessage();
        httpClient
            .SendAsync(failfulMessage)
            .ReturnsForAnyArgs((request) => new HttpResponseMessage(HttpStatusCode.RequestTimeout));

        var customClient = new ManagementHttpClient(
            httpClient,
            new DefaultResiliencePolicyProvider(retryAttempts),
            Constants.ENABLE_RESILIENCE_POLICY);

        httpClient.ClearReceivedCalls();
        await Assert.ThrowsAsync<ManagementException>(async () => { await customClient.SendAsync(messageCreator, endpointUrl, method); });
        await httpClient.ReceivedWithAnyArgs(expectedAttempts).SendAsync(failfulMessage);
    }

    [Fact]
    public async void Retries_WithCustomResilencePolicy_PolicyUsed()
    {
        var retryAttempts = 1;
        var expectedAttempts = retryAttempts + 1;

        var failfulMessage = new HttpRequestMessage();
        httpClient
            .SendAsync(failfulMessage)
            .ReturnsForAnyArgs((request) => new HttpResponseMessage(HttpStatusCode.RequestTimeout));

        var mockResilencePolicyProvider = Substitute.For<IResiliencePolicyProvider>();
        mockResilencePolicyProvider.Policy
           .Returns(Policy.HandleResult<HttpResponseMessage>(result => true).RetryAsync(retryAttempts));

        var customClient = new ManagementHttpClient(
            httpClient,
            mockResilencePolicyProvider,
            Constants.ENABLE_RESILIENCE_POLICY);

        httpClient.ClearReceivedCalls();
        await Assert.ThrowsAsync<ManagementException>(async () => { await customClient.SendAsync(messageCreator, endpointUrl, method); });
        var policy = mockResilencePolicyProvider.Received(expectedAttempts).Policy;
    }

    [Fact]
    public async void Retries_WithCustomResilencePolicyAndPolicyDisabled_PolicyIgnored()
    {
        var retryAttempts = 2;

        var failfulMessage = new HttpRequestMessage();
        httpClient
            .SendAsync(failfulMessage)
            .ReturnsForAnyArgs((request) => new HttpResponseMessage(HttpStatusCode.RequestTimeout));

        var mockResilencePolicyProvider = Substitute.For<IResiliencePolicyProvider>();
        mockResilencePolicyProvider.Policy
           .Returns(Policy.HandleResult<HttpResponseMessage>(result => true).RetryAsync(retryAttempts));

        var customClient = new ManagementHttpClient(
            httpClient,
            mockResilencePolicyProvider,
            Constants.DISABLE_RESILIENCE_POLICY);

        httpClient.ClearReceivedCalls();
        await Assert.ThrowsAsync<ManagementException>(async () => { await customClient.SendAsync(messageCreator, endpointUrl, method); });
        var policy = mockResilencePolicyProvider.DidNotReceive().Policy;
    }
}
