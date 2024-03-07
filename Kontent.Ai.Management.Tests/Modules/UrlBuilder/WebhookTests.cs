using Kontent.Ai.Management.Models.Shared;
using System;
using Xunit;

namespace Kontent.Ai.Management.Tests.Modules.UrlBuilder;

public partial class EndpointUrlBuilderTests
{
    [Fact]
    public void BuildWebhooksUrl_ReturnsCorrectUrl()
    {
        var actualUrl = _builder.BuildWebhooksUrl();
        var expectedUrl = $"{ENDPOINT}/projects/{ENVIRONMENT_ID}/webhooks-vnext";
        
        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildWebhooksUrl_ById_ReturnsCorrectUrl()
    {
        var identifier = Reference.ById(Guid.NewGuid());
        var actualUrl = _builder.BuildWebhooksUrl(identifier);
        var expectedUrl = $"{ENDPOINT}/projects/{ENVIRONMENT_ID}/webhooks-vnext/{identifier.Id}";
        
        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildWebhooksEnableUrl_ById_ReturnsCorrectUrl()
    {
        var identifier = Reference.ById(Guid.NewGuid());
        var actualUrl = _builder.BuildWebhooksEnableUrl(identifier);
        var expectedUrl = $"{ENDPOINT}/projects/{ENVIRONMENT_ID}/webhooks-vnext/{identifier.Id}/enable";
        
        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildWebhooksDisableUrl_ById_ReturnsCorrectUrl()
    {
        var identifier = Reference.ById(Guid.NewGuid());
        var actualUrl = _builder.BuildWebhooksDisableUrl(identifier);
        var expectedUrl = $"{ENDPOINT}/projects/{ENVIRONMENT_ID}/webhooks-vnext/{identifier.Id}/disable";
        
        Assert.Equal(expectedUrl, actualUrl);
    }
}