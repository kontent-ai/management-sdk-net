using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
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
    [JsonProperty("collections")]
    public IEnumerable<Reference> Collections { get; set; }
    
    /// <summary>
    /// References to content types
    /// </summary>
    [JsonProperty("content_types")]
    public IEnumerable<Reference> ContentTypes { get; set; }
    
    /// <summary>
    /// References to languages
    /// </summary>
    [JsonProperty("languages")]
    public IEnumerable<Reference> Languages { get; set; }
}