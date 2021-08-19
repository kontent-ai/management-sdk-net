using Kentico.Kontent.Management.Models.ProjectReport;
using System.Threading.Tasks;
using Xunit;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    partial class ManagementClientTests
    {
        [Fact]
        [Trait("Category", "Project")]
        public async Task GetProjectInfo_GetsProjectInfo()
        {
            var expected = new Project
            {
                Environment = "Production",
                Id = "a9931a80-9af4-010b-0590-ecb1273cf1b8",
                Name = ".NET MAPI V2 SDK Tests",
            };

            var client = CreateManagementClient();
            var response = await client.GetProjectInformation();

            Assert.Equal(expected.Id, response.Id);
            Assert.Equal(expected.Name, response.Name);
            Assert.Equal(expected.Environment, response.Environment);
        }
    }
}
