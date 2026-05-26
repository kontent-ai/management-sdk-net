namespace Kontent.Ai.Management.Models.Items;

/// <summary>
/// Represents content item upsert model.
/// </summary>
public sealed record ContentItemUpsertModel
{
    /// <summary>
    /// Gets the name of the content item.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the codename of the content item.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the type of the content item.
    /// Type is taken into account only when creating a new content item.
    /// Type is ignored in case of update.
    /// </summary>
    [JsonPropertyName("type")]
    public Reference Type { get; init; }

    /// <summary>
    /// Gets sitemap locations of the content item.
    /// </summary>
    [JsonPropertyName("sitemap_locations")]
    public IEnumerable<Reference> SitemapLocations { get; init; } = Enumerable.Empty<Reference>();

    /// <summary>
    /// Gets the collection of the content item.
    /// </summary>
    [JsonPropertyName("collection")]
    public Reference Collection { get; init; }

    /// <summary>
    /// Gets the external identifier of the content item.
    /// ExternalId is taken into account only when creating a new content item.
    /// ExternalId is ignored in case of update.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string ExternalId { get; init; }
}
