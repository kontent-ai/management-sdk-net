using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.Subscription;

/// <summary>
/// Represents project's environment.
/// </summary>
public sealed record SubscriptionProjectEnvironmentModel
{
    /// <summary>
    /// Gets the environment's internal ID.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the environment's name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; init; }
}
