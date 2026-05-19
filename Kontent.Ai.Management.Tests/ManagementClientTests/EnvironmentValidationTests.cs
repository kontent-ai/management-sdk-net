using AwesomeAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.EnvironmentReport;
using Kontent.Ai.Management.Models.EnvironmentValidation;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using Xunit;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class EnvironmentValidationTests
{
    private static string ProjectValidation => Fixture("ProjectValidation.json");
    private static string AsyncValidationTask => Fixture("AsyncValidationTask.json");
    private static string AsyncValidationTaskIssues => Fixture("AsyncValidationTaskIssues.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, "Data", "ProjectValidation", name));

    [Fact]
    public async Task ValidateEnvironment_ReturnsEnvironmentReportModel()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/validate")
            .Respond("application/json", ProjectValidation);

        var response = await client.ValidateEnvironmentAsync();

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonSerializer.Deserialize<EnvironmentReportModel>(ProjectValidation, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task InitiateEnvironmentAsyncValidationTaskAsync_ReturnsAsyncValidationTask()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/validate-async")
            .Respond("application/json", AsyncValidationTask);

        var response = await client.InitiateEnvironmentAsyncValidationTaskAsync();

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonSerializer.Deserialize<AsyncValidationTaskModel>(AsyncValidationTask, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task GetAsyncValidationTaskAsync_ReturnsAsyncValidationTask()
    {
        var (client, mock) = MockClientFactory.Create();
        var taskIdentifier = Guid.Empty;
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/validate-async/tasks/{taskIdentifier}")
            .Respond("application/json", AsyncValidationTask);

        var response = await client.GetAsyncValidationTaskAsync(taskIdentifier);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonSerializer.Deserialize<AsyncValidationTaskModel>(AsyncValidationTask, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task GetAsyncValidationTaskIssuesAsync_ReturnsAsyncValidationTask()
    {
        var (client, mock) = MockClientFactory.Create();
        var taskIdentifier = Guid.Empty;
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/validate-async/tasks/{taskIdentifier}/issues")
            .Respond("application/json", AsyncValidationTaskIssues);

        var response = await client.ListAsyncValidationTaskIssuesAsync(taskIdentifier).GetAllAsync();

        mock.VerifyNoOutstandingExpectation();
        var items = JsonNode.Parse(AsyncValidationTaskIssues)!.AsObject().First().Value!.ToString();
        response.Should().BeEquivalentTo(JsonSerializer.Deserialize<List<AsyncValidationTaskIssueModel>>(items, SharedTestJsonOptions.Default));
    }
}
