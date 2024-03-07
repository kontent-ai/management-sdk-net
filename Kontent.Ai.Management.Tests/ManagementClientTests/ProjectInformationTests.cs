using Kontent.Ai.Management.Tests.Base;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class EnvironmentInformationTests
{
    private readonly Scenario _scenario;

    public EnvironmentInformationTests()
    {
        _scenario = new Scenario(folder: "ProjectInformation");
    }

    [Fact]
    public async Task GetEnvironmentInformationAsync_GetsEnvironmentInformationAsync()
    {
        var client = _scenario
            .WithResponses("Project.json")
            .CreateManagementClient();

        var response = await client.GetEnvironmentInformationAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}")
            .Validate();
    }
}
