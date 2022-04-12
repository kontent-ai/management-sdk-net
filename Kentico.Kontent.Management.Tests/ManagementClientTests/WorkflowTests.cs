using System;
using System.Collections.Generic;
using FluentAssertions;
using Kentico.Kontent.Management.Models.Shared;
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
    public async void CreateWorkflow_ReturnsCreatedWorkflow()
    {
        var newWorkflow = new WorkflowUpsertModel
        {
            Name = "Marketing",
            Scopes = new List<WorkflowScopeUpsertModel>
                {
                    new()
                    {
                        ContentTypes = new List<Reference>{ Reference.ById(Guid.Parse("b33a98e8-2d0b-409a-a601-3df59edd82be")) }
                    }
                },
            Steps = new List<WorkflowStepUpsertModel>
                {
                    new()
                    {
                        Name = "Draft",
                        Color = WorkflowStepColorModel.Red,
                        RoleIds = new List<Guid>(),
                        TransitionsTo = new List<WorkflowStepTransitionToUpsertModel>
                        {
                            new()
                            {
                                Step = Reference.ByCodename("published")
                            },
                            new()
                            {
                                Step = Reference.ByCodename("archived")
                            }
                        }
                    }
                },
            PublishedStep = new WorkflowPublishedStepUpsertModel
            {
                RoleCreateNewVersionIds = new List<Guid> { Guid.Parse("b28a237e-e821-4d7d-a5bd-e69e158887d6") },
                RolesUnpublishArchivedCancelSchedulingIds = new List<Guid> { Guid.Parse("b28a237e-e821-4d7d-a5bd-e69e158887d6") }
            },
            ArchivedStep = new WorkflowArchivedStepUpsertModel
            {
                RoleIds = new List<Guid> { Guid.Parse("b28a237e-e821-4d7d-a5bd-e69e158887d6") }
            }
        };

        var client = _fileSystemFixture.CreateMockClientWithResponse("Workflow.json");
        var response = await client.CreateWorkflowAsync(newWorkflow);

        var expected = _fileSystemFixture.GetExpectedResponse<WorkflowModel>("Workflow.json");

        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void UpdateWorkflow_ReturnsUpdatedWorkflow()
    {
        var newWorkflow = new WorkflowUpsertModel
        {
            Name = "Marketing",
            Steps = new List<WorkflowStepUpsertModel>
                {
                    new()
                    {
                        Name = "Draft",
                        Color = WorkflowStepColorModel.Red,
                        RoleIds = new List<Guid>(),
                        TransitionsTo = new List<WorkflowStepTransitionToUpsertModel>
                        {
                            new()
                            {
                                Step = Reference.ByCodename("published")
                            }
                        }
                    }
                },
            PublishedStep = new WorkflowPublishedStepUpsertModel(),
            ArchivedStep = new WorkflowArchivedStepUpsertModel()
        };

        var client = _fileSystemFixture.CreateMockClientWithResponse("Workflow.json");
        var response = await client.UpdateWorkflowAsync(Reference.ByCodename("marketing"), newWorkflow);
        var expected = _fileSystemFixture.GetExpectedResponse<WorkflowModel>("Workflow.json");

        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void DeleteWorkflow_NoIdentifier_ThrowsArgumentNullException()
    {
        var client = _fileSystemFixture.CreateMockClientWithoutResponse();

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await client.DeleteWorkflowAsync(null));
    }
}
