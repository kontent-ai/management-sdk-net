namespace Kontent.Ai.Management.Models.Languages;

/// <summary>
/// A project language (response shape).
/// </summary>
public sealed record LanguageModel
{
    /// <summary>
    /// Server-generated language ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public required string Codename { get; init; }

    /// <summary>
    /// Caller-supplied external ID. Only present when one was specified on create.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string? ExternalId { get; init; }

    /// <summary>
    /// Whether the language is active.
    /// </summary>
    [JsonPropertyName("is_active")]
    public required bool IsActive { get; init; }

    /// <summary>
    /// Whether this is the project's default language. Read-only — set by the platform, cannot be patched.
    /// </summary>
    [JsonPropertyName("is_default")]
    public required bool IsDefault { get; init; }

    /// <summary>
    /// Language to use when this language has no content.
    /// </summary>
    [JsonPropertyName("fallback_language")]
    public required Reference FallbackLanguage { get; init; }
}
