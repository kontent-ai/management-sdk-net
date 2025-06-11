using FluentAssertions;
using Kontent.Ai.Management.Configuration;
using System;
using System.Net.Http;
using Xunit;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class ConstructorTests
{
    private static ManagementClient CreateClient(ManagementOptions options, bool useHttpClient)
        => useHttpClient
            ? new ManagementClient(options, new HttpClient())
            : new ManagementClient(options);

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Ctor_ValidOptions_DoesNotThrow(bool useHttpClient)
    {
        var options = new ManagementOptions
        {
            EnvironmentId = Guid.NewGuid().ToString(),
            ApiKey = "valid-key"
        };

        Action act = () => CreateClient(options, useHttpClient);
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Ctor_NullOptions_ThrowsArgumentNull(bool useHttpClient)
    {
        Action act = () => CreateClient(null!, useHttpClient);

        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData("", "key", "environment identifier is not specified")]
    [InlineData("no-guid", "key", "not a valid environment identifier")]
    [InlineData("4ee3d5cc-2e5b-4c81-9f4c-6a8f7b5d3c1e", "", "API key is not specified")]
    public void Ctor_InvalidOptions_ThrowsArgument(
        string envId,
        string apiKey,
        string messagePart)
    {
        var options = new ManagementOptions
        {
            EnvironmentId = envId,
            ApiKey = apiKey
        };

        Action act = () => CreateClient(options, false);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage($"*{messagePart}*");
    }
}
