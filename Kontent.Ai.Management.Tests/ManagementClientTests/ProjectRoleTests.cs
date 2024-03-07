using FluentAssertions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class EnvironmentRoleTests : IClassFixture<FileSystemFixture>
{
    private readonly Scenario _scenario;

    public EnvironmentRoleTests()
    {
        _scenario = new Scenario(folder: "ProjectRole");
    }

    [Fact]
    public async void ListEnvironmentRolesAsync_ListsEnvironmentRoles()
    {
        var client = _scenario
            .WithResponses("ProjectRoles.json")
            .CreateManagementClient();

        var response = await client.ListEnvironmentRolesAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/roles")
            .Validate();
    }

    [Fact]
    public async Task GetEnvironmentRoleAsync_ById_GetsEnvironmentRole()
    {
        var client = _scenario
            .WithResponses("ProjectRole.json")
            .CreateManagementClient();

        var identifier = Reference.ById(Guid.NewGuid());
        var response = await client.GetEnvironmentRoleAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/roles/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async Task GetEnvironmentRoleAsync_ByCodename_GetsEnvironmentRole()
    {
        var client = _scenario
            .WithResponses("ProjectRole.json")
            .CreateManagementClient();

        var identifier = Reference.ByCodename("codename");
        var response = await client.GetEnvironmentRoleAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/roles/codename/{identifier.Codename}")
            .Validate();
    }

    [Fact]
    public async Task GetEnvironmentRoleAsync_ByExternalId_Throws()
    {
        var client = _scenario
            .WithResponses("ProjectRole.json")
            .CreateManagementClient();

        var identifier = Reference.ByExternalId("externalId");
        await client.Invoking(x => x.GetEnvironmentRoleAsync(identifier)).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task GetEnvironmentRoleAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario
            .WithResponses("ProjectRole.json")
            .CreateManagementClient();

        await client.Invoking(x => x.GetEnvironmentRoleAsync(null)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }
}
