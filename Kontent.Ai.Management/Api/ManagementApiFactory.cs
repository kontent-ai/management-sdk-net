using Kontent.Ai.Management.Configuration;

namespace Kontent.Ai.Management.Api;

/// <summary>
/// Builds a Refit <see cref="IManagementApi"/> wired to the Kontent.ai Management API v2: the environment-scoped base
/// address (<c>{endpoint}/projects/{environmentId}</c>), bearer auth + SDK tracking headers, and the transitional
/// Newtonsoft content serializer. Resilience is not wired here yet — it stays on the legacy <c>ManagementHttpClient</c>
/// path until the dedicated resilience step.
/// </summary>
internal static class ManagementApiFactory
{
    public static IManagementApi Create(ManagementOptions options, HttpMessageHandler? innerHandler = null)
    {
        ArgumentNullException.ThrowIfNull(options);

        var pipeline = new ManagementApiAuthorizationHandler(options.ApiKey)
        {
            InnerHandler = innerHandler ?? new HttpClientHandler(),
        };

        var httpClient = new HttpClient(pipeline)
        {
            BaseAddress = new Uri(string.Format(options.EndpointV2, $"projects/{options.EnvironmentId}"), UriKind.Absolute),
        };

        return RestService.For<IManagementApi>(httpClient, new RefitSettings
        {
            ContentSerializer = new ManagementApiContentSerializer(),
        });
    }
}
