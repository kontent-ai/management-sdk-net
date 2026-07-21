namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// A custom workflow step (response shape).
/// </summary>
public sealed record WorkflowStepModel
{
    /// <summary>
    /// Server-generated step ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public required string Codename { get; init; }

    /// <summary>
    /// UI color shown for this step.
    /// </summary>
    [JsonPropertyName("color")]
    public required WorkflowStepColor Color { get; init; }

    /// <summary>
    /// Workflow steps that this step can transition to.
    /// </summary>
    [JsonPropertyName("transitions_to")]
    public required IReadOnlyList<WorkflowStepTransitionToModel> TransitionsTo { get; init; }

    /// <summary>
    /// Roles allowed to work with items in this step. May be empty.
    /// </summary>
    [JsonPropertyName("role_ids")]
    public required IReadOnlyList<Guid> RoleIds { get; init; }
}
