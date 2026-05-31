namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Payload for the Published system step inside a workflow upsert.
/// </summary>
public sealed record WorkflowPublishedStepUpsertModel
{
    /// <summary>
    /// Roles allowed to create a new version from a published variant. May be empty.
    /// </summary>
    [JsonPropertyName("create_new_version_role_ids")]
    public required IReadOnlyCollection<Guid> CreateNewVersionRoleIds { get; init; }

    /// <summary>
    /// Roles allowed to unpublish variants in this step. May be empty.
    /// </summary>
    [JsonPropertyName("unpublish_role_ids")]
    public required IReadOnlyCollection<Guid> UnpublishRoleIds { get; init; }
}
