using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Net.Http;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class EnvironmentValidationTests : IClassFixture<FileSystemFixture>
{
    private readonly Scenario _scenario;

    public EnvironmentValidationTests()
    {
        _scenario = new Scenario(folder: "ProjectValidation");
    }

    [Fact]
    public async void ValidateEnvironment_ReturnsEnvironmentReportModel()
    {
        var client = _scenario
            .WithResponses("ProjectValidation.json")
            .CreateManagementClient();

        var response = await client.ValidateEnvironmentAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Post)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/validate")
            .Validate();
    }

    [Fact]
    public async void InitiateEnvironmentAsyncValidationTaskAsync_ReturnsAsyncValidationTask()
    {
        var client = _scenario
            .WithResponses("AsyncValidationTask.json")
            .CreateManagementClient();

        var response = await client.InitiateEnvironmentAsyncValidationTaskAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Post)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/validate-async")
            .Validate();
    }
 
    [Fact]
    public async void GetAsyncValidationTaskAsync_ReturnsAsyncValidationTask()
    {
        var client = _scenario
            .WithResponses("AsyncValidationTask.json")
            .CreateManagementClient();

        var taskIdentifier = Guid.Empty;
        var response = await client.GetAsyncValidationTaskAsync(taskIdentifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/validate-async/tasks/{taskIdentifier}")
            .Validate();
    }
 
    [Fact]
    public async void GetAsyncValidationTaskIssuesAsync_ReturnsAsyncValidationTask()
    {
        var client = _scenario
            .WithResponses("AsyncValidationTaskIssues.json")
            .CreateManagementClient();

        var taskIdentifier = Guid.Empty;
        var response = await client.ListAsyncValidationTaskIssuesAsync(taskIdentifier).GetAllAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .ListingResponse(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/validate-async/tasks/{taskIdentifier}/issues")
            .Validate();
    }
}
