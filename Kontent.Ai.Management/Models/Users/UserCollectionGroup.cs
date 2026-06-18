namespace Kontent.Ai.Management.Models.Users;

/// <summary>
/// Pairs a set of collections with the roles a user holds in them.
/// </summary>
public sealed record UserCollectionGroup
{
    /// <summary>
    /// Collections this group applies to. An empty array assigns the user to all collections.
    /// </summary>
    [JsonPropertyName("collections")]
    public required IReadOnlyList<Reference> Collections { get; init; }

    /// <summary>
    /// Roles the user holds in these collections.
    /// </summary>
    [JsonPropertyName("roles")]
    public required IReadOnlyList<RoleModel> Roles { get; init; }
}
