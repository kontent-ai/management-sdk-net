using AwesomeAssertions;
using Kontent.Ai.Management.Models.Workflow;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using System.Text.Json;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class WorkflowTests
{
    private static string Workflow => Fixture("Workflow.json");
    private static string Workflows => Fixture("Workflows.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "Workflow", name));

    // ByExternalId is unsupported for these operations; combined with the null identifier they all fail
    // before any HTTP call, so the loose throw assertion covers both.
    public static TheoryData<Reference?> InvalidIdentifiers =>
    [
        Reference.ByExternalId("externalId"),
        null,
    ];

    [Fact]
    public async Task ListWorkflowsAsync_ListsAllWorkflows()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/workflows")
            .Respond("application/json", Workflows);

        var result = await client.ListWorkflowsAsync();

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<IEnumerable<WorkflowModel>>(Workflows, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task CreateWorkflowAsync_CreatesWorkflow()
    {
        var (client, mock) = MockClientFactory.Create();
        var newWorkflow = GetNewWorkflow();

        string? capturedBody = null;
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/workflows")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", Workflow);

        var result = await client.CreateWorkflowAsync(newWorkflow);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<WorkflowModel>(Workflow, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<WorkflowUpsertModel>(capturedBody!, SharedTestJsonOptions.Default)
            .Should().BeEquivalentTo(JsonSerializer.Deserialize<WorkflowUpsertModel>(JsonSerializer.Serialize(newWorkflow, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task CreateWorkflowAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.CreateWorkflowAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateWorkflowAsync_ById_UpdatesWorkflow()
    {
        var (client, mock) = MockClientFactory.Create();
        var newWorkflow = GetNewWorkflow();
        var identifier = Reference.ById(Guid.NewGuid());

        string? capturedBody = null;
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/workflows/{identifier.Id}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", Workflow);

        var result = await client.UpdateWorkflowAsync(identifier, newWorkflow);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<WorkflowModel>(Workflow, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<WorkflowUpsertModel>(capturedBody!, SharedTestJsonOptions.Default)
            .Should().BeEquivalentTo(JsonSerializer.Deserialize<WorkflowUpsertModel>(JsonSerializer.Serialize(newWorkflow, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task UpdateWorkflowAsync_ByCodename_UpdatesWorkflow()
    {
        var (client, mock) = MockClientFactory.Create();
        var newWorkflow = GetNewWorkflow();
        var identifier = Reference.ByCodename("codename");

        string? capturedBody = null;
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/workflows/codename/{identifier.Codename}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", Workflow);

        var result = await client.UpdateWorkflowAsync(identifier, newWorkflow);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<WorkflowModel>(Workflow, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<WorkflowUpsertModel>(capturedBody!, SharedTestJsonOptions.Default)
            .Should().BeEquivalentTo(JsonSerializer.Deserialize<WorkflowUpsertModel>(JsonSerializer.Serialize(newWorkflow, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default));
    }

    [Theory]
    [MemberData(nameof(InvalidIdentifiers))]
    public async Task UpdateWorkflowAsync_InvalidIdentifier_Throws(Reference? identifier)
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.UpdateWorkflowAsync(identifier!, GetNewWorkflow())).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task UpdateWorkflowAsync_UpsertModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.UpdateWorkflowAsync(Reference.ByCodename("codename"), null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteWorkflowAsync_ById_DeletesWorkflow()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Delete, $"{MockClientFactory.BaseUrl}/workflows/{identifier.Id}")
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.DeleteWorkflowAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteWorkflowAsync_ByCodename_DeletesWorkflow()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByCodename("codename");
        mock.Expect(HttpMethod.Delete, $"{MockClientFactory.BaseUrl}/workflows/codename/{identifier.Codename}")
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.DeleteWorkflowAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(InvalidIdentifiers))]
    public async Task DeleteWorkflowAsync_InvalidIdentifier_Throws(Reference? identifier)
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.DeleteWorkflowAsync(identifier!)).Should().ThrowAsync<Exception>();
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
            CreateNewVersionRoleIds = new List<Guid> { Guid.Parse("b28a237e-e821-4d7d-a5bd-e69e158887d6") },
            UnpublishRoleIds = new List<Guid> { Guid.Parse("b28a237e-e821-4d7d-a5bd-e69e158887d6") }
        },
        ArchivedStep = new WorkflowArchivedStepUpsertModel
        {
            RoleIds = new List<Guid> { Guid.Parse("b28a237e-e821-4d7d-a5bd-e69e158887d6") }
        }
    };
}
