namespace Kontent.Ai.Management.Models.PreviewConfiguration;

/// <summary>
/// A single preview URL pattern, optionally scoped to a space.
/// </summary>
public sealed record PreviewUrlPatternModel
{
    /// <summary>
    /// Reference to the space this pattern applies to. Null for the default (space-agnostic) pattern.
    /// </summary>
    [JsonPropertyName("space")]
    public Reference? Space { get; init; }

    /// <summary>
    /// The preview URL pattern (may contain placeholders such as <c>{URLSlug}</c> and <c>{Space}</c>).
    /// </summary>
    [JsonPropertyName("url_pattern")]
    public required string UrlPattern { get; init; }
}
