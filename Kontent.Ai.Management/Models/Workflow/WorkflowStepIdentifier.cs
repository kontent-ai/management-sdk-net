using System.Diagnostics.CodeAnalysis;

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Wire shape identifying a workflow step on variant operations. Serializes as <c>{ workflow_identifier: ..., step_identifier: ... }</c>.
/// </summary>
public sealed record WorkflowStepIdentifier
{
    /// <summary>
    /// Reference to the workflow.
    /// </summary>
    [JsonPropertyName("workflow_identifier")]
    public required Reference Workflow { get; init; }

    /// <summary>
    /// Reference to the step within the workflow.
    /// </summary>
    [JsonPropertyName("step_identifier")]
    public required Reference Step { get; init; }

    /// <inheritdoc/>
    [SetsRequiredMembers]
    public WorkflowStepIdentifier(Reference workflowIdentifier, Reference stepIdentifier)
    {
        Workflow = workflowIdentifier;
        Step = stepIdentifier;
    }
}
