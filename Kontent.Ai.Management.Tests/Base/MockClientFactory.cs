using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Conversion;
using RichardSzalay.MockHttp;

namespace Kontent.Ai.Management.Tests.Base;

/// <summary>
/// Builds an <see cref="IManagementClient"/> whose Refit transport is backed by a
/// <see cref="MockHttpMessageHandler"/>, wired in as the inner handler through <see cref="ManagementApiFactory"/>,
/// so tests arrange responses and assert outgoing requests via MockHttp's fluent API. Supply a pre-registered
/// <paramref name="converter"/> for strongly-typed variant paths: the client's own converter would otherwise
/// auto-scan the whole test assembly and trip the intentional codename collision among the test fixtures.
/// </summary>
internal static class MockClientFactory
{
    public const string EnvironmentId = "a9931a80-9af4-010b-0590-ecb1273cf1b8";
    public const string SubscriptionId = "9c7b9841-ea99-48a7-a46d-65b2549d6c0";

    // Refit composes "{BaseAddress}{relative path}", so the base must carry no trailing slash or every path
    // doubles the separator. EndpointV2 defaults to "https://manage.kontent.ai/v2/{0}", scoped to the project here.
    public static string BaseUrl => $"https://manage.kontent.ai/v2/projects/{EnvironmentId}";

    public static (IManagementClient Client, MockHttpMessageHandler Mock) Create(ContentItemEnvelopeConverter? converter = null)
    {
        var mock = new MockHttpMessageHandler();
        var options = new ManagementOptions
        {
            ApiKey = "Dummy_API_key",
            EnvironmentId = EnvironmentId,
            SubscriptionId = SubscriptionId,
        };

        var managementApi = ManagementApiFactory.Create(options, mock);
        var subscriptionApi = ManagementApiFactory.CreateSubscription(options, mock);
        var client = new ManagementClient(managementApi, subscriptionApi, contentConverter: converter);
        return (client, mock);
    }
}
