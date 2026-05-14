using FluentAssertions;
using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class DisposeTests
{
    private static ManagementOptions ValidOptions() => new()
    {
        EnvironmentId = Guid.NewGuid().ToString(),
        ApiKey = "dummy",
    };

    [Fact]
    public void Dispose_OnCtorBuiltClient_IsIdempotent()
    {
        var client = new ManagementClient(ValidOptions());

        // First disposal releases the HttpClients; a second call must not throw.
        client.Dispose();
        Action act = () => client.Dispose();

        act.Should().NotThrow();
    }

    [Fact]
    public async Task DisposeAsync_OnCtorBuiltClient_Succeeds()
    {
        var client = new ManagementClient(ValidOptions());

        Func<Task> act = async () => await client.DisposeAsync();

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public void Dispose_OnDiResolvedClient_IsNoop()
    {
        // The DI factory passes `null` for ownedResources because the host container owns the HttpClient via
        // IHttpClientFactory. The interface (IManagementClient) doesn't expose IDisposable; consumers don't dispose
        // DI-resolved instances. The concrete ManagementClient implements IDisposable, and its Dispose must be safe
        // (no factory corruption) when the container ultimately disposes the singleton.
        var services = new ServiceCollection();
        services.AddManagementClient(o =>
        {
            o.EnvironmentId = Guid.NewGuid().ToString();
            o.ApiKey = "dummy";
        });

        using var provider = services.BuildServiceProvider();
        var client = (ManagementClient)provider.GetRequiredService<IManagementClient>();

        Action act = () => client.Dispose();

        act.Should().NotThrow();
    }
}
