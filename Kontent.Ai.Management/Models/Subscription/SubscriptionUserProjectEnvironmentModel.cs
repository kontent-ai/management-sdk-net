using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Subscription;

/// <summary>
/// Represents projects to which the user has been invited.
/// </summary>
public sealed class SubscriptionUserProjectEnvironmentModel
{
    /// <summary>
    /// Gets or sets he environment's internal ID.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the environment's display name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a flag determining whether the user is active in this environment.
    /// </summary>
    [JsonProperty("is_user_active")]
    public bool IsUserActive { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of the last user's activity in the environment.
    /// </summary>
    [JsonProperty("last_activity_at")]
    public DateTime LastActivityAt { get; set; }

    /// <summary>
    /// Gets or sets collections user is assigned to with a set of roles.
    /// </summary>
    [JsonProperty("collection_groups")]
    public IEnumerable<SubscriptionColletionGroupModel> CollectionGroups { get; set; }
}
