namespace Kontent.Ai.Management.Models.Languages;

/// <summary>
/// Payload for creating a project language.
/// </summary>
public sealed record LanguageCreateModel
{
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
    /// Caller-supplied external ID. Optional.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string? ExternalId { get; init; }

    /// <summary>
    /// Whether the language is active. Defaults to false.
    /// </summary>
    [JsonPropertyName("is_active")]
    public bool IsActive { get; init; }

    /// <summary>
    /// Language to use when this language has no content. Optional — when omitted the API populates it with the project's default language.
    /// </summary>
    [JsonPropertyName("fallback_language")]
    public Reference? FallbackLanguage { get; init; }
}
