using Kontent.Ai.Management.Models.Shared;
using System;
using Xunit;

namespace Kontent.Ai.Management.Tests.Modules.UrlBuilder;

public partial class EndpointUrlBuilderTests
{
    [Fact]
    public void BuildEnvironmentRolesUrl_ReturnsCorrectUrl()
    {
        var actualUrl = _builder.BuildEnvironmentRolesUrl();
        var expectedUrl = $"{ENDPOINT}/projects/{ENVIRONMENT_ID}/roles";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildEnvironmentRoleUrl_WithId_ReturnsCorrectUrl()
    {
        var roleIdentifier = Reference.ById(Guid.NewGuid());
        var actualUrl = _builder.BuildEnvironmentRoleUrl(roleIdentifier);
        var expectedUrl = $"{ENDPOINT}/projects/{ENVIRONMENT_ID}/roles/{roleIdentifier.Id}";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildEnvironmentRoleUrl_WithCodename_ReturnsCorrectUrl()
    {
        var roleIdentifier = Reference.ByCodename("codename");
        var actualUrl = _builder.BuildEnvironmentRoleUrl(roleIdentifier);
        var expectedUrl = $"{ENDPOINT}/projects/{ENVIRONMENT_ID}/roles/codename/{roleIdentifier.Codename}";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildEnvironmentRoleUrl_WithExternalId_Throws()
    {
        var roleIdentifier = Reference.ByExternalId("external");
        Assert.Throws<InvalidOperationException>(() => _builder.BuildEnvironmentRoleUrl(roleIdentifier));
    }
}
