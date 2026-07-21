namespace Kontent.Ai.Management.Models.Users;

/// <summary>
/// Payload for replacing a user's roles. The user is identified by the request URL, so the body carries only the
/// collection-to-roles assignments.
/// </summary>
public sealed record UserRolesUpdateModel
{
    /// <summary>
    /// The user's collection-to-roles assignments.
    /// </summary>
    [JsonPropertyName("collection_groups")]
    public required IReadOnlyList<UserCollectionGroup> CollectionGroups { get; init; }
}
