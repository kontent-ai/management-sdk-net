using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.ContentItem;

/// <summary>
/// Represents the content item filters model.
/// </summary>
public class ContentItemFiltersModel
{
    /// <summary>
    /// References to collections
    /// </summary>
    [JsonPropertyName("collections")]
    public IEnumerable<Reference> Collections { get; set; }
    
    /// <summary>
    /// References to content types
    /// </summary>
    [JsonPropertyName("content_types")]
    public IEnumerable<Reference> ContentTypes { get; set; }
    
    /// <summary>
    /// References to languages
    /// </summary>
    [JsonPropertyName("languages")]
    public IEnumerable<Reference> Languages { get; set; }
}