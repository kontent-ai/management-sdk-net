using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the workflow response model.
/// </summary>
public sealed record WorkflowModel
{
    /// <summary>
    /// Gets the workflow's internal ID.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the workflow's name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the workflow's codename.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the workflow's scopes.
    /// </summary>
    [JsonProperty("scopes")]
    public IReadOnlyList<WorkflowScopeModel> Scopes { get; init; }

    /// <summary>
    /// Gets the workflow's steps.
    /// </summary>
    [JsonProperty("steps")]
    public IReadOnlyList<WorkflowStepModel> Steps { get; init; }

    /// <summary>
    /// Gets the workflow's Published step.
    /// </summary>
    [JsonProperty("published_step")]
    public WorkflowPublishedStepModel PublishedStep { get; init; }

    /// <summary>
    /// Gets the workflow's Scheduled step.
    /// </summary>
    [JsonProperty("scheduled_step")]
    public WorkflowScheduledStepModel ScheduledStep { get; init; }

    /// <summary>
    /// Gets the workflow's Archived step.
    /// </summary>
    [JsonProperty("archived_step")]
    public WorkflowArchivedStepModel ArchivedStep { get; init; }
}