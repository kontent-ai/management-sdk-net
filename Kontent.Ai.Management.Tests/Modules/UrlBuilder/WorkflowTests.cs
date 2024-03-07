using Kontent.Ai.Management.Models.Shared;
using System;
using Xunit;

namespace Kontent.Ai.Management.Tests.Modules.UrlBuilder;

public partial class EndpointUrlBuilderTests
{
    [Fact]
    public void BuildWorkflowsUrl_ReturnsWorkflowsUrl()
    {
        var expectedUrl = $"{ENDPOINT}/projects/{ENVIRONMENT_ID}/workflows";
        var actualUrl = _builder.BuildWorkflowsUrl();

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildWorkflowsUrl_ById_ReturnsExpectedUrl()
    {
        var internalId = Guid.NewGuid();

        var actualUrl = _builder.BuildWorkflowsUrl(Reference.ById(internalId));

        var expectedUrl = $"{ENDPOINT}/projects/{ENVIRONMENT_ID}/workflows/{internalId}";
        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildWorkflowsUrl_ByCodename_ReturnsExpectedUrl()
    {
        var codename = "codename";

        var actualUrl = _builder.BuildWorkflowsUrl(Reference.ByCodename(codename));

        var expectedUrl = $"{ENDPOINT}/projects/{ENVIRONMENT_ID}/workflows/codename/{codename}";
        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildWorkflowsUrl_ByExternalId_NotSupported()
    {
        Assert.Throws<InvalidOperationException>(() => _builder.BuildWorkflowsUrl(Reference.ByExternalId("external_1234_%#$")));
    }
}
