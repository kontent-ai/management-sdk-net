using FluentAssertions;
using Kontent.Ai.Management.Tests.Base;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Xunit;

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

        var response = await client.GetEnvironmentInformationAsync();

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<Models.EnvironmentReport.Environment>(Project));
    }
}
