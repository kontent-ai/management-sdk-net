using Kentico.Kontent.Management.Models.ProjectReport;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    [Trait("ManagementClient", "Validation")]
    public class ProjectTests
    {
        private readonly IManagementClient _client;
        private readonly Scenario _scenario;

        public ProjectTests(ITestOutputHelper output)
        {
            //this magic can be replace once new xunit is delivered
            //https://github.com/xunit/xunit/issues/621
            var type = output.GetType();
            var testMember = type.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
            var test = (ITest)testMember.GetValue(output);

            _scenario = new Scenario(test.TestCase.TestMethod.Method.Name);
            _client = _scenario.Client;
        }

        [Fact]
        public async Task GetProjectInfo_GetsProjectInfo()
        {
            var expected = new Project
            {
                Environment = "Production",
                Id = "a9931a80-9af4-010b-0590-ecb1273cf1b8",
                Name = ".NET MAPI V2 SDK Tests",
            };

            
            var response = await _client.GetProjectInformation();

            Assert.Equal(expected.Id, response.Id);
            Assert.Equal(expected.Name, response.Name);
            Assert.Equal(expected.Environment, response.Environment);
        }
    }
}
