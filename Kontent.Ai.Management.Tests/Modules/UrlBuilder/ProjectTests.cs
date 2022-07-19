using Xunit;

namespace Kontent.Ai.Management.Tests.Modules.UrlBuilder;

public partial class EndpointUrlBuilderTests
{
    [Fact]
    public void BuildProjectUrl_ReturnsCorrectUrl()
    {
        var actualUrl = _builder.BuildProjectUrl();
        var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}";

        Assert.Equal(expectedUrl, actualUrl);
    }
}
