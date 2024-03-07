using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Subscription;

/// <summary>
/// Represents the subscription project object.
/// </summary>
public sealed class SubscriptionProjectModel
{
    /// <summary>
    /// Gets or sets the id of the project.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the project's name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a flag determining whether the project is active.
    /// </summary>
    [JsonProperty("is_active")]
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the project's environments.
    /// </summary>
    [JsonProperty("environments")]
    public IEnumerable<SubscriptionProjectEnvironmentModel> Environments { get; set; }
}
