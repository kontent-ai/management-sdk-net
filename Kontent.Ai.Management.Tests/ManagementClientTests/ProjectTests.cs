using FluentAssertions;
using Kontent.Ai.Management.Models.ProjectReport;
using Kontent.Ai.Management.Tests.Base;
using System.Threading.Tasks;
using Xunit;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class ProjectTests : IClassFixture<FileSystemFixture>
{
    private readonly FileSystemFixture _fileSystemFixture;

    public ProjectTests(FileSystemFixture fileSystemFixture)
    {
        _fileSystemFixture = fileSystemFixture;
        _fileSystemFixture.SetSubFolder("Project");
    }

    [Fact]
    public async Task GetProjectInfo_GetsProjectInfo()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Project.json");

        var expected = _fileSystemFixture.GetExpectedResponse<Project>("Project.json");

        var response = await client.GetProjectInformationAsync();

        response.Should().BeEquivalentTo(expected);
    }
}
