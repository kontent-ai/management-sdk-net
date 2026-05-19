using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Items;

/// <summary>
/// Represents content item model.
/// </summary>
public sealed record ContentItemModel
{
    /// <summary>
    /// Gets the id of the content item.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; init; }

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
    /// </summary>
    [JsonProperty("type")]
    public Reference Type { get; init; }

    /// <summary>
    /// Gets the collection of the content item.
    /// </summary>
    [JsonProperty("collection")]
    public Reference Collection { get; init; }

    /// <summary>
    /// Gets the spaces of the content item
    /// </summary>
    [JsonProperty("spaces")]
    public IReadOnlyCollection<Reference> Spaces { get; init; }

    /// <summary>
    /// Gets sitemap locations of the content item.
    /// </summary>
    [JsonProperty("sitemap_locations")]
    public IEnumerable<Reference> SitemapLocations { get; init; }

    /// <summary>
    /// Gets the external identifier of the content item.
    /// </summary>
    [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string ExternalId { get; init; }

    /// <summary>
    /// Gets the last modified timestamp of the content item.
    /// </summary>
    [JsonProperty("last_modified")]
    public DateTime? LastModified { get; init; }
}
