using Kontent.Ai.Management.Tests.Base;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class ProjectInformationTests
{
    private readonly Scenario _scenario;

    public ProjectInformationTests()
    {
        _scenario = new Scenario(folder: "ProjectInformation");
    }

    [Fact]
    public async Task GetProjectInformationAsync_GetsProjectInformationAsync()
    {
        var client = _scenario
            .WithResponses("Project.json")
            .CreateManagementClient();

        var response = await client.GetProjectInformationAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}")
            .Validate();
    }
}
