using FluentAssertions;
using Kontent.Ai.Management.Models.Users;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Users;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class ProjectUserTests : IClassFixture<FileSystemFixture>
{
    private readonly FileSystemFixture _fileSystemFixture;

    public ProjectUserTests(FileSystemFixture fileSystemFixture)
    {
        _fileSystemFixture = fileSystemFixture;
        _fileSystemFixture.SetSubFolder("ProjectUser");
    }

    [Fact]
    public async Task InviteUser_InvitesUser()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("ProjectUser.json");

        var expected = _fileSystemFixture.GetExpectedResponse<UserModel>("ProjectUser.json");

        var invitation = new UserInviteModel
        {
            Email = "test@kentico.com",
            CollectionGroup = expected.CollectionGroup
        };

        var response = await client.InviteUserIntoProjectAsync(invitation);

        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task InviteUser_UserInvitationModelNotProvided_ThrowsException()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("ProjectUser.json");

        await client.Invoking(x => x.InviteUserIntoProjectAsync(null)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ModifyUsersRole_ByEmail_ModifiesUserRoles()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("ProjectUser.json");

        var expected = _fileSystemFixture.GetExpectedResponse<UserModel>("ProjectUser.json");

        var response = await client.ModifyUsersRolesAsync(UserIdentifier.ByEmail("test@kentico.com"), expected);

        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ModifyUsersRole_ById_ModifiesUserRoles()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("ProjectUser.json");

        var expected = _fileSystemFixture.GetExpectedResponse<UserModel>("ProjectUser.json");

        var response = await client.ModifyUsersRolesAsync(UserIdentifier.ById(expected.Id), expected);

        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ModifyUsersRole_NullIdentifier_ModifiesUserRoles()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("ProjectUser.json");

        await client.Invoking(x => x.ModifyUsersRolesAsync(null, new UserModel())).Should().ThrowExactlyAsync<ArgumentNullException>();
    }
}
