using FluentAssertions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Workflow;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class WorkflowTests
{
    private readonly Scenario _scenario;

    public WorkflowTests()
    {
        _scenario = new Scenario(folder: "Workflow");
    }

    [Fact]
    public async void ListWorkflowsAsync_ListsAllWorkflows()
    {
        var client = _scenario
            .WithResponses("Workflows.json")
            .CreateManagementClient();

        var response = await client.ListWorkflowsAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/workflows")
            .Validate();
    }

    [Fact]
    public async void CreateWorkflowAsync_CreatesWorkflow()
    {
        var client = _scenario
            .WithResponses("Workflow.json")
            .CreateManagementClient();

        var newWorkflow = GetNewWorkflow();

        var response = await client.CreateWorkflowAsync(newWorkflow);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Post)
            .RequestPayload(newWorkflow)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/workflows")
            .Validate();
    }

    [Fact]
    public async void CreateWorkflowAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.CreateWorkflowAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void UpdateWorkflowAsync_ById_UpdatesWorkflow()
    {
        var client = _scenario
            .WithResponses("Workflow.json")
            .CreateManagementClient();

        var newWorkflow = GetNewWorkflow();

        var identifier = Reference.ById(Guid.NewGuid());
        var response = await client.UpdateWorkflowAsync(identifier, newWorkflow);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .RequestPayload(newWorkflow)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/workflows/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async void UpdateWorkflowAsync_ByCodename_UpdatesWorkflow()
    {
        var client = _scenario
            .WithResponses("Workflow.json")
            .CreateManagementClient();

        var newWorkflow = GetNewWorkflow();

        var identifier = Reference.ByCodename("codename");
        var response = await client.UpdateWorkflowAsync(identifier, newWorkflow);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .RequestPayload(newWorkflow)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/workflows/codename/{identifier.Codename}")
            .Validate();
    }

    [Fact]
    public async void UpdateWorkflowAsync_ByExternalId_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.UpdateWorkflowAsync(Reference.ByExternalId("externalId"), GetNewWorkflow())).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void UpdateWorkflowAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.UpdateWorkflowAsync(null, GetNewWorkflow())).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void UpdateWorkflowAsync_UpsertModelIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.UpdateWorkflowAsync(Reference.ByCodename("codename"), null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void DeleteWorkflowAsync_ById_DeletesWorkflow()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = Reference.ById(Guid.NewGuid());
        await client.DeleteWorkflowAsync(identifier);

        _scenario
            .CreateExpectations()
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/workflows/{identifier.Id}")
            .HttpMethod(HttpMethod.Delete)
            .Validate();
    }

    [Fact]
    public async void DeleteWorkflowAsync_ByCodename_DeletesWorkflow()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = Reference.ByCodename("codename");
        await client.DeleteWorkflowAsync(identifier);

        _scenario
            .CreateExpectations()
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/workflows/codename/{identifier.Codename}")
            .HttpMethod(HttpMethod.Delete)
            .Validate();
    }

    [Fact]
    public async void DeleteWorkflowAsync_ByExternalId_DeletesWorkflow()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.DeleteWorkflowAsync(Reference.ByExternalId("externalId"))).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void DeleteWorkflowAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.DeleteWorkflowAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    private static WorkflowUpsertModel GetNewWorkflow() => new()
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
}