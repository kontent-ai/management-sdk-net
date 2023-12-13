using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.ContentType;

/// <summary>
/// Represents content type action.
/// </summary>
public class ContentTypeActionModel
{
    /// <summary>
    /// Content type action.
    /// </summary>
    [JsonProperty("action")]
    public ContentTypeActionEnum Action { get; set; }
}