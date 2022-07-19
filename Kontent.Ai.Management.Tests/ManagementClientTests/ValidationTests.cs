using FluentAssertions;
using Kontent.Ai.Management.Models.ProjectReport;
using Kontent.Ai.Management.Tests.Base;
using Xunit;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class ValidationTests : IClassFixture<FileSystemFixture>
{
    private readonly FileSystemFixture _fileSystemFixture;

    public ValidationTests(FileSystemFixture fileSystemFixture)
    {
        _fileSystemFixture = fileSystemFixture;
        _fileSystemFixture.SetSubFolder("Validation");
    }

    [Fact]
    public async void ValidateProject_ReturnsProjectReportModel()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("ProjectValidation.json");

        var expected = _fileSystemFixture.GetExpectedResponse<ProjectReportModel>("ProjectValidation.json");

        var response = await client.ValidateProjectAsync();

        response.Should().BeEquivalentTo(expected);
    }
}
