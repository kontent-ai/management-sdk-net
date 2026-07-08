using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Conversion;
using RichardSzalay.MockHttp;
using System.Net;

namespace Kontent.Ai.Management.Tests.Base;

/// <summary>
/// Builds an <see cref="IManagementClient"/> whose Refit transport is backed by a
/// <see cref="MockHttpMessageHandler"/>, wired in as the inner handler through <see cref="ManagementApiFactory"/>,
/// so tests arrange responses and assert outgoing requests via MockHttp's fluent API. Supply a pre-registered
/// <c>converter</c> for strongly-typed variant paths: the client's own converter would otherwise
/// auto-scan the whole test assembly and trip the intentional codename collision among the test fixtures.
/// </summary>
internal static class MockClientFactory
{
    public const string EnvironmentId = "a9931a80-9af4-010b-0590-ecb1273cf1b8";
    public const string SubscriptionId = "9c7b9841-ea99-48a7-a46d-65b2549d6c05";

    // Refit composes "{BaseAddress}{relative path}", so the base must carry no trailing slash or every path
    // doubles the separator. Endpoint defaults to "https://manage.kontent.ai"; the SDK appends "/v2/projects/{id}".
    public static string BaseUrl => $"https://manage.kontent.ai/v2/projects/{EnvironmentId}";

    // Subscription-scoped endpoints resolve against the subscription scope instead of the project.
    public static string SubscriptionBaseUrl => $"https://manage.kontent.ai/v2/subscriptions/{SubscriptionId}";

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

    // Doc-sample helpers. Samples don't assert the outgoing request — they just need the call to return a canned
    // (or empty) response so the snippet runs. The catch-all responder is FIFO-clamped over the given fixtures:
    // each call returns the next fixture, the last one repeats, and with none the response is an empty 200 OK.
    public static IManagementClient CreateForSample(string folder, params string[] fixtureFiles)
    {
        var bodies = fixtureFiles.Select(f => File.ReadAllText(SamplePath(folder, f))).ToArray();
        var mock = new MockHttpMessageHandler();
        var index = 0;
        mock.Fallback.Respond(request =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK) { RequestMessage = request };
            if (bodies.Length > 0)
            {
                response.Content = new StringContent(bodies[Math.Min(index++, bodies.Length - 1)]);
            }
            return response;
        });

        var options = new ManagementOptions
        {
            ApiKey = "Dummy_API_key",
            EnvironmentId = EnvironmentId,
            SubscriptionId = SubscriptionId,
        };
        return new ManagementClient(
            ManagementApiFactory.Create(options, mock),
            ManagementApiFactory.CreateSubscription(options, mock));
    }

    private static string SamplePath(string folder, string file)
        => Path.Combine(Environment.CurrentDirectory, "Data", folder, file);
}
