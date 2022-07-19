using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the workflow step's 'transition to' upsert model.
/// </summary>
public class WorkflowStepTransitionToUpsertModel
{
    /// <summary>
    /// Gets or sets the workflow step's internal ID.
    /// </summary>
    [JsonProperty("step")]
    public Reference Step { get; set; }
}
