using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.Languages;

/// <summary>
/// Represents the language model.
/// </summary>
public sealed record LanguageModel
{
    /// <summary>
    /// Gets the language's internal ID.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the language's display name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the language's codename.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the language's external id.
    /// </summary>
    [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string ExternalId { get; init; }

    /// <summary>
    /// Gets a flag determining whether the language is active.
    /// </summary>
    [JsonProperty("is_active")]
    public bool IsActive { get; init; }

    /// <summary>
    /// Gets a flag determining whether the language is the default language.
    /// </summary>
    [JsonProperty("is_default")]
    public bool IsDefault { get; init; }

    /// <summary>
    /// Gets the language to use when the current language contains no content. With multiple languages you can create fallback chains.
    /// </summary>
    [JsonProperty("fallback_language")]
    public Reference FallbackLanguage { get; init; }
}
