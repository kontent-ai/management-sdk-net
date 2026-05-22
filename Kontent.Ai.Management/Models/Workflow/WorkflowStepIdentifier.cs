
namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the workflow step identifier.
/// </summary>
public sealed record WorkflowStepIdentifier
{
    /// <summary>
    /// Represents the identifier of the workflow.
    /// </summary>
    [JsonPropertyName("workflow_identifier")]
    public Reference Workflow { get; init; }

    /// <summary>
    /// Represents the identifier of the step in the workflow.
    /// </summary>
    [JsonPropertyName("step_identifier")]
    public Reference Step { get; init; }

    /// <summary>
    /// Creates an instance of the workflow step identifier.
    /// </summary>
    /// <param name="workflowIdentifier">The identifier of the workflow.</param>
    /// <param name="stepIdentifier">The identifier of the workflow step.</param>
    public WorkflowStepIdentifier(Reference workflowIdentifier, Reference stepIdentifier)
    {
        Workflow = workflowIdentifier;
        Step = stepIdentifier;
    }
}
