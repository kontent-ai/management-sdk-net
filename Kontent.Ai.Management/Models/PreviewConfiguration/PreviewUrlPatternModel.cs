using Kontent.Ai.Management.Models.Shared;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.PreviewConfiguration;

/// <summary>
/// Represents preview URL pattern model.
/// </summary>
public sealed record PreviewUrlPatternModel
{
    /// <summary>
    /// Gets the space reference.
    /// </summary>
    [JsonPropertyName("space")]
    public Reference Space { get; init; }

    /// <summary>
    /// Gets the content type URL pattern.
    /// </summary>
    [JsonPropertyName("url_pattern")]
    public string UrlPattern { get; init; }
}