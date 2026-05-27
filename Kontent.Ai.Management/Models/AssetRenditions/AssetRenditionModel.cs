namespace Kontent.Ai.Management.Models.AssetRenditions;

/// <summary>
/// An asset rendition — a server-stored transformation of an underlying asset image.
/// </summary>
public sealed record AssetRenditionModel
{
    /// <summary>
    /// Server-generated rendition ID.
    /// </summary>
    [JsonPropertyName("rendition_id")]
    public required Guid RenditionId { get; init; }

    /// <summary>
    /// ID of the asset this rendition belongs to.
    /// </summary>
    [JsonPropertyName("asset_id")]
    public required Guid AssetId { get; init; }

    /// <summary>
    /// Caller-supplied external ID. Only present when one was specified on create.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string? ExternalId { get; init; }

    /// <summary>
    /// Specifies how the original image is transformed to produce this rendition.
    /// </summary>
    [JsonPropertyName("transformation")]
    public required ImageTransformation Transformation { get; init; }

    /// <summary>
    /// ISO-8601 timestamp of the last change to the rendition.
    /// </summary>
    [JsonPropertyName("last_modified")]
    public required DateTime LastModified { get; init; }
}
