using Kontent.Ai.Management.Models.Shared;
using System;
using Xunit;

namespace Kontent.Ai.Management.Tests.Modules.UrlBuilder;

public partial class EndpointUrlBuilderTests
{
    [Fact]
    public void BuildSpacesUrl_ReturnsSpacesUrl()
    {
        var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/spaces";
        var actualUrl = _builder.BuildSpacesUrl();

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildSpacesUrl_ById_ReturnsSpacesUrl()
    {
        var identifier = Reference.ById(Guid.NewGuid());
        var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/spaces/{identifier.Id}";
        var actualUrl = _builder.BuildSpacesUrl(identifier);

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildSpacesUrl_ByCodename_ReturnsSpacesUrl()
    {
        var identifier = Reference.ByCodename("space_codename");
        var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/spaces/codename/{identifier.Codename}";
        var actualUrl = _builder.BuildSpacesUrl(identifier);

        Assert.Equal(expectedUrl, actualUrl);
    }
}