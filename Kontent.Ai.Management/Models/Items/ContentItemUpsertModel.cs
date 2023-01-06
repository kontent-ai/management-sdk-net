using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Kontent.Ai.Management.Models.Items;

/// <summary>
/// Represents content item upsert model.
/// </summary>
public sealed class ContentItemUpsertModel
{
    /// <summary>
    /// Gets or sets the name of the content item.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the codename of the content item.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; set; }

    /// <summary>
    /// Gets or sets the type of the content item.
    /// Type is taken into account only when creating a new content item.
    /// Type is ignored in case of update.
    /// </summary>
    [JsonProperty("type")]
    public Reference Type { get; set; }

    /// <summary>
    /// Gets or sets sitemap locations of the content item.
    /// </summary>
    [JsonProperty("sitemap_locations")]
    public IEnumerable<Reference> SitemapLocations { get; set; } = Enumerable.Empty<Reference>();

    /// <summary>
    /// Gets or sets the collection of the content item.
    /// </summary>
    [JsonProperty("collection")]
    public Reference Collection { get; set; }

    /// <summary>
    /// Gets or sets the external identifier of the content item.
    /// ExternalId is taken into account only when creating a new content item.
    /// ExternalId is ignored in case of update.
    /// </summary>
    [JsonProperty("external_id")]
    public string ExternalId { get; set; }
}
