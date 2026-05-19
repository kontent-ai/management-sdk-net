using AwesomeAssertions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Users;
using Kontent.Ai.Management.Tests.Base;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Xunit;

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
            CollectionGroup = new[] {
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

        string? capturedBody = null;
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/users")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", ProjectUser);

        var response = await client.InviteUserIntoEnvironmentAsync(invitation);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<UserModel>(ProjectUser));
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<UserInviteModel>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<UserInviteModel>(JsonConvert.SerializeObject(invitation)));
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
            CollectionGroup = new[] {
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
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/users/email/{Uri.EscapeDataString(identifier.Email)}/roles")
            .Respond("application/json", ProjectUser);

        var response = await client.ModifyUsersRolesAsync(identifier, user);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<UserModel>(ProjectUser));
    }

    [Fact]
    public async Task ModifyUsersRolesAsync_ById_ModifiesUserRoles()
    {
        var (client, mock) = MockClientFactory.Create();
        var user = new UserModel
        {
            CollectionGroup = new[] {
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

        var response = await client.ModifyUsersRolesAsync(identifier, user);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<UserModel>(ProjectUser));
    }

    [Fact]
    public async Task ModifyUsersRolesAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ModifyUsersRolesAsync(null!, new UserModel())).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ModifyUsersRolesAsync_UserModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ModifyUsersRolesAsync(UserIdentifier.ById("userId"), null!)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }
}
