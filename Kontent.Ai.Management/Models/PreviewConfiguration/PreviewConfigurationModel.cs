using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.PreviewConfiguration;

/// <summary>
/// Represents the preview configuration model.
/// </summary>
public sealed record PreviewConfigurationModel
{
    /// <summary>
    /// Gets space domains.
    /// </summary>
    [JsonProperty("space_domains")]
    public IReadOnlyCollection<SpaceDomainModel> SpaceDomains { get; init; }

    /// <summary>
    /// Gets preview URL patterns.
    /// </summary>
    [JsonProperty("preview_url_patterns")]
    public IReadOnlyCollection<TypePreviewUrlPatternModel> PreviewUrlPatterns { get; init; }
}