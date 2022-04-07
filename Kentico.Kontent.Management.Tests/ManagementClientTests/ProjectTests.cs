using FluentAssertions;
using Kentico.Kontent.Management.Models.ProjectReport;
using Kentico.Kontent.Management.Tests.Base;
using System.Threading.Tasks;
using Xunit;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests;

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
