using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Items;

/// <summary>
/// Represents content item model.
/// </summary>
public sealed class ContentItemModel
{
    /// <summary>
    /// Gets or sets the id of the content item.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }

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
    /// </summary>
    [JsonProperty("type")]
    public Reference Type { get; set; }

    /// <summary>
    /// Gets or sets the collection of the content item.
    /// </summary>
    [JsonProperty("collection")]
    public Reference Collection { get; set; }

    /// <summary>
    /// Gets or sets the spaces of the content item
    /// </summary>
    [JsonProperty("spaces")]
    public IReadOnlyCollection<Reference> Spaces { get; set; }

    /// <summary>
    /// Gets or sets sitemap locations of the content item.
    /// </summary>
    [JsonProperty("sitemap_locations")]
    public IEnumerable<Reference> SitemapLocations { get; set; }

    /// <summary>
    /// Gets or sets the external identifier of the content item.
    /// </summary>
    [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string ExternalId { get; set; }

    /// <summary>
    /// Gets or sets the last modified timestamp of the content item.
    /// </summary>
    [JsonProperty("last_modified")]
    public DateTime? LastModified { get; set; }
}
