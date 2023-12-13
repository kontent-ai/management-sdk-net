using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.ContentItem;

/// <summary>
/// Specifies a workflow and its workflow step.
/// </summary>
public class ContentItemWorkflowTransition
{
    /// <summary>
    /// Reference to the content item variant's workflow.
    /// </summary>
    [JsonProperty("workflow_identifier", Required = Required.Always)]
    public Reference WorkflowReference { get; set; }
    
    /// <summary>
    /// Reference to the content item variant's workflow step.
    /// </summary>
    [JsonProperty("step_identifier", Required = Required.Always)]
    public Reference WorkflowStepReference { get; set; }
}