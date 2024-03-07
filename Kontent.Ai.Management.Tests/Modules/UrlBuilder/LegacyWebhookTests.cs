using Kontent.Ai.Management.Models.Shared;
using System;
using Xunit;

namespace Kontent.Ai.Management.Tests.Modules.UrlBuilder;

public partial class EndpointUrlBuilderTests
{
    [Fact]
    public void BuildLegacyWebhooksUrl_ReturnsCorrectUrl()
    {
        var actualUrl = _builder.BuildLegacyWebhooksUrl();
        var expectedUrl = $"{ENDPOINT}/projects/{ENVIRONMENT_ID}/webhooks";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildLegacyWebhooksUrl_ById_ReturnsCorrectUrl()
    {
        var identifier = Reference.ById(Guid.NewGuid());
        var actualUrl = _builder.BuildLegacyWebhooksUrl(identifier);
        var expectedUrl = $"{ENDPOINT}/projects/{ENVIRONMENT_ID}/webhooks/{identifier.Id}";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildLegacyWebhooksEnableUrl_ById_ReturnsCorrectUrl()
    {
        var identifier = Reference.ById(Guid.NewGuid());
        var actualUrl = _builder.BuildLegacyWebhooksEnableUrl(identifier);
        var expectedUrl = $"{ENDPOINT}/projects/{ENVIRONMENT_ID}/webhooks/{identifier.Id}/enable";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildLegacyWebhooksDisableUrl_ById_ReturnsCorrectUrl()
    {
        var identifier = Reference.ById(Guid.NewGuid());
        var actualUrl = _builder.BuildLegacyWebhooksDisableUrl(identifier);
        var expectedUrl = $"{ENDPOINT}/projects/{ENVIRONMENT_ID}/webhooks/{identifier.Id}/disable";

        Assert.Equal(expectedUrl, actualUrl);
    }
}