namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// A single allowed transition from a workflow step (response shape).
/// </summary>
public sealed record WorkflowStepTransitionToModel
{
    /// <summary>
    /// Reference to the destination step.
    /// </summary>
    [JsonPropertyName("step")]
    public required Reference Step { get; init; }
}
