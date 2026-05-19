using Kontent.Ai.Management.Models.Shared;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Languages;

/// <summary>
/// Represents the language create model.
/// </summary>
public sealed record LanguageCreateModel
{
    /// <summary>
    /// Gets the language's display name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the language's codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the language's external id.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string ExternalId { get; init; }

    /// <summary>
    /// Gets a flag determining whether the language is active.
    /// </summary>
    [JsonPropertyName("is_active")]
    public bool IsActive { get; init; }

    /// <summary>
    /// Gets the language to use when the current language contains no content. With multiple languages you can create fallback chains.
    /// </summary>
    [JsonPropertyName("fallback_language")]
    public Reference FallbackLanguage { get; init; }
}
