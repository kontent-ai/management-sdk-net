using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using AwesomeAssertions;
using Kontent.Ai.Management.Extensions;
using Polly;
using Polly.Retry;
using Xunit;

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
    public async Task GetRetryAfterDelay_Non429_ReturnsNull()
    {
        var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(7));

        var delay = await InvokeGetRetryAfterDelay(response);

        delay.Should().BeNull();
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
            return ValueTask.FromResult(WithRetryAfter(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable), TimeSpan.Zero));
        });

        // 1 initial + 3 retries = 4 total attempts, then surfaces the final failure.
        attempts.Should().Be(4);
        response.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
    }

    private static HttpResponseMessage WithRetryAfter(HttpResponseMessage response, TimeSpan delta)
    {
        response.Headers.RetryAfter = new RetryConditionHeaderValue(delta);
        return response;
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
