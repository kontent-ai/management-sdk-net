using Kontent.Ai.Management.Serialization.Converters;

namespace Kontent.Ai.Management.Models.Subscription;

/// <summary>
/// A language a subscription user is assigned to within a role. An empty role-languages array represents the remaining languages not assigned in any other role.
/// </summary>
public sealed record SubscriptionUserRoleLanguageModel
{
    /// <summary>
    /// Language ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Caller-supplied external ID. Only present when one was specified.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string? ExternalId { get; init; }

    /// <summary>
    /// Codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public required string Codename { get; init; }

    /// <summary>
    /// Display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Whether the language is active. The API serializes this as a quoted string; the converter normalizes it to a boolean.
    /// </summary>
    [JsonPropertyName("is_active")]
    [JsonConverter(typeof(StringOrBooleanJsonConverter))]
    public required bool IsActive { get; init; }
}
