using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Users;

/// <summary>
/// Represents user's colection group.
/// </summary>
public sealed record UserCollectionGroup
{
    /// <summary>
    /// Gets user's collection.
    /// </summary>
    [JsonPropertyName("collections")]
    public IEnumerable<Reference> Collections { get; init; }

    /// <summary>
    /// Gets user's roles.
    /// </summary>
    [JsonPropertyName("roles")]
    public IEnumerable<RoleModel> Roles { get; init; }
}
