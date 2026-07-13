namespace Kontent.Ai.Management.Models.PreviewConfiguration;

/// <summary>
/// The environment's preview configuration. Used as both the GET response and the PUT (full-replace) request body.
/// </summary>
public sealed record PreviewConfigurationModel
{
    /// <summary>
    /// Space-to-domain mappings. Always present; may be empty.
    /// </summary>
    [JsonPropertyName("space_domains")]
    public required IReadOnlyList<SpaceDomainModel> SpaceDomains { get; init; }

    /// <summary>
    /// Preview URL patterns per content type. Always present; may be empty.
    /// </summary>
    [JsonPropertyName("preview_url_patterns")]
    public required IReadOnlyList<TypePreviewUrlPatternModel> PreviewUrlPatterns { get; init; }
}
