namespace Kontent.Ai.Management.Models.Items;

/// <summary>
/// Represents content item model.
/// </summary>
public sealed record ContentItemModel
{
    /// <summary>
    /// Gets the id of the content item.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

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
    /// </summary>
    [JsonPropertyName("type")]
    public Reference Type { get; init; }

    /// <summary>
    /// Gets the collection of the content item.
    /// </summary>
    [JsonPropertyName("collection")]
    public Reference Collection { get; init; }

    /// <summary>
    /// Gets the spaces of the content item
    /// </summary>
    [JsonPropertyName("spaces")]
    public IReadOnlyCollection<Reference> Spaces { get; init; }

    /// <summary>
    /// Gets sitemap locations of the content item.
    /// </summary>
    [JsonPropertyName("sitemap_locations")]
    public IEnumerable<Reference> SitemapLocations { get; init; }

    /// <summary>
    /// Gets the external identifier of the content item.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string ExternalId { get; init; }

    /// <summary>
    /// Gets the last modified timestamp of the content item.
    /// </summary>
    [JsonPropertyName("last_modified")]
    public DateTime? LastModified { get; init; }
}
