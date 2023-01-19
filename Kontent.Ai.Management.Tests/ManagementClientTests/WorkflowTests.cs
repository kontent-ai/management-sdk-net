using FluentAssertions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Workflow;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Collections.Generic;
using Xunit;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

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
    public async void CreateWorkflow_CreatesWorkflow()
    {
        var newWorkflow = new WorkflowUpsertModel
        {
            Name = "Marketing",
            Scopes = new List<WorkflowScopeUpsertModel>
                {
                    new()
                    {
                        Collections = new List<Reference>{ Reference.ById(Guid.Parse("bbab58d7-5f33-4741-a71d-f5435586519c")) },
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
    public async void UpdateWorkflow_UpdatesWorkflow()
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
