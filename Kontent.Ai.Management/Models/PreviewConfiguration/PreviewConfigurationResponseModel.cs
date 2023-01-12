using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.PreviewConfiguration;

/// <summary>
/// Represents the preview configuration response model.
/// </summary>
public class PreviewConfigurationResponseModel
{
    /// <summary>
    /// Gets or sets space domains.
    /// </summary>
    [JsonProperty("space_domains")]
    public IReadOnlyCollection<SpaceDomainResponseModel> SpaceDomains { get; set; }

    /// <summary>
    /// Gets or sets preview URL patterns.
    /// </summary>
    [JsonProperty("preview_url_patterns")]
    public IReadOnlyCollection<TypePreviewUrlPatternResponseModel> PreviewUrlPatterns { get; set; }
}