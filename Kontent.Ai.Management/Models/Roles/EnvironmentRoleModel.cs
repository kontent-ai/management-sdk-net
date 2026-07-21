namespace Kontent.Ai.Management.Models.Roles;

/// <summary>
/// An environment role (response shape).
/// </summary>
public sealed record EnvironmentRoleModel
{
    /// <summary>
    /// Server-generated role ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Codename. Only present for the built-in Project manager role; null for all other roles.
    /// </summary>
    [JsonPropertyName("codename")]
    public string? Codename { get; init; }
}
