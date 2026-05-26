using AwesomeAssertions;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using System.Text.Json;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class EnvironmentInformationTests
{
    private static string Project => Fixture("Project.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "ProjectInformation", name));

    [Fact]
    public async Task GetEnvironmentInformationAsync_GetsEnvironmentInformationAsync()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}")
            .Respond("application/json", Project);

        var result = await client.GetEnvironmentInformationAsync();

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<Models.EnvironmentReport.Environment>(Project, SharedTestJsonOptions.Default));
    }
}
