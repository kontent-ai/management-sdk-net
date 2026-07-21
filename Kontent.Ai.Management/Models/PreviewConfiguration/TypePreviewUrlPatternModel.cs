namespace Kontent.Ai.Management.Models.PreviewConfiguration;

/// <summary>
/// Preview URL patterns for a single content type.
/// </summary>
public sealed record TypePreviewUrlPatternModel
{
    /// <summary>
    /// Reference to the content type these patterns apply to.
    /// </summary>
    [JsonPropertyName("content_type")]
    public required Reference ContentType { get; init; }

    /// <summary>
    /// Preview URL patterns for the content type.
    /// </summary>
    [JsonPropertyName("url_patterns")]
    public required IReadOnlyList<PreviewUrlPatternModel> UrlPatterns { get; init; }
}
