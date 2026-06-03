namespace Kontent.Ai.Management.Models.Subscription;

/// <summary>
/// A subscription user (response shape).
/// </summary>
public sealed record SubscriptionUserModel
{
    /// <summary>
    /// User ID. A prefixed string (e.g. <c>usr_...</c>), not a Guid.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    /// <summary>
    /// First name. Null for users with a pending invitation who have not yet registered.
    /// </summary>
    [JsonPropertyName("first_name")]
    public string? FirstName { get; init; }

    /// <summary>
    /// Last name. Null for users with a pending invitation who have not yet registered.
    /// </summary>
    [JsonPropertyName("last_name")]
    public string? LastName { get; init; }

    /// <summary>
    /// Email address.
    /// </summary>
    [JsonPropertyName("email")]
    public required string Email { get; init; }

    /// <summary>
    /// Whether the user has any pending project invitation.
    /// </summary>
    [JsonPropertyName("has_pending_invitation")]
    public required bool HasPendingInvitation { get; init; }

    /// <summary>
    /// Projects the user belongs to or has been invited to.
    /// </summary>
    [JsonPropertyName("projects")]
    public required IEnumerable<SubscriptionUserProjectModel> Projects { get; init; }
}
