namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// The Archived system step on a workflow (response shape). Variants in this step are read-only and outside the publishing lifecycle.
/// </summary>
/// <remarks>
/// <see cref="Id"/>, <see cref="Name"/>, and <see cref="Codename"/> are platform-defined and cannot be changed.
/// </remarks>
public sealed record WorkflowArchivedStepModel
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
    /// Roles allowed to work with archived variants. May be empty.
    /// </summary>
    [JsonPropertyName("role_ids")]
    public required IReadOnlyCollection<Guid> RoleIds { get; init; }
}
