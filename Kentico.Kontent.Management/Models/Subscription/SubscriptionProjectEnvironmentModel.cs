using Newtonsoft.Json;
using System;

namespace Kentico.Kontent.Management.Models.Subscription;

/// <summary>
/// Represents project's environemnt.
/// </summary>
public sealed class SubscriptionProjectEnvironmentModel
{
    /// <summary>
    /// Gets or sets the environment's internal ID.
    /// Use this as the projectId path parameter in project-specific endpoints.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the environment's name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }
}
