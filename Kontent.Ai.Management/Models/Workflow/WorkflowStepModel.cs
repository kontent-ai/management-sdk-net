using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the custom workflow step response model.
/// </summary>
public sealed record WorkflowStepModel
{
    /// <summary>
    /// Gets the workflow step's internal ID.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the workflow step's name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the workflow step's codename.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the workflow step's color.
    /// </summary>
    [JsonProperty("color")]
    public WorkflowStepColorModel Color { get; init; }

    /// <summary>
    /// Gets the workflow steps that this step can transition to.
    /// </summary>
    [JsonProperty("transitions_to")]
    public IReadOnlyList<WorkflowStepTransitionToModel> TransitionsTo { get; init; }

    /// <summary>
    /// Gets the roles which can work with an item in this step.
    /// </summary>
    [JsonProperty("role_ids")]
    public IReadOnlyCollection<Guid> RoleIds { get; init; }
}
