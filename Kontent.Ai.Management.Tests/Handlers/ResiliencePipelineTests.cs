using AwesomeAssertions;
using Kontent.Ai.Management.Extensions;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Polly.Retry;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;

namespace Kontent.Ai.Management.Tests.Handlers;

public class ResiliencePipelineTests
{
    public static TheoryData<HttpStatusCode, bool> RetryableStatusCodes => new()
    {
        { HttpStatusCode.RequestTimeout, true },
        { HttpStatusCode.TooManyRequests, true },
        { HttpStatusCode.InternalServerError, true },
        { HttpStatusCode.BadGateway, true },
        { HttpStatusCode.ServiceUnavailable, true },
        { HttpStatusCode.GatewayTimeout, true },
        { HttpStatusCode.OK, false },
        { HttpStatusCode.BadRequest, false },
        { HttpStatusCode.Unauthorized, false },
        { HttpStatusCode.Forbidden, false },
        { HttpStatusCode.NotFound, false },
        { HttpStatusCode.Conflict, false },
        { HttpStatusCode.UnprocessableEntity, false }
    };

    [Theory]
    [MemberData(nameof(RetryableStatusCodes))]
    public void IsRetryableStatusCode_MatchesExpected(HttpStatusCode code, bool expected)
    {
        ServiceCollectionExtensions.IsRetryableStatusCode(code).Should().Be(expected);
    }

    [Fact]
    public void IsRetryableStatusCode_Null_ReturnsFalse()
    {
        ServiceCollectionExtensions.IsRetryableStatusCode(null).Should().BeFalse();
    }

    [Fact]
    public void IsTransientException_Null_ReturnsFalse()
    {
        ServiceCollectionExtensions.IsTransientException(null, CancellationToken.None).Should().BeFalse();
    }

    [Fact]
    public void IsTransientException_HttpRequestException_ReturnsTrue()
    {
        ServiceCollectionExtensions.IsTransientException(new HttpRequestException(), CancellationToken.None).Should().BeTrue();
    }

    [Fact]
    public void IsTransientException_SocketException_WrappedInHttpRequestException_ReturnsTrue()
    {
        var inner = new SocketException((int)SocketError.ConnectionRefused);
        var ex = new HttpRequestException("transient", inner);

        ServiceCollectionExtensions.IsTransientException(ex, CancellationToken.None).Should().BeTrue();
    }

    [Fact]
    public void IsTransientException_TimeoutException_ReturnsTrue()
    {
        ServiceCollectionExtensions.IsTransientException(new TimeoutException(), CancellationToken.None).Should().BeTrue();
    }

    [Fact]
    public void IsTransientException_TaskCanceledException_WithoutUserCancellation_ReturnsTrue()
    {
        ServiceCollectionExtensions.IsTransientException(new TaskCanceledException(), CancellationToken.None).Should().BeTrue();
    }

    [Fact]
    public void IsTransientException_TaskCanceledException_WithTimeoutInner_ReturnsTrue()
    {
        var ex = new TaskCanceledException("http timeout", new TimeoutException());

        ServiceCollectionExtensions.IsTransientException(ex, CancellationToken.None).Should().BeTrue();
    }

    [Fact]
    public void IsTransientException_OperationCanceled_UserCancelled_ReturnsFalse()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var ex = new OperationCanceledException(cts.Token);

