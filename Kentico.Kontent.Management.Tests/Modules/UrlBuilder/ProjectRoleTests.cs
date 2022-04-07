using Kentico.Kontent.Management.Models.Shared;
using System;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Modules.UrlBuilder;

public partial class EndpointUrlBuilderTests
{
    [Fact]
    public void BuildProjectRolesUrl_ReturnsCorrectUrl()
    {
        var actualUrl = _builder.BuildProjectRolesUrl();
        var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/roles";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildProjectRoleUrl_WithId_ReturnsCorrectUrl()
    {
        var roleIdentifier = Reference.ById(Guid.NewGuid());
        var actualUrl = _builder.BuildProjectRoleUrl(roleIdentifier);
        var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/roles/{roleIdentifier.Id}";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildProjectRoleUrl_WithCodename_ReturnsCorrectUrl()
    {
        var roleIdentifier = Reference.ByCodename("codename");
        var actualUrl = _builder.BuildProjectRoleUrl(roleIdentifier);
        var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/roles/codename/{roleIdentifier.Codename}";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildProjectRoleUrl_WithExternalId_Throws()
    {
        var roleIdentifier = Reference.ByExternalId("external");
        Assert.Throws<InvalidOperationException>(() => _builder.BuildProjectRoleUrl(roleIdentifier));
    }
}
