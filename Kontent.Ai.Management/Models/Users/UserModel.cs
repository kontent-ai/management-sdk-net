namespace Kontent.Ai.Management.Models.Users;

/// <summary>
/// An environment user (response shape).
/// </summary>
public sealed record UserModel
{
    /// <summary>
    /// User ID. A prefixed string (e.g. <c>usr_...</c>), not a Guid.
    /// </summary>
    [JsonPropertyName("user_id")]
    public required string Id { get; init; }

    /// <summary>
    /// The user's collection-to-roles assignments.
    /// </summary>
    [JsonPropertyName("collection_groups")]
    public required IEnumerable<UserCollectionGroup> CollectionGroups { get; init; }
}
