using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.PreviewConfiguration;

/// <summary>
/// Represents the preview configuration request model.
/// </summary>
public class PreviewConfigurationRequestModel
{
    /// <summary>
    /// Gets or sets space domains.
    /// </summary>
    [JsonProperty("space_domains")]
    public IReadOnlyCollection<SpaceDomainRequestModel> SpaceDomains { get; set; }

    /// <summary>
    /// Gets or sets preview URL patterns.
    /// </summary>
    [JsonProperty("preview_url_patterns")]
    public IReadOnlyCollection<TypePreviewUrlPatternRequestModel> PreviewUrlPatterns { get; set; }
}