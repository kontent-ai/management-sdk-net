using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Subscription;

/// <summary>
/// Represents projects to which the user has been invited.
/// </summary>
public sealed record SubscriptionUserProjectEnvironmentModel
{
    /// <summary>
    /// Gets he environment's internal ID.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the environment's display name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets a flag determining whether the user is active in this environment.
    /// </summary>
    [JsonPropertyName("is_user_active")]
    public bool IsUserActive { get; init; }

    /// <summary>
    /// Gets the timestamp of the last user's activity in the environment.
    /// </summary>
    [JsonPropertyName("last_activity_at")]
    public DateTime LastActivityAt { get; init; }

    /// <summary>
    /// Gets collections user is assigned to with a set of roles.
    /// </summary>
    [JsonPropertyName("collection_groups")]
    public IEnumerable<SubscriptionColletionGroupModel> CollectionGroups { get; init; }
}