        ServiceCollectionExtensions.IsTransientException(ex, cts.Token).Should().BeFalse();
    }

    [Fact]
    public void IsTransientException_InvalidOperationException_ReturnsFalse()
    {
        ServiceCollectionExtensions.IsTransientException(new InvalidOperationException(), CancellationToken.None).Should().BeFalse();
    }

    [Fact]
    public async Task GetRetryAfterDelay_Returns429RetryAfterDelta()
    {
        var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests);
        response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(7));

        var delay = await InvokeGetRetryAfterDelay(response);

        delay.Should().Be(TimeSpan.FromSeconds(7));
    }

    [Fact]
    public async Task GetRetryAfterDelay_429WithoutHeader_ReturnsNull()
    {
        var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests);

        var delay = await InvokeGetRetryAfterDelay(response);

        delay.Should().BeNull();
    }

    [Fact]
    public async Task GetRetryAfterDelay_503WithHeader_ReturnsDelta()
    {
        var response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
        response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(7));

        var delay = await InvokeGetRetryAfterDelay(response);

        delay.Should().Be(TimeSpan.FromSeconds(7));
    }

    [Theory]
    [InlineData("GET", true)]
    [InlineData("HEAD", true)]
    [InlineData("OPTIONS", true)]
    [InlineData("PUT", true)]
    [InlineData("DELETE", true)]
    [InlineData("POST", false)]
    [InlineData("PATCH", false)]
    public void IsIdempotent_MatchesExpected(string method, bool expected)
    {
        ServiceCollectionExtensions.IsIdempotent(HttpMethod.Parse(method)).Should().Be(expected);
    }

    [Fact]
    public void IsIdempotent_UnknownMethod_IsNotIdempotent()
    {
        ServiceCollectionExtensions.IsIdempotent(null).Should().BeFalse();
    }

    // ----- ShouldRetry: idempotency-aware retry decisions --------------------------------------------------------

    [Theory]
    [InlineData("GET", HttpStatusCode.InternalServerError, true)]
    [InlineData("PUT", HttpStatusCode.ServiceUnavailable, true)]
    [InlineData("POST", HttpStatusCode.InternalServerError, false)]
    [InlineData("PATCH", HttpStatusCode.ServiceUnavailable, false)]
    [InlineData("POST", HttpStatusCode.TooManyRequests, true)]  // 429 was rejected, not processed — safe for any method
    [InlineData("PATCH", HttpStatusCode.TooManyRequests, true)]
    [InlineData("GET", HttpStatusCode.BadRequest, false)]
    public void ShouldRetry_Response_HonorsMethodIdempotency(string method, HttpStatusCode statusCode, bool expected)
    {
        var response = new HttpResponseMessage(statusCode)
        {
            RequestMessage = new HttpRequestMessage(HttpMethod.Parse(method), "https://example.test/"),
        };

        InvokeShouldRetry(Outcome.FromResult(response), request: null).Should().Be(expected);
    }

    [Fact]
    public void ShouldRetry_TransientException_IdempotentMethod_Retries()
    {
        var request = new HttpRequestMessage(HttpMethod.Put, "https://example.test/");

        InvokeShouldRetry(Outcome.FromException<HttpResponseMessage>(new HttpRequestException()), request)
            .Should().BeTrue();
    }

    [Fact]
    public void ShouldRetry_TransientException_PostMethod_DoesNotRetry()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://example.test/");

        InvokeShouldRetry(Outcome.FromException<HttpResponseMessage>(new HttpRequestException()), request)
            .Should().BeFalse();
    }

    [Fact]
    public void ShouldRetry_TransientException_NoRequestMessage_DoesNotRetry()
    {
        InvokeShouldRetry(Outcome.FromException<HttpResponseMessage>(new HttpRequestException()), request: null)
            .Should().BeFalse();
    }

    [Fact]
    public async Task ConfigureDefaultResilience_RetriesOnTransientStatusCode()
    {
        var builder = new ResiliencePipelineBuilder<HttpResponseMessage>();
        ServiceCollectionExtensions.ConfigureDefaultResilience(builder);
        var pipeline = builder.Build();

        var attempts = 0;
        var response = await pipeline.ExecuteAsync(_ =>
        {
            attempts++;
            return attempts < 2
                ? ValueTask.FromResult(WithRetryAfter(new HttpResponseMessage(HttpStatusCode.TooManyRequests), TimeSpan.Zero))
                : ValueTask.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        attempts.Should().Be(2);
    }

    [Fact]
    public async Task ConfigureDefaultResilience_DoesNotRetryOnNonRetryableStatusCode()
    {
        var builder = new ResiliencePipelineBuilder<HttpResponseMessage>();
        ServiceCollectionExtensions.ConfigureDefaultResilience(builder);
        var pipeline = builder.Build();

        var attempts = 0;
        var response = await pipeline.ExecuteAsync(_ =>
        {
            attempts++;
            return ValueTask.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized));
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        attempts.Should().Be(1);
    }

    [Fact]
    public async Task ConfigureDefaultResilience_GivesUpAfterMaxAttempts()
    {
        var builder = new ResiliencePipelineBuilder<HttpResponseMessage>();
        ServiceCollectionExtensions.ConfigureDefaultResilience(builder);
        var pipeline = builder.Build();

        var attempts = 0;
        var response = await pipeline.ExecuteAsync(_ =>
        {
            attempts++;
            return ValueTask.FromResult(WithRetryAfter(new HttpResponseMessage(HttpStatusCode.TooManyRequests), TimeSpan.Zero));
        });

        // 1 initial + 3 retries = 4 total attempts, then surfaces the final failure.
        attempts.Should().Be(4);
        response.StatusCode.Should().Be(HttpStatusCode.TooManyRequests);
    }

    // ----- Through ResilienceHandler: the production handler shape (request message flows via the context) --------

    [Fact]
    public async Task DefaultResilience_ThroughHandler_Get503_Retries()
    {
        var (invoker, stub) = CreateDefaultResilienceInvoker(attempt => attempt < 2
            ? WithRetryAfter(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable), TimeSpan.Zero)
            : new HttpResponseMessage(HttpStatusCode.OK));

        var response = await invoker.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://example.test/"), CancellationToken.None);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stub.Attempts.Should().Be(2);
    }

    [Fact]
    public async Task DefaultResilience_ThroughHandler_Post503_DoesNotRetry()
    {
        var (invoker, stub) = CreateDefaultResilienceInvoker(_ =>
            WithRetryAfter(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable), TimeSpan.Zero));

        var response = await invoker.SendAsync(new HttpRequestMessage(HttpMethod.Post, "https://example.test/"), CancellationToken.None);

        response.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
        stub.Attempts.Should().Be(1);
    }

    [Fact]
    public async Task DefaultResilience_ThroughHandler_Post429_Retries()
    {
        var (invoker, stub) = CreateDefaultResilienceInvoker(attempt => attempt < 2
            ? WithRetryAfter(new HttpResponseMessage(HttpStatusCode.TooManyRequests), TimeSpan.Zero)
            : new HttpResponseMessage(HttpStatusCode.OK));

        var response = await invoker.SendAsync(new HttpRequestMessage(HttpMethod.Post, "https://example.test/"), CancellationToken.None);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stub.Attempts.Should().Be(2);
    }

    private static (HttpMessageInvoker Invoker, StubHandler Stub) CreateDefaultResilienceInvoker(Func<int, HttpResponseMessage> responder)
    {
        var builder = new ResiliencePipelineBuilder<HttpResponseMessage>();
        ServiceCollectionExtensions.ConfigureDefaultResilience(builder);
        var stub = new StubHandler(responder);
        return (new HttpMessageInvoker(new ResilienceHandler(builder.Build()) { InnerHandler = stub }), stub);
    }

    private sealed class StubHandler(Func<int, HttpResponseMessage> responder) : HttpMessageHandler
    {
        public int Attempts { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Attempts++;
            var response = responder(Attempts);
            response.RequestMessage ??= request;
            return Task.FromResult(response);
        }
    }

    private static HttpResponseMessage WithRetryAfter(HttpResponseMessage response, TimeSpan delta)
    {
        response.Headers.RetryAfter = new RetryConditionHeaderValue(delta);
        return response;
    }

    private static bool InvokeShouldRetry(Outcome<HttpResponseMessage> outcome, HttpRequestMessage? request)
    {
        var context = ResilienceContextPool.Shared.Get();
        try
        {
            if (request is not null)
            {
                context.SetRequestMessage(request);
            }
            return ServiceCollectionExtensions.ShouldRetry(outcome, context);
        }
        finally
        {
            ResilienceContextPool.Shared.Return(context);
        }
    }

    private static async Task<TimeSpan?> InvokeGetRetryAfterDelay(HttpResponseMessage response)
    {
        var context = ResilienceContextPool.Shared.Get();
        try
        {
            var args = new RetryDelayGeneratorArguments<HttpResponseMessage>(
                context,
                Outcome.FromResult(response),
                attemptNumber: 0);

            return await ServiceCollectionExtensions.GetRetryAfterDelay(args);
        }
        finally
        {
            ResilienceContextPool.Shared.Return(context);
        }
    }
}
