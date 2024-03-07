using Xunit;

namespace Kontent.Ai.Management.Tests.Modules.UrlBuilder;

public partial class EndpointUrlBuilderTests
{
    [Fact]
    public void BuildValidationUrl_ReturnsValidationUrl()
    {
        var expectedUrl = $"{ENDPOINT}/projects/{ENVIRONMENT_ID}/validate";
        var actualUrl = _builder.BuildValidationUrl();

        Assert.Equal(expectedUrl, actualUrl);
    }
}
