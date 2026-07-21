namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// A single allowed transition from a workflow step (upsert shape).
/// </summary>
public sealed record WorkflowStepTransitionToUpsertModel
{
    /// <summary>
    /// Reference to the destination step.
    /// </summary>
    [JsonPropertyName("step")]
    public required Reference Step { get; init; }
}
