using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.Subscription;

/// <summary>
/// Represents project's environment.
/// </summary>
public sealed class SubscriptionProjectEnvironmentModel
{
    /// <summary>
    /// Gets or sets the environment's internal ID.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the environment's name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }
}
