using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Subscription;

/// <summary>
/// Represents the subscription project object.
/// </summary>
public sealed record SubscriptionProjectModel
{
    /// <summary>
    /// Gets the id of the project.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the project's name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets a flag determining whether the project is active.
    /// </summary>
    [JsonPropertyName("is_active")]
    public bool IsActive { get; init; }

    /// <summary>
    /// Gets the project's environments.
    /// </summary>
    [JsonPropertyName("environments")]
    public IEnumerable<SubscriptionProjectEnvironmentModel> Environments { get; init; }
}
