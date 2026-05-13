using Kontent.Ai.Management.Configuration;

namespace Kontent.Ai.Management.Api;

/// <summary>
/// Builds the Refit clients for the Kontent.ai Management API v2 — <see cref="IManagementApi"/> for the environment-scoped
/// endpoints (<c>{endpoint}/projects/{environmentId}</c>) and <see cref="ISubscriptionApi"/> for the subscription-scoped
/// ones (<c>{endpoint}/subscriptions/{subscriptionId}</c>). Each gets bearer auth + SDK tracking headers and the
/// transitional Newtonsoft content serializer. Resilience is not wired here yet — it stays on the legacy
/// <c>ManagementHttpClient</c> path until the dedicated resilience step.
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
        var pipeline = new ManagementApiAuthorizationHandler(options.ApiKey)
        {
            InnerHandler = innerHandler ?? new HttpClientHandler(),
        };

        var httpClient = new HttpClient(pipeline)
        {
            BaseAddress = new Uri(string.Format(options.EndpointV2, scopePath), UriKind.Absolute),
        };

        return RestService.For<T>(httpClient, new RefitSettings
        {
            ContentSerializer = new ManagementApiContentSerializer(),
        });
    }
}
