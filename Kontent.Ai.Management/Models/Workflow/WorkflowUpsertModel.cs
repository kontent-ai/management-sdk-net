using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the workflow upsert model.
/// </summary>
public sealed record WorkflowUpsertModel
{
    /// <summary>
    /// Gets the workflow's name.
    /// </summary>
    [JsonProperty("name", Required = Required.Always)]
    public string Name { get; init; }

    /// <summary>
    /// Gets the workflow's codename.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the workflow's scopes.
    /// </summary>
    [JsonProperty("scopes", Required = Required.Always)]
    public IReadOnlyList<WorkflowScopeUpsertModel> Scopes { get; init; }

    /// <summary>
    /// Gets the workflow's steps.
    /// </summary>
    [JsonProperty("steps", Required = Required.Always)]
    public IReadOnlyList<WorkflowStepUpsertModel> Steps { get; init; }

    /// <summary>
    /// Gets the workflow's Published step.
    /// </summary>
    [JsonProperty("published_step", Required = Required.Always)]
    public WorkflowPublishedStepUpsertModel PublishedStep { get; init; }

    /// <summary>
    /// Gets the workflow's Archived step.
    /// </summary>
    [JsonProperty("archived_step", Required = Required.Always)]
    public WorkflowArchivedStepUpsertModel ArchivedStep { get; init; }
}
