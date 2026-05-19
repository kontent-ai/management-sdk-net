using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Subscription;

/// <summary>
/// Represents subscription user object.
/// </summary>
public sealed record SubscriptionUserModel
{
    /// <summary>
    /// Gets the user's internal ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; }

    /// <summary>
    /// Gets the user's first name.
    /// </summary>
    [JsonPropertyName("first_name")]
    public string FirstName { get; init; }

    /// <summary>
    /// Gets the user's last name.
    /// </summary>
    [JsonPropertyName("last_name")]
    public string LastName { get; init; }

    /// <summary>
    /// Gets the user's email address.
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; init; }

    /// <summary>
    /// Gets a flag determining whether the user has any pending invitation to a project.
    /// </summary>
    [JsonPropertyName("has_pending_invitation")]
    public bool HasPendingInvitation { get; init; }

    /// <summary>
    /// Gets the projects to which the user has been invited.
    /// </summary>
    [JsonPropertyName("projects")]
    public IEnumerable<SubscriptionUserProjectModel> Projects { get; init; }
}
