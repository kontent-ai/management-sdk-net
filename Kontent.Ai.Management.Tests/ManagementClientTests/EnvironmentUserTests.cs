using AwesomeAssertions;
using Kontent.Ai.Management.Models.Users;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using System.Text.Json;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class EnvironmentUserTests
{
    private static string ProjectUser => Fixture("ProjectUser.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "ProjectUser", name));

    [Fact]
    public async Task InviteUserIntoProjectAsync_InvitesUser()
    {
        var (client, mock) = MockClientFactory.Create();
        var invitation = new UserInviteModel
        {
            Email = "test@kontent.ai",
            CollectionGroups = new[] {
                new UserCollectionGroup
                {
                    Collections = new [] { Reference.ById(Guid.NewGuid()), Reference.ById(Guid.NewGuid()) },
                    Roles = new[] {
                        new RoleModel
                        {
                            Id = Guid.NewGuid(),
                            Languages = new [] { Reference.ById(Guid.NewGuid()), Reference.ById(Guid.NewGuid()) }
                        }
                    }
                }
            }
        };

        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/users")
            .CaptureBody(out var capturedBody)
            .Respond("application/json", ProjectUser);

        var result = await client.InviteUserIntoEnvironmentAsync(invitation);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<UserModel>(ProjectUser, SharedTestJsonOptions.Default));
        capturedBody.ShouldMatchSerialized(invitation);
    }

    [Fact]
    public async Task InviteUserIntoProjectAsync_UserInvitationModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.InviteUserIntoEnvironmentAsync(null!)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ModifyUsersRolesAsync_ByEmail_ModifiesUserRoles()
    {
        var (client, mock) = MockClientFactory.Create();
        var user = new UserModel
        {
            CollectionGroups = new[] {
                new UserCollectionGroup
                {
                    Collections = new [] { Reference.ById(Guid.NewGuid()), Reference.ById(Guid.NewGuid()) },
                    Roles = new[] {
                        new RoleModel
                        {
                            Id = Guid.NewGuid(),
                            Languages = new [] { Reference.ById(Guid.NewGuid()), Reference.ById(Guid.NewGuid()) }
                        }
                    }
                }
            },
            Id = "somethingId"
        };

        var identifier = UserIdentifier.ByEmail("test@kontent.ai");
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/users/email/{Uri.EscapeDataString(identifier.Email!)}/roles")
            .Respond("application/json", ProjectUser);

        var result = await client.ModifyUsersRolesAsync(identifier, user);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<UserModel>(ProjectUser, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task ModifyUsersRolesAsync_ById_ModifiesUserRoles()
    {
        var (client, mock) = MockClientFactory.Create();
        var user = new UserModel
        {
            CollectionGroups = new[] {
                new UserCollectionGroup
                {
                    Collections = new [] { Reference.ById(Guid.NewGuid()), Reference.ById(Guid.NewGuid()) },
                    Roles = new[] {
                        new RoleModel
                        {
                            Id = Guid.NewGuid(),
                            Languages = new [] { Reference.ById(Guid.NewGuid()), Reference.ById(Guid.NewGuid()) }
                        }
                    }
                }
            },
            Id = "somethingId"
        };

        var identifier = UserIdentifier.ById("userId");
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/users/{identifier.Id}/roles")
            .Respond("application/json", ProjectUser);

        var result = await client.ModifyUsersRolesAsync(identifier, user);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<UserModel>(ProjectUser, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task ModifyUsersRolesAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ModifyUsersRolesAsync(null!, new UserModel { Id = "usr_x", CollectionGroups = [] })).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ModifyUsersRolesAsync_UserModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ModifyUsersRolesAsync(UserIdentifier.ById("userId"), null!)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }
}
