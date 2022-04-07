using FluentAssertions;
using Kentico.Kontent.Management.Models.Shared;
using System;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Modules.UrlBuilder;

public partial class EndpointUrlBuilderTests
{
    [Fact]
    public void BuildUsersUrl_ReturnsExpectedUrl()
    {
        var expectedResult = $"{ENDPOINT}/projects/{PROJECT_ID}/users";
        var actualResult = _builder.BuildUsersUrl();

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void BuildModifyUsersRoleUrl_WithEmail_ReturnsExpectedUrl()
    {
        var email = "test@test.test";
        var expectedResult = $"{ENDPOINT}/projects/{PROJECT_ID}/users/email/{email}/roles";
        var actualResult = _builder.BuildModifyUsersRoleUrl(UserIdentifier.ByEmail(email));

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void BuildModifyUsersRoleUrl_WithId_ReturnsExpectedUrl()
    {
        var id = "id";
        var expectedResult = $"{ENDPOINT}/projects/{PROJECT_ID}/users/{id}/roles";
        var actualResult = _builder.BuildModifyUsersRoleUrl(UserIdentifier.ById(id));

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void BuildModifyUsersRoleUrl_MissingId_MissingEmail_ThrowsException()
    {
        _builder.Invoking(x => x.BuildModifyUsersRoleUrl(UserIdentifier.ByEmail(null))).Should().ThrowExactly<ArgumentException>();
    }
}
