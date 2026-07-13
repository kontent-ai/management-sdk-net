namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Payload for the Published system step inside a workflow upsert.
/// </summary>
public sealed record WorkflowPublishedStepUpsertModel
{
    /// <summary>
    /// Roles allowed to create a new version from a published variant. Defaults to empty — no role restriction.
    /// </summary>
    [JsonPropertyName("create_new_version_role_ids")]
    public IReadOnlyList<Guid> CreateNewVersionRoleIds { get; init; } = [];

    /// <summary>
    /// Roles allowed to unpublish variants in this step. Defaults to empty — no role restriction.
    /// </summary>
    [JsonPropertyName("unpublish_role_ids")]
    public IReadOnlyList<Guid> UnpublishRoleIds { get; init; } = [];
}
