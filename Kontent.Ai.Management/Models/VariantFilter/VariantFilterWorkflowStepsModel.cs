namespace Kontent.Ai.Management.Models.VariantFilter;

/// <summary>
/// Workflow-based filter: restrict results to variants currently in specific steps of a workflow.
/// </summary>
public sealed record VariantFilterWorkflowStepsModel
{
    /// <summary>
    /// Reference to the workflow.
    /// </summary>
    [JsonPropertyName("workflow_identifier")]
    public required Reference WorkflowReference { get; init; }

    /// <summary>
    /// Specific steps within the workflow to match. Omit to match any step.
    /// </summary>
    [JsonPropertyName("step_identifiers")]
    public IReadOnlyList<Reference>? WorkflowStepReferences { get; init; }
}