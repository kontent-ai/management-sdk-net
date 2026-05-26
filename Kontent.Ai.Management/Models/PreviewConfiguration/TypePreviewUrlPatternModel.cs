namespace Kontent.Ai.Management.Models.PreviewConfiguration;

/// <summary>
/// Represents content type preview URL patterns model.
/// </summary>
public sealed record TypePreviewUrlPatternModel
{
    /// <summary>
    /// Gets the content type reference.
    /// </summary>
    [JsonPropertyName("content_type")]
    public Reference ContentType { get; init; }

    /// <summary>
    /// Gets content type's url patterns.
    /// </summary>
    [JsonPropertyName("url_patterns")]
    public IReadOnlyCollection<PreviewUrlPatternModel> UrlPatterns { get; init; }
}