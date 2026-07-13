namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Payload for the Archived system step inside a workflow upsert.
/// </summary>
public sealed record WorkflowArchivedStepUpsertModel
{
    /// <summary>
    /// Roles allowed to work with archived variants. Defaults to empty — no role restriction.
    /// </summary>
    [JsonPropertyName("role_ids")]
    public IEnumerable<Guid> RoleIds { get; init; } = [];
}
