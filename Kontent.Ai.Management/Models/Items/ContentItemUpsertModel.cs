namespace Kontent.Ai.Management.Models.Items;

/// <summary>
/// Request payload for upserting a content item via <c>PUT /items/{identifier}</c>. The item identifier is carried by the URL; only the upsert-by-external-id form can create a new item, in which case the URL's external_id is also assigned to the new item.
/// </summary>
public sealed record ContentItemUpsertModel
{
    /// <summary>
    /// Display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public string? Codename { get; init; }

    /// <summary>
    /// Reference to the content type. Used only when this upsert creates a new item (upsert-by-external-id, target not found); ignored when updating an existing item.
    /// </summary>
    [JsonPropertyName("type")]
    public Reference? Type { get; init; }

    /// <summary>
    /// Sitemap locations. Deprecated — sitemap is being phased out.
    /// </summary>
    [JsonPropertyName("sitemap_locations")]
    public IEnumerable<Reference>? SitemapLocations { get; init; }

    /// <summary>
    /// Reference to the collection the item should belong to.
    /// </summary>
    [JsonPropertyName("collection")]
    public Reference? Collection { get; init; }
}
