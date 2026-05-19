using System;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Subscription;

/// <summary>
/// Represents set of languages user is assigned to within a role.
/// Empty array represents remaining languages not assigned in any other roles.
/// </summary>
public sealed record SubscriptionUserRoleLangaugeModel
{
    /// <summary>
    /// Gets the id of the language.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the externalId of the language.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string ExternalId { get; init; }

    /// <summary>
    /// Gets the codename of the language.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the name of the language.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets a flag determining whether the language is active.
    /// </summary>
    [JsonPropertyName("is_active")]
    public string IsActive { get; init; }
}
