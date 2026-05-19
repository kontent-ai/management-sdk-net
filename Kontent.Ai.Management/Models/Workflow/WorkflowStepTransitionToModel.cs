using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the workflow step's 'transition to' response model.
/// </summary>
public sealed record WorkflowStepTransitionToModel
{
    /// <summary>
    /// Gets the workflow step's internal ID.
    /// </summary>
    [JsonProperty("step")]
    public Reference Step { get; init; }
}
