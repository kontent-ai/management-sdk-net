namespace Kontent.Ai.Management.Models.Webhooks.Triggers.ContentItem;

/// <summary>
/// A workflow/step pair that fires a content-item workflow-step-changed webhook.
/// </summary>
public sealed record ContentItemWorkflowTransition
{
    /// <summary>
    /// Reference to the workflow.
    /// </summary>
    [JsonPropertyName("workflow_identifier")]
    public required Reference WorkflowReference { get; init; }

    /// <summary>
    /// Reference to the workflow step.
    /// </summary>
    [JsonPropertyName("step_identifier")]
    public required Reference WorkflowStepReference { get; init; }
}
