namespace Kontent.Ai.Management.Models.Subscription;

/// <summary>
/// A set of collections a subscription user is assigned to, with the roles they hold there.
/// </summary>
public sealed record SubscriptionCollectionGroupModel
{
    /// <summary>
    /// Collections the user is assigned to. An empty array means the user can access any collection.
    /// </summary>
    [JsonPropertyName("collections")]
    public required IEnumerable<Reference> Collections { get; init; }

    /// <summary>
    /// Roles the user holds within these collections.
    /// </summary>
    [JsonPropertyName("roles")]
    public required IEnumerable<SubscriptionUserRoleModel> Roles { get; init; }
}
