using AwesomeAssertions;
using Kontent.Ai.Management.Models.EnvironmentReport;
using Kontent.Ai.Management.Models.EnvironmentValidation;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using System.Text.Json;

using static Kontent.Ai.Management.Tests.Base.PagedFixtures;

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

        var result = await client.ValidateEnvironmentAsync();

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<EnvironmentReportModel>(ProjectValidation, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task InitiateEnvironmentAsyncValidationTaskAsync_ReturnsAsyncValidationTask()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/validate-async")
            .Respond("application/json", AsyncValidationTask);

        var result = await client.InitiateEnvironmentAsyncValidationTaskAsync();

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<AsyncValidationTaskModel>(AsyncValidationTask, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task GetAsyncValidationTaskAsync_ReturnsAsyncValidationTask()
    {
        var (client, mock) = MockClientFactory.Create();
        var taskIdentifier = Guid.Empty;
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/validate-async/tasks/{taskIdentifier}")
            .Respond("application/json", AsyncValidationTask);

        var result = await client.GetAsyncValidationTaskAsync(taskIdentifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<AsyncValidationTaskModel>(AsyncValidationTask, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task ListAsyncValidationTaskIssuesAsync_PagesThroughAllIssues()
    {
        var (client, mock) = MockClientFactory.Create();
        var taskIdentifier = Guid.Empty;
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/validate-async/tasks/{taskIdentifier}/issues")
            .Respond("application/json", AsyncValidationTaskIssues);

        var listResult = await client.ListAsyncValidationTaskIssuesAsync(taskIdentifier);
        listResult.IsSuccess.Should().BeTrue();
        IReadOnlyList<AsyncValidationTaskIssueModel> issues = listResult.Value;

        mock.VerifyNoOutstandingExpectation();
        issues.Should().BeEquivalentTo(ConcatPages<AsyncValidationTaskIssueModel>(AsyncValidationTaskIssues));
    }
}
