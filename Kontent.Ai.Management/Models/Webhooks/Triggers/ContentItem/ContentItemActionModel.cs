using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.ContentItem;

/// <summary>
/// Represents content item action.
/// </summary>
public class ContentItemActionModel
{
    /// <summary>
    /// The action performed on a content item.
    /// </summary>
    [JsonProperty("action")]
    public ContentItemActionEnum Action { get; set; }
    
    /// <summary>
    /// Specifies a workflow and its workflow step. 
    /// </summary>
    [JsonProperty("transition_to")]
    public IEnumerable<ContentItemWorkflowTransition> TransitionTo { get; set; }
}