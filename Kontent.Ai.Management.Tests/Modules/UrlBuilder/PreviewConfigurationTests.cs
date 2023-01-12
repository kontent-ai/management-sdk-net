using Xunit;

namespace Kontent.Ai.Management.Tests.Modules.UrlBuilder;

public partial class EndpointUrlBuilderTests
{
    [Fact]
    public void BuildPreviewConfigurationUrl_ReturnsPreviewConfigurationUrl()
    {
        var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/preview-configuration";
        var actualUrl = _builder.BuildPreviewConfigurationUrl();

        Assert.Equal(expectedUrl, actualUrl);
    }
}
