using FluentAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.ProjectReport;
using Kontent.Ai.Management.Models.ProjectValidation;
using Kontent.Ai.Management.Tests.Base;
using System.Collections.Generic;
using Xunit;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class ValidationTests : IClassFixture<FileSystemFixture>
{
    private readonly FileSystemFixture _fileSystemFixture;

    public ValidationTests(FileSystemFixture fileSystemFixture)
    {
        _fileSystemFixture = fileSystemFixture;
        _fileSystemFixture.SetSubFolder("Validation");
    }

    [Fact]
    public async void ValidateProject_ReturnsProjectReportModel()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("ProjectValidation.json");

        var expected = _fileSystemFixture.GetExpectedResponse<ProjectReportModel>("ProjectValidation.json");

        var response = await client.ValidateProjectAsync();

        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void InitiateProjectAsyncValidationTaskAsync_ReturnsAsyncValidationTask()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("AsyncValidationTask.json");

        var expected = _fileSystemFixture.GetExpectedResponse<AsyncValidationTaskModel>("AsyncValidationTask.json");

        var response = await client.InitiateProjectAsyncValidationTaskAsync();

        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void GetAsyncValidationTaskAsync_ReturnsAsyncValidationTask()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("AsyncValidationTask.json");

        var expected = _fileSystemFixture.GetExpectedResponse<AsyncValidationTaskModel>("AsyncValidationTask.json");

        var response = await client.GetAsyncValidationTaskAsync(System.Guid.Empty);

        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void GetAsyncValidationTaskIssuesAsync_ReturnsAsyncValidationTask()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("AsyncValidationTaskIssues.json");

        var expected = _fileSystemFixture.GetExpectedResponse<List<AsyncValidationTaskIssueModel>>("ExpectedAsyncValidationTaskIssues.json");

        var response = await client.ListAsyncValidationTaskIssuesAsync(System.Guid.Empty).GetAllAsync();

        response.Should().BeEquivalentTo(expected);
    }
}
