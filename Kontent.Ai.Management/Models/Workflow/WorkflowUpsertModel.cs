using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the workflow upsert model.
/// </summary>
public sealed record WorkflowUpsertModel
{
    /// <summary>
    /// Gets the workflow's name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the workflow's codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the workflow's scopes.
    /// </summary>
    [JsonPropertyName("scopes")]
    public IReadOnlyList<WorkflowScopeUpsertModel> Scopes { get; init; }

    /// <summary>
    /// Gets the workflow's steps.
    /// </summary>
    [JsonPropertyName("steps")]
    public IReadOnlyList<WorkflowStepUpsertModel> Steps { get; init; }

    /// <summary>
    /// Gets the workflow's Published step.
    /// </summary>
    [JsonPropertyName("published_step")]
    public WorkflowPublishedStepUpsertModel PublishedStep { get; init; }

    /// <summary>
    /// Gets the workflow's Archived step.
    /// </summary>
    [JsonPropertyName("archived_step")]
    public WorkflowArchivedStepUpsertModel ArchivedStep { get; init; }
}
