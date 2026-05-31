namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// The Published system step on a workflow (response shape). Variants in this step are read-only and visible via the Delivery APIs.
/// </summary>
/// <remarks>
/// <see cref="Id"/>, <see cref="Name"/>, and <see cref="Codename"/> are platform-defined and cannot be changed.
/// </remarks>
public sealed record WorkflowPublishedStepModel
{
    /// <summary>
    /// Server-generated step ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public required string Codename { get; init; }

    /// <summary>
    /// Roles allowed to unpublish variants in this step. May be empty.
    /// </summary>
    [JsonPropertyName("unpublish_role_ids")]
    public required IReadOnlyCollection<Guid> UnpublishRoleIds { get; init; }

    /// <summary>
    /// Roles allowed to create a new version from a published variant. May be empty.
    /// </summary>
    [JsonPropertyName("create_new_version_role_ids")]
    public required IReadOnlyCollection<Guid> CreateNewVersionRoleIds { get; init; }
}
