using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.PreviewConfiguration;

/// <summary>
/// Represents preview URL pattern response model.
/// </summary>
public class PreviewUrlPatternResponseModel
{
    /// <summary>
    /// Gets or sets the space reference.
    /// </summary>
    [JsonProperty("space")]
    public Reference Space { get; set; }

    /// <summary>
    /// Gets or sets the content type URL pattern.
    /// </summary>
    [JsonProperty("url_pattern")]
    public string UrlPattern { get; set; }

    /// <summary>
    /// Gets or sets if the URL pattern should be enabled.
    /// </summary>
    [JsonProperty("enabled")]
    public bool Enabled { get; set; }
}