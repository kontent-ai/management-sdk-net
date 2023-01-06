using Polly;
using System.Net.Http;

namespace Kontent.Ai.Management.Modules.ResiliencePolicy;

/// <summary>
/// Provides a resilience policy.
/// </summary>
public interface IResiliencePolicyProvider
{
    /// <summary>
    /// Gets the resilience policy.
    /// </summary>
    IAsyncPolicy<HttpResponseMessage> Policy { get; }
}
