using Xunit;

namespace Kontent.Ai.Management.Tests.Modules.UrlBuilder;

public partial class EndpointUrlBuilderTests
{
    [Fact]
    public void BuildWebSpotlightUrl_ReturnsWebSpotlightUrl()
    {
        var expectedUrl = $"{ENDPOINT}/projects/{ENVIRONMENT_ID}/web-spotlight";
        var actualUrl = _builder.BuildWebSpotlightUrl();

        Assert.Equal(expectedUrl, actualUrl);
    }
}