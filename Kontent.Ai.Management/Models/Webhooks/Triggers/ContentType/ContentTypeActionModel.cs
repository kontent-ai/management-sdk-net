using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.ContentType;

/// <summary>
/// Represents content type action.
/// </summary>
public class ContentTypeActionModel
{
    /// <summary>
    /// Content type action.
    /// </summary>
    [JsonPropertyName("action")]
    public ContentTypeAction Action { get; set; }
}