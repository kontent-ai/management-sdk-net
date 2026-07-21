using AwesomeAssertions;
using Kontent.Ai.Management.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class ManagementClientBuilderTests
{
    private static Action<ManagementOptions> ValidOptions => o =>
    {
        o.EnvironmentId = Guid.NewGuid().ToString();
        o.ApiKey = "valid-key";
    };

    [Fact]
    public void WithOptions_Action_Build_ReturnsClient()
    {
        using var client = ManagementClientBuilder.WithOptions(ValidOptions).Build();

        client.Should().NotBeNull().And.BeAssignableTo<IManagementClient>();
    }

    [Fact]
    public void WithOptions_Instance_Build_ReturnsClient()
    {
        var options = new ManagementOptions
        {
            EnvironmentId = Guid.NewGuid().ToString(),
            ApiKey = "valid-key",
        };

        using var client = ManagementClientBuilder.WithOptions(options).Build();

        client.Should().NotBeNull();
    }

    [Fact]
    public async Task Build_ClientIsAsyncDisposable()
    {
        var client = ManagementClientBuilder.WithOptions(ValidOptions).Build();

        await ((IAsyncDisposable)client).Invoking(c => c.DisposeAsync().AsTask()).Should().NotThrowAsync();
    }

    [Fact]
    public void WithResilience_And_ConfigureRefit_AreChainable()
    {
        using var client = ManagementClientBuilder
            .WithOptions(ValidOptions)
            .WithResilience(_ => { })
            .ConfigureRefit(_ => { })
            .Build();

        client.Should().NotBeNull();
    }

    [Fact]
    public void WithOptions_NullAction_Throws()
    {
        Action act = () => ManagementClientBuilder.WithOptions((Action<ManagementOptions>)null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void WithOptions_NullInstance_Throws()
    {
        Action act = () => ManagementClientBuilder.WithOptions((ManagementOptions)null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void WithResilience_Null_Throws()
    {
        Action act = () => ManagementClientBuilder.WithOptions(ValidOptions).WithResilience(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ConfigureRefit_Null_Throws()
    {
        Action act = () => ManagementClientBuilder.WithOptions(ValidOptions).ConfigureRefit(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(null, "key")]
    [InlineData("no-guid", "key")]
    [InlineData("00000000-0000-0000-0000-000000000000", "key")]
    [InlineData("4ee3d5cc-2e5b-4c81-9f4c-6a8f7b5d3c1e", null)]
    public void Build_InvalidOptions_ThrowsValidationException(string? envId, string? apiKey)
    {
        Action act = () => ManagementClientBuilder
            .WithOptions(o =>
            {
                o.EnvironmentId = envId!;
                o.ApiKey = apiKey!;
            })
            .Build();

        act.Should().Throw<ValidationException>();
    }
}
