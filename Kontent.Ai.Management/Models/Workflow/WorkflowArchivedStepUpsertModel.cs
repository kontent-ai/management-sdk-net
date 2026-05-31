namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Payload for the Archived system step inside a workflow upsert.
/// </summary>
public sealed record WorkflowArchivedStepUpsertModel
{
    /// <summary>
    /// Roles allowed to work with archived variants. May be empty.
    /// </summary>
    [JsonPropertyName("role_ids")]
    public required IReadOnlyCollection<Guid> RoleIds { get; init; }
}
