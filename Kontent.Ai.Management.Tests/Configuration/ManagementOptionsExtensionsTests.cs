using AwesomeAssertions;
using Kontent.Ai.Management.Configuration;

namespace Kontent.Ai.Management.Tests.Configuration;

public class ManagementOptionsExtensionsTests
{
    [Theory]
    [InlineData("https://manage.kontent.ai")]
    [InlineData("https://manage.kontent.ai/")] // trailing slash must not double the separator
    public void ScopedEndpoint_ComposesV2ScopedAddress(string endpoint)
    {
        var options = new ManagementOptions { Endpoint = endpoint };

        options.ScopedEndpoint("projects/abc")
            .Should().Be(new Uri("https://manage.kontent.ai/v2/projects/abc"));
    }
}
