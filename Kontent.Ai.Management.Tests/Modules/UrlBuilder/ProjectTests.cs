using Xunit;

namespace Kontent.Ai.Management.Tests.Modules.UrlBuilder;

public partial class EndpointUrlBuilderTests
{
    [Fact]
    public void BuildEnvironmentUrl_ReturnsCorrectUrl()
    {
        var actualUrl = _builder.BuildEnvironmentUrl();
        var expectedUrl = $"{ENDPOINT}/projects/{ENVIRONMENT_ID}";

        Assert.Equal(expectedUrl, actualUrl);
    }
}
