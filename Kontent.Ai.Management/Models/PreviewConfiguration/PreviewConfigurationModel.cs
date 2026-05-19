using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.PreviewConfiguration;

/// <summary>
/// Represents the preview configuration model.
/// </summary>
public sealed record PreviewConfigurationModel
{
    /// <summary>
    /// Gets space domains.
    /// </summary>
    [JsonPropertyName("space_domains")]
    public IReadOnlyCollection<SpaceDomainModel> SpaceDomains { get; init; }

    /// <summary>
    /// Gets preview URL patterns.
    /// </summary>
    [JsonPropertyName("preview_url_patterns")]
    public IReadOnlyCollection<TypePreviewUrlPatternModel> PreviewUrlPatterns { get; init; }
}