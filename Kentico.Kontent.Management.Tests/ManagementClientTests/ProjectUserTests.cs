using FluentAssertions;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Users;
using Kentico.Kontent.Management.Tests.Base;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
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
                email = "test@kentico.com",
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
}
