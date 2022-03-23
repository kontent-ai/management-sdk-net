using Kentico.Kontent.Management.Models.Shared;
using System;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Modules.UrlBuilder;

public partial class EndpointUrlBuilderTests
{
    [Fact]
    public void BuildSnippetsUrl_ReturnsExpectedUrl()
    {
        var actualUrl = _builder.BuildSnippetsUrl();
        var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/snippets";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildSnippetsUrl_ById_ReturnsExpectedUrl()
    {
        var identifier = Reference.ById(Guid.NewGuid());

        var actualUrl = _builder.BuildSnippetsUrl(identifier);
        var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/snippets/{identifier.Id}";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildSnippetsUrl_ByCodename_ReturnsExpectedUrl()
    {
        var identifier = Reference.ByCodename("codename");

        var actualUrl = _builder.BuildSnippetsUrl(identifier);
        var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/snippets/codename/{identifier.Codename}";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildSnippetsUrl_ByExternalId_ReturnsExpectedUrl()
    {
        var identifier = Reference.ByExternalId("external");

        var actualUrl = _builder.BuildSnippetsUrl(identifier);
        var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/snippets/external-id/{identifier.ExternalId}";

        Assert.Equal(expectedUrl, actualUrl);
    }
}
