using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Subscription;

/// <summary>
/// Represents the projects to which the user has been invited.
/// </summary>
public sealed record SubscriptionUserProjectModel
{
    /// <summary>
    /// Gets the project's internal ID.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the project's name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the project's environments.
    /// </summary>
    [JsonPropertyName("environments")]
    public IEnumerable<SubscriptionUserProjectEnvironmentModel> Environments { get; init; }
}
