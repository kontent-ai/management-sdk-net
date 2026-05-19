using System.ComponentModel.DataAnnotations;
using AwesomeAssertions;
using Kontent.Ai.Management.Configuration;
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
    [InlineData(null, "key")]                                              // missing env id ([Required])
    [InlineData("no-guid", "key")]                                         // non-GUID env id (IValidatableObject)
    [InlineData("00000000-0000-0000-0000-000000000000", "key")]            // Guid.Empty (IValidatableObject)
    [InlineData("4ee3d5cc-2e5b-4c81-9f4c-6a8f7b5d3c1e", null)]              // missing api key ([Required])
    public void Ctor_InvalidOptions_ThrowsValidationException(string? envId, string? apiKey)
    {
        // Validation goes through Validator.ValidateObject inside BuildDependencies, surfacing as
        // ValidationException — consistent with the builder path and with the standard .NET options pattern.
        var options = new ManagementOptions
        {
            EnvironmentId = envId!,
            ApiKey = apiKey!,
        };

        Action act = () => new ManagementClient(options);

        act.Should().Throw<ValidationException>();
    }
}
