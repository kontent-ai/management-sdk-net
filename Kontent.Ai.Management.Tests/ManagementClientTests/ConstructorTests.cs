using FluentAssertions;
using Kontent.Ai.Management.Configuration;
using System;
using Xunit;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class ConstructorTests
{
    [Fact]
    public void Ctor_ValidOptions_DoesNotThrow()
    {
        var options = new ManagementOptions
        {
            EnvironmentId = Guid.NewGuid().ToString(),
            ApiKey = "valid-key",
        };

        Action act = () => new ManagementClient(options);
        act.Should().NotThrow();
    }

    [Fact]
    public void Ctor_NullOptions_ThrowsArgumentNull()
    {
        Action act = () => new ManagementClient(null!);

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
            ApiKey = apiKey,
        };

        Action act = () => new ManagementClient(options);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage($"*{messagePart}*");
    }
}
