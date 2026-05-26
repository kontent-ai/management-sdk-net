namespace Kontent.Ai.Management.Models.Users;

/// <summary>
/// Represents the project user model.
/// </summary>
public sealed record UserModel
{
    /// <summary>
    /// Gets the user's ID.
    /// </summary>
    [JsonPropertyName("user_id")]
    public string Id { get; init; }

    /// <summary>
    /// Gets the language's display name.
    /// </summary>
    [JsonPropertyName("collection_groups")]
    public IEnumerable<UserCollectionGroup> CollectionGroup { get; init; }
}
