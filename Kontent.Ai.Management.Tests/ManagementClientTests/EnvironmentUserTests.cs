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
                        new UserRoleModel
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
    public async Task UpdateUserRolesAsync_ByEmail_ModifiesUserRoles()
    {
        var (client, mock) = MockClientFactory.Create();
        var roles = new UserRolesUpdateModel
        {
            CollectionGroups =
            [
                new UserCollectionGroup
                {
                    Collections = [Reference.ById(Guid.NewGuid()), Reference.ById(Guid.NewGuid())],
                    Roles =
                    [
                        new UserRoleModel
                        {
                            Id = Guid.NewGuid(),
                            Languages = [Reference.ById(Guid.NewGuid()), Reference.ById(Guid.NewGuid())]
                        }
                    ]
                }
            ]
        };

        var identifier = UserIdentifier.ByEmail("test@kontent.ai");
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/users/email/{Uri.EscapeDataString(identifier.Email!)}/roles")
            .CaptureBody(out var body)
            .Respond("application/json", ProjectUser);

        var result = await client.UpdateUserRolesAsync(identifier, roles);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<UserModel>(ProjectUser, SharedTestJsonOptions.Default));
        body.ShouldMatchSerialized(roles);
        body.Value.Should().NotContain("user_id", "the user is identified by the URL, not the body");
    }

    [Fact]
    public async Task UpdateUserRolesAsync_ById_ModifiesUserRoles()
    {
        var (client, mock) = MockClientFactory.Create();
        var roles = new UserRolesUpdateModel
        {
            CollectionGroups =
            [
                new UserCollectionGroup
                {
                    Collections = [Reference.ById(Guid.NewGuid()), Reference.ById(Guid.NewGuid())],
                    Roles =
                    [
                        new UserRoleModel
                        {
                            Id = Guid.NewGuid(),
                            Languages = [Reference.ById(Guid.NewGuid()), Reference.ById(Guid.NewGuid())]
                        }
                    ]
                }
            ]
        };

        var identifier = UserIdentifier.ById("userId");
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/users/{identifier.Id}/roles")
            .Respond("application/json", ProjectUser);

        var result = await client.UpdateUserRolesAsync(identifier, roles);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<UserModel>(ProjectUser, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task UpdateUserRolesAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.UpdateUserRolesAsync(null!, new UserRolesUpdateModel { CollectionGroups = [] })).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateUserRolesAsync_UserModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.UpdateUserRolesAsync(UserIdentifier.ById("userId"), null!)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }
}
