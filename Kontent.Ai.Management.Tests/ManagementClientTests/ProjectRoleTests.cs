using FluentAssertions;
using Kontent.Ai.Management.Models.Roles;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Threading.Tasks;
using Xunit;

using static FluentAssertions.FluentActions;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class ProjectRoleTests : IClassFixture<FileSystemFixture>
{
    private readonly FileSystemFixture _fileSystemFixture;

    public ProjectRoleTests(FileSystemFixture fileSystemFixture)
    {
        _fileSystemFixture = fileSystemFixture;
        _fileSystemFixture.SetSubFolder("ProjectRole");
    }

    [Fact]
    public async Task ListProjectRoles_ListsProjectRoles()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("ProjectRoles.json");

        var expectedItems = _fileSystemFixture.GetExpectedResponse<ProjectRolesModel>("ProjectRoles.json");

        var response = await client.ListProjectRolesAsync();

        response.Should().BeEquivalentTo(expectedItems);
    }

    [Fact]
    public async Task GetProjectRole_NoIdentifier_ExceptionRaised()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("ProjectRole.json");

        await Awaiting(() => client.GetProjectRoleAsync(null))
            .Should()
            .ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetProjectRole_ById_GetsProjectRole()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("ProjectRole.json");

        var expected = _fileSystemFixture.GetExpectedResponse<ProjectRoleModel>("ProjectRole.json");

        var response = await client.GetProjectRoleAsync(Reference.ById(expected.Id));

        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetProjectRole_ByCodename_GetsProjectRole()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("ProjectRole.json");

        var expected = _fileSystemFixture.GetExpectedResponse<ProjectRoleModel>("ProjectRole.json");

        var response = await client.GetProjectRoleAsync(Reference.ByCodename(expected.Codename));

        response.Should().BeEquivalentTo(expected);
    }
}
