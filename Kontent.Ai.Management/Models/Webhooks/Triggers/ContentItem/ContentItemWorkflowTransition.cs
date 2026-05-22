
namespace Kontent.Ai.Management.Models.Webhooks.Triggers.ContentItem;

/// <summary>
/// Specifies a workflow and its workflow step.
/// </summary>
public class ContentItemWorkflowTransition
{
    /// <summary>
    /// Reference to the content item variant's workflow.
    /// </summary>
    [JsonPropertyName("workflow_identifier")]
    public Reference WorkflowReference { get; set; }
    
    /// <summary>
    /// Reference to the content item variant's workflow step.
    /// </summary>
    [JsonPropertyName("step_identifier")]
    public Reference WorkflowStepReference { get; set; }
}