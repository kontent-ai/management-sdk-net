using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the custom workflow step upsert model.
/// </summary>
public sealed record WorkflowStepUpsertModel
{
    /// <summary>
    /// Gets the workflow step's identifier.
    /// </summary>
    /// <remarks>
    /// Not applicable for creating a new workflow because the property is used
    /// to identify already existing steps within the edited workflow
    /// </remarks>
    [JsonPropertyName("id")]
    public Guid? Id { get; init; }

    /// <summary>
    /// Gets the workflow step's name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the workflow step's codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the workflow step's color.
    /// </summary>
    [JsonPropertyName("color")]
    public WorkflowStepColorModel Color { get; init; }

    /// <summary>
    /// Gets the workflow steps that this step can transition to.
    /// </summary>
    [JsonPropertyName("transitions_to")]
    public IReadOnlyList<WorkflowStepTransitionToUpsertModel> TransitionsTo { get; init; }

    /// <summary>
    /// Gets the roles which can work with an item in this step.
    /// </summary>
    [JsonPropertyName("role_ids")]
    public IReadOnlyCollection<Guid> RoleIds { get; init; }
}
