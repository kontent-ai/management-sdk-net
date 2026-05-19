using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Kontent.Ai.Management.Models.Items;

/// <summary>
/// Represents content item upsert model.
/// </summary>
public sealed record ContentItemUpsertModel
{
    /// <summary>
    /// Gets the name of the content item.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the codename of the content item.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the type of the content item.
    /// Type is taken into account only when creating a new content item.
    /// Type is ignored in case of update.
    /// </summary>
    [JsonProperty("type")]
    public Reference Type { get; init; }

    /// <summary>
    /// Gets sitemap locations of the content item.
    /// </summary>
    [JsonProperty("sitemap_locations")]
    public IEnumerable<Reference> SitemapLocations { get; init; } = Enumerable.Empty<Reference>();

    /// <summary>
    /// Gets the collection of the content item.
    /// </summary>
    [JsonProperty("collection")]
    public Reference Collection { get; init; }

    /// <summary>
    /// Gets the external identifier of the content item.
    /// ExternalId is taken into account only when creating a new content item.
    /// ExternalId is ignored in case of update.
    /// </summary>
    [JsonProperty("external_id")]
    public string ExternalId { get; init; }
}
