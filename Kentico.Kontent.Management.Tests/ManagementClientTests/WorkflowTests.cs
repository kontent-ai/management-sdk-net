using System;
using System.Collections.Generic;
using FluentAssertions;
using Kentico.Kontent.Management.Models.Workflow;
using Kentico.Kontent.Management.Tests.Base;
using Xunit;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests;

public class WorkflowTests : IClassFixture<FileSystemFixture>
{
    private readonly FileSystemFixture _fileSystemFixture;

    public WorkflowTests(FileSystemFixture fileSystemFixture)
    {
        _fileSystemFixture = fileSystemFixture;
        _fileSystemFixture.SetSubFolder("Workflow");
    }

    [Fact]
    public async void ListWorkflows_ListsAllWorkflows()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Workflows.json");
        var response = await client.ListWorkflowsAsync();

        var expected = _fileSystemFixture.GetExpectedResponse<List<WorkflowModel>>("Workflows.json");

        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void DeleteWorkflow_NoIdentifier_ThrowsArgumentNullException()
    {
        var client = _fileSystemFixture.CreateMockClientWithoutResponse();

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await client.DeleteWorkflowAsync(null));
    }
}
