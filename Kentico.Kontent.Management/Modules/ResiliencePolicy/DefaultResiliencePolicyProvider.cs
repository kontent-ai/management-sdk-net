using System;
using System.Net.Http;

using Polly;

namespace Kentico.Kontent.Management.Modules.ResiliencePolicy;

/// <summary>
/// Provides a default (fallback) retry policy for HTTP requests
/// </summary>
public class DefaultResiliencePolicyProvider : IResiliencePolicyProvider
{
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
    public IAsyncPolicy<HttpResponseMessage> Policy =>
            // Only HTTP status codes are handled with retries, not exceptions.
            Polly.Policy
                .HandleResult<HttpResponseMessage>(result => Enum.IsDefined(typeof(RetryHttpCode), (RetryHttpCode)result.StatusCode))
                .WaitAndRetryAsync(
                    _maxRetryAttempts,
                    retryAttempt => TimeSpan.FromMilliseconds(Math.Pow(2, retryAttempt) * 100)
                );
}
