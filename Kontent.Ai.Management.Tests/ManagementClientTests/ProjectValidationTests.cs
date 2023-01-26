using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Net.Http;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class ProjectValidationTests : IClassFixture<FileSystemFixture>
{
    private readonly Scenario _scenario;

    public ProjectValidationTests()
    {
        _scenario = new Scenario(folder: "ProjectValidation");
    }

    [Fact]
    public async void ValidateProject_ReturnsProjectReportModel()
    {
        var client = _scenario
            .WithResponses("ProjectValidation.json")
            .CreateManagementClient();

        var response = await client.ValidateProjectAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Post)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/validate")
            .Validate();
    }

    [Fact]
    public async void InitiateProjectAsyncValidationTaskAsync_ReturnsAsyncValidationTask()
    {
        var client = _scenario
            .WithResponses("AsyncValidationTask.json")
            .CreateManagementClient();

        var response = await client.InitiateProjectAsyncValidationTaskAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Post)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/validate-async")
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
            .Url($"{Endpoint}/projects/{PROJECT_ID}/validate-async/tasks/{taskIdentifier}")
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
            .Url($"{Endpoint}/projects/{PROJECT_ID}/validate-async/tasks/{taskIdentifier}/issues")
            .Validate();
    }
}
