using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Subscription;

/// <summary>
/// Represents the projects to which the user has been invited.
/// </summary>
public sealed record SubscriptionUserProjectModel
{
    /// <summary>
    /// Gets the project's internal ID.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the project's name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the project's environments.
    /// </summary>
    [JsonProperty("environments")]
    public IEnumerable<SubscriptionUserProjectEnvironmentModel> Environments { get; init; }
}
