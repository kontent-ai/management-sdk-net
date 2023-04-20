using FluentAssertions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class ProjectRoleTests : IClassFixture<FileSystemFixture>
{
    private readonly Scenario _scenario;

    public ProjectRoleTests()
    {
        _scenario = new Scenario(folder: "ProjectRole");
    }

    [Fact]
    public async void ListProjectRolesAsync_ListsProjectRoles()
    {
        var client = _scenario
            .WithResponses("ProjectRoles.json")
            .CreateManagementClient();

        var response = await client.ListProjectRolesAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/roles")
            .Validate();
    }

    [Fact]
    public async Task GetProjectRoleAsync_ById_GetsProjectRole()
    {
        var client = _scenario
            .WithResponses("ProjectRole.json")
            .CreateManagementClient();

        var identifier = Reference.ById(Guid.NewGuid());
        var response = await client.GetProjectRoleAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/roles/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async Task GetProjectRoleAsync_ByCodename_GetsProjectRole()
    {
        var client = _scenario
            .WithResponses("ProjectRole.json")
            .CreateManagementClient();

        var identifier = Reference.ByCodename("codename");
        var response = await client.GetProjectRoleAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/roles/codename/{identifier.Codename}")
            .Validate();
    }

    [Fact]
    public async Task GetProjectRoleAsync_ByExternalId_Throws()
    {
        var client = _scenario
            .WithResponses("ProjectRole.json")
            .CreateManagementClient();

        var identifier = Reference.ByExternalId("externalId");
        await client.Invoking(x => x.GetProjectRoleAsync(identifier)).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task GetProjectRoleAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario
            .WithResponses("ProjectRole.json")
            .CreateManagementClient();

        await client.Invoking(x => x.GetProjectRoleAsync(null)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }
}
