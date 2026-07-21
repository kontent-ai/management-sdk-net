namespace Kontent.Ai.Management.Models.Subscription;

/// <summary>
/// A subscription user's membership in a single environment.
/// </summary>
public sealed record SubscriptionUserProjectEnvironmentModel
{
    /// <summary>
    /// Environment ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Environment display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Whether the user is active in this environment.
    /// </summary>
    [JsonPropertyName("is_user_active")]
    public required bool IsUserActive { get; init; }

    /// <summary>
    /// Timestamp of the user's last activity in the environment. Null when the user has never been active.
    /// </summary>
    [JsonPropertyName("last_activity_at")]
    public DateTime? LastActivityAt { get; init; }

    /// <summary>
    /// The user's collection-to-roles assignments in this environment.
    /// </summary>
    [JsonPropertyName("collection_groups")]
    public required IReadOnlyList<SubscriptionCollectionGroupModel> CollectionGroups { get; init; }
}
