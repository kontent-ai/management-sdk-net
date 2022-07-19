using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the workflow response model.
/// </summary>
public class WorkflowModel
{
    /// <summary>
    /// Gets or sets the workflow's internal ID.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the workflow's name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the workflow's codename.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; set; }

    /// <summary>
    /// Gets or sets the workflow's scopes.
    /// </summary>
    [JsonProperty("scopes")]
    public IReadOnlyList<WorkflowScopeModel> Scopes { get; set; }

    /// <summary>
    /// Gets or sets the workflow's steps.
    /// </summary>
    [JsonProperty("steps")]
    public IReadOnlyList<WorkflowStepModel> Steps { get; set; }

    /// <summary>
    /// Gets or sets the workflow's Published step.
    /// </summary>
    [JsonProperty("published_step")]
    public WorkflowPublishedStepModel PublishedStep { get; set; }

    /// <summary>
    /// Gets or sets the workflow's Archived step.
    /// </summary>
    [JsonProperty("archived_step")]
    public WorkflowArchivedStepModel ArchivedStep { get; set; }
}
