using FluentAssertions;
using Kentico.Kontent.Management.Models.ProjectReport;
using Kentico.Kontent.Management.Tests.Unit.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Unit.ManagementClientTests
{
    public class ProjectTests : IClassFixture<FileSystemFixture>
    {
        private FileSystemFixture _fileSystemFixture;

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
}
