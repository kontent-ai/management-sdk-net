namespace Kontent.Ai.Management.Models.Users;

/// <summary>
/// Payload for inviting a user into an environment.
/// </summary>
public sealed record UserInviteModel
{
    /// <summary>
    /// Email address of the user to invite.
    /// </summary>
    [JsonPropertyName("email")]
    public required string Email { get; init; }

    /// <summary>
    /// Collection-to-roles assignments for the invited user. Must contain at least one group.
    /// </summary>
    [JsonPropertyName("collection_groups")]
    public required IReadOnlyList<UserCollectionGroup> CollectionGroups { get; init; }
}
