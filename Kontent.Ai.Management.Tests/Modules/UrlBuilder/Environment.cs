using Xunit;

namespace Kontent.Ai.Management.Tests.Modules.UrlBuilder;

public partial class EndpointUrlBuilderTests
{
    [Fact]
    public void BuildCloneEnvironmentUrl_ReturnsCorrectUrl()
    {
        var actualUrl = _builder.BuildCloneEnvironmentUrl();
        var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/clone-environment";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildGetEnvironmentCloningStateUrl_ReturnsCorrectUrl()
    {
        var actualUrl = _builder.BuildGetEnvironmentCloningStateUrl();
        var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/environment-cloning-state";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildMarkEnvironmentAsProductionUrl_ReturnsCorrectUrl()
    {
        var actualUrl = _builder.BuildMarkEnvironmentAsProductionUrl();
        var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/mark-environment-as-production";

        Assert.Equal(expectedUrl, actualUrl);
    }
}
