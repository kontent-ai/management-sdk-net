using FluentAssertions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Users;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class ProjectUserTests : IClassFixture<FileSystemFixture>
{
    private readonly Scenario _scenario;

    public ProjectUserTests()
    {
        _scenario = new Scenario(folder: "ProjectUser");
    }

    [Fact]
    public async Task InviteUserIntoProjectAsync_InvitesUser()
    {
        var client = _scenario
            .WithResponses("ProjectUser.json")
            .CreateManagementClient();

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

        var response = await client.InviteUserIntoProjectAsync(invitation);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Post)
            .RequestPayload(invitation)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/users")
            .Validate();
    }

    
    [Fact]
    public async Task InviteUserIntoProjectAsync_UserInvitationModelIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.InviteUserIntoProjectAsync(null)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    
    [Fact]
    public async Task ModifyUsersRolesAsync_ByEmail_ModifiesUserRoles()
    {
        var client = _scenario
            .WithResponses("ProjectUser.json")
            .CreateManagementClient();

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
        var response = await client.ModifyUsersRolesAsync(identifier, user);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/users/email/{identifier.Email}/roles")
            .Validate();
    }

    [Fact]
    public async Task ModifyUsersRolesAsync_ById_ModifiesUserRoles()
    {
        var client = _scenario
        .WithResponses("ProjectUser.json")
        .CreateManagementClient();

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
        var response = await client.ModifyUsersRolesAsync(identifier, user);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/users/{identifier.Id}/roles")
            .Validate();
    }

    [Fact]
    public async Task ModifyUsersRolesAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.ModifyUsersRolesAsync(null, new UserModel())).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ModifyUsersRolesAsync_UserModelIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.ModifyUsersRolesAsync(UserIdentifier.ById("userId"), null)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }
}
