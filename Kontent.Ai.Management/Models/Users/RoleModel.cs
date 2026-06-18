namespace Kontent.Ai.Management.Models.Users;

/// <summary>
/// A role assignment within a user's collection group.
/// </summary>
public sealed record RoleModel
{
    /// <summary>
    /// Reference ID of the role.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Languages the role applies to. An empty array assigns the role to all languages.
    /// </summary>
    [JsonPropertyName("languages")]
    public required IReadOnlyList<Reference> Languages { get; init; }
}
