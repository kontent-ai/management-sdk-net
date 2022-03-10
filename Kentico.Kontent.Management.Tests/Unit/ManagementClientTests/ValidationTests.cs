using FluentAssertions;
using Kentico.Kontent.Management.Models.ProjectReport;
using Kentico.Kontent.Management.Tests.Unit.Base;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Unit.ManagementClientTests
{
    public class ValidationTests : IClassFixture<FileSystemFixture>
    {
        private FileSystemFixture _fileSystemFixture;

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
}
