namespace Kontent.Ai.Management.Models.Webhooks.Triggers.ContentType;

/// <summary>
/// Represents the content type filters model.
/// </summary>
public class ContentTypeFiltersModel
{
    /// <summary>
    /// References to content types
    /// </summary>
    [JsonPropertyName("content_types")]
    public IEnumerable<Reference> ContentTypes { get; set; }
}