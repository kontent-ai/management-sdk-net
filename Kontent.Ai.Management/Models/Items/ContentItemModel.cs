namespace Kontent.Ai.Management.Models.Items;

/// <summary>
/// A single content item, as returned by the Management API.
/// </summary>
public sealed record ContentItemModel
{
    /// <summary>
    /// Content item ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Codename. Auto-generated from the name when not supplied on create.
    /// </summary>
    [JsonPropertyName("codename")]
    public required string Codename { get; init; }

    /// <summary>
    /// Reference to the content type the item belongs to.
    /// </summary>
    [JsonPropertyName("type")]
    public required Reference Type { get; init; }

    /// <summary>
    /// Reference to the collection the item is assigned to. When the item has no explicit collection, this references the default collection (id <c>00000000-0000-0000-0000-000000000000</c>).
    /// </summary>
    [JsonPropertyName("collection")]
    public required Reference Collection { get; init; }

    /// <summary>
    /// Spaces the item is assigned to. Empty when none.
    /// </summary>
    [JsonPropertyName("spaces")]
    public required IReadOnlyList<Reference> Spaces { get; init; }

    /// <summary>
    /// Sitemap locations. Deprecated — sitemap is being phased out.
    /// </summary>
    [JsonPropertyName("sitemap_locations")]
    public IReadOnlyList<Reference>? SitemapLocations { get; init; }

    /// <summary>
    /// Caller-supplied external ID. Only present when one was specified on create.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string? ExternalId { get; init; }

    /// <summary>
    /// ISO-8601 timestamp of the most recent change.
    /// </summary>
    [JsonPropertyName("last_modified")]
    public required DateTime LastModified { get; init; }
}
