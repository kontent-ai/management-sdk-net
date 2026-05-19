using Kontent.Ai.Management.Models.Shared;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the workflow step's 'transition to' upsert model.
/// </summary>
public sealed record WorkflowStepTransitionToUpsertModel
{
    /// <summary>
    /// Gets the workflow step's internal ID.
    /// </summary>
    [JsonPropertyName("step")]
    public Reference Step { get; init; }
}
