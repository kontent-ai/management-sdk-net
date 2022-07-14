using Kentico.Kontent.Management.Models.Shared;
using System;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Modules.UrlBuilder;

public partial class EndpointUrlBuilderTests
{
    [Fact]
    public void BuildWebhooksUrl_ReturnsCorrectUrl()
    {
        var actualUrl = _builder.BuildWebhooksUrl();
        var expectedUrl = $"{ ENDPOINT}/projects/{ PROJECT_ID}/webhooks";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildWebhooksEnableUrl_ById_ReturnsCorrectUrl()
    {
        var identifier = Reference.ById(Guid.NewGuid());
        var actualUrl = _builder.BuildWebhooksEnableUrl(identifier);
        var expectedUrl = $"{ ENDPOINT}/projects/{ PROJECT_ID}/webhooks/{identifier.Id}/enable";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildWebhooksDisableUrl_ById_ReturnsCorrectUrl()
    {
        var identifier = Reference.ById(Guid.NewGuid());
        var actualUrl = _builder.BuildWebhooksDisableUrl(identifier);
        var expectedUrl = $"{ ENDPOINT}/projects/{ PROJECT_ID}/webhooks/{identifier.Id}/disable";

        Assert.Equal(expectedUrl, actualUrl);
    }
}