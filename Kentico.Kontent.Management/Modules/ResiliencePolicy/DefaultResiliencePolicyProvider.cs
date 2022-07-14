using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Kentico.Kontent.Management.Modules.Extensions;
using Polly;

namespace Kentico.Kontent.Management.Modules.ResiliencePolicy;

/// <summary>
/// Provides a default (fallback) retry policy for HTTP requests
/// </summary>
public class DefaultResiliencePolicyProvider : IResiliencePolicyProvider
{
    private static readonly HttpStatusCode[] StatusCodesToRetry =
    {
            HttpStatusCode.RequestTimeout,
            (HttpStatusCode)429, // Too Many Requests
            HttpStatusCode.InternalServerError,
            HttpStatusCode.BadGateway,
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.GatewayTimeout,
    };
    
    private static readonly HttpStatusCode[] StatusCodesWithPossibleRetryHeader =
    {
            (HttpStatusCode)429, // Too Many Requests
            HttpStatusCode.ServiceUnavailable,
    };

    private readonly int _maxRetryAttempts;

    /// <summary>
    /// Creates a default retry policy provider with a maximum number of retry attempts.
    /// </summary>
    /// <param name="maxRetryAttempts">Maximum retry attempts for a request.</param>
    public DefaultResiliencePolicyProvider(int maxRetryAttempts)
    {
        _maxRetryAttempts = maxRetryAttempts;
    }

    /// <summary>
    /// Gets the default (fallback) retry policy for HTTP requests.
    /// </summary>
    public IAsyncPolicy<HttpResponseMessage> Policy => WrappedPolicy();

    private IAsyncPolicy<HttpResponseMessage> WrappedPolicy()
    {
        var defaultPolicy = Polly.Policy
            .HandleResult<HttpResponseMessage>(result => StatusCodesToRetry.Contains(result.StatusCode))
            .WaitAndRetryAsync(
                _maxRetryAttempts,
                retryAttempt => TimeSpan.FromMilliseconds(Math.Pow(2, retryAttempt) * 100));

        var retryAfterPolicy = Polly.Policy
            .HandleResult<HttpResponseMessage>(result => StatusCodesWithPossibleRetryHeader.Contains(result.StatusCode) && result.Headers.RetryAfterExists())
            .WaitAndRetryAsync(
            retryCount: 1,
            sleepDurationProvider: (retryCount, response, context) => response.Result.Headers.GetRetryAfter(),
            onRetryAsync: (response, timespan, retryCount, context) => Task.CompletedTask);

        return Polly.Policy.WrapAsync(defaultPolicy, retryAfterPolicy);
    }
}

