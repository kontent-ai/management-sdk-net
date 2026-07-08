using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Handlers;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace Kontent.Ai.Management.Api;

/// <summary>
/// Builds the configured <see cref="HttpClient"/> and Refit clients for the Management API. The handler chain and
/// base-address scoping live in <see cref="CreateHttpClient"/> so the standalone <see cref="ManagementClient"/> and
/// this test-facing factory share one definition. The <see cref="Create"/> / <see cref="CreateSubscription"/> entry
/// points omit resilience and let the test infrastructure inject a primary handler to short-circuit the network.
/// </summary>
internal static class ManagementApiFactory
{
    public static IManagementApi Create(ManagementOptions options, HttpMessageHandler? innerHandler = null)
    {
        ArgumentNullException.ThrowIfNull(options);
        var http = CreateHttpClient(options, $"projects/{options.EnvironmentId}", new SnapshotManagementOptionsAccessor(options), primaryHandler: innerHandler);
        return RestService.For<IManagementApi>(http, RefitSettingsProvider.CreateDefaultSettings());
    }

    public static ISubscriptionApi CreateSubscription(ManagementOptions options, HttpMessageHandler? innerHandler = null)
    {
        ArgumentNullException.ThrowIfNull(options);
        var http = CreateHttpClient(options, options.SubscriptionScopePath(), new SnapshotManagementOptionsAccessor(options), primaryHandler: innerHandler);
        return RestService.For<ISubscriptionApi>(http, RefitSettingsProvider.CreateDefaultSettings());
    }

    /// <summary>
    /// Builds an <see cref="HttpClient"/> scoped to <paramref name="scopePath"/> with the handler chain, outermost to
    /// innermost, <c>[resilience] → tracking → auth → primary</c> — matching the DI path's ordering so each retry
    /// re-runs tracking and auth fresh. Resilience is included only when <paramref name="resiliencePipeline"/> is
    /// supplied; <paramref name="primaryHandler"/> overrides the default <see cref="HttpClientHandler"/> (the test
    /// infrastructure injects a mock here).
    /// </summary>
    public static HttpClient CreateHttpClient(
        ManagementOptions options,
        string scopePath,
        IManagementOptionsAccessor optionsAccessor,
        ResiliencePipeline<HttpResponseMessage>? resiliencePipeline = null,
        HttpMessageHandler? primaryHandler = null)
    {
        HttpMessageHandler primary = primaryHandler ?? new HttpClientHandler();
        var auth = new ManagementAuthenticationHandler(optionsAccessor) { InnerHandler = primary };
        var tracking = new TrackingHandler { InnerHandler = auth };
        HttpMessageHandler outermost = resiliencePipeline is null
            ? tracking
            : new ResilienceHandler(resiliencePipeline) { InnerHandler = tracking };

        return new HttpClient(outermost)
        {
            BaseAddress = options.ScopedEndpoint(scopePath),
        };
    }
}
