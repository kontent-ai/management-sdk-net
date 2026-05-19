using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.PreviewConfiguration;

/// <summary>
/// Represents preview URL pattern model.
/// </summary>
public sealed record PreviewUrlPatternModel
{
    /// <summary>
    /// Gets the space reference.
    /// </summary>
    [JsonProperty("space")]
    public Reference Space { get; init; }

    /// <summary>
    /// Gets the content type URL pattern.
    /// </summary>
    [JsonProperty("url_pattern")]
    public string UrlPattern { get; init; }
}