using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.Subscription;

/// <summary>
/// Represents set of languages user is assigned to within a role.
/// Empty array represents remaining languages not assigned in any other roles.
/// </summary>
public sealed class SubscriptionUserRoleLangaugeModel
{
    /// <summary>
    /// Gets or sets the id of the language.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the externalId of the language.
    /// </summary>
    [JsonProperty("external_id")]
    public string ExternalId { get; set; }

    /// <summary>
    /// Gets or sets the codename of the language.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; set; }

    /// <summary>
    /// Gets or sets the name of the language.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a flag determining whether the language is active.
    /// </summary>
    [JsonProperty("is_active")]
    public string IsActive { get; set; }
}
