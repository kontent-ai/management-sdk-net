using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Handlers;

namespace Kontent.Ai.Management.Api;

/// <summary>
/// Builds Refit clients for the Management API without the DI / resilience pipeline. Used by the test infrastructure,
/// which injects an <see cref="HttpMessageHandler"/> directly to short-circuit the network. Production callers go
/// through <c>AddManagementClient</c> or <see cref="ManagementClientBuilder"/> instead, which add resilience.
/// </summary>
internal static class ManagementApiFactory
{
    public static IManagementApi Create(ManagementOptions options, HttpMessageHandler? innerHandler = null)
    {
        ArgumentNullException.ThrowIfNull(options);
        return CreateClient<IManagementApi>(options, innerHandler, $"projects/{options.EnvironmentId}");
    }

    public static ISubscriptionApi CreateSubscription(ManagementOptions options, HttpMessageHandler? innerHandler = null)
    {
        ArgumentNullException.ThrowIfNull(options);
        return CreateClient<ISubscriptionApi>(options, innerHandler, $"subscriptions/{options.SubscriptionId}");
    }

    private static T CreateClient<T>(ManagementOptions options, HttpMessageHandler? innerHandler, string scopePath)
    {
        // Pipeline order matches the DI registration: tracking → auth → resilience (absent here) → inner.
        var optionsAccessor = new SnapshotManagementOptionsAccessor(options);
        var auth = new ManagementAuthenticationHandler(optionsAccessor)
        {
            InnerHandler = innerHandler ?? new HttpClientHandler(),
        };
        var tracking = new TrackingHandler
        {
            InnerHandler = auth,
        };

        var httpClient = new HttpClient(tracking)
        {
            BaseAddress = new Uri(string.Format(options.EndpointV2, scopePath), UriKind.Absolute),
        };

        return RestService.For<T>(httpClient, new RefitSettings
        {
            ContentSerializer = new ManagementApiContentSerializer(),
        });
    }
}
