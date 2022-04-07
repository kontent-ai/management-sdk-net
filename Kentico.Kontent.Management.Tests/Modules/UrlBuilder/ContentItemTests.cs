using Kentico.Kontent.Management.Models.Shared;
using System;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Modules.UrlBuilder;

public partial class EndpointUrlBuilderTests
{
    [Fact]
    public void BuildItemsUrl_ReturnsCorrectUrl()
    {
        var actualUrl = _builder.BuildItemsUrl();
        var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildItemUrl_ItemId_ReturnsCorrectUrl()
    {
        var identifier = Reference.ById(Guid.NewGuid());
        var actualUrl = _builder.BuildItemUrl(identifier);
        var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/{identifier.Id}";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildItemUrl_ItemCodename_ReturnsCorrectUrl()
    {
        var identifier = Reference.ByCodename("codename");
        var actualUrl = _builder.BuildItemUrl(identifier);
        var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/codename/{identifier.Codename}";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildItemUrl_ItemExternalId_ReturnsCorrectUrl()
    {
        var identifier = Reference.ByExternalId("externalId");
        var actualUrl = _builder.BuildItemUrl(identifier);
        var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/external-id/{identifier.ExternalId}";

        Assert.Equal(expectedUrl, actualUrl);
    }
}
