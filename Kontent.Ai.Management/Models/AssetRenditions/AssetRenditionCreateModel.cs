
namespace Kontent.Ai.Management.Models.AssetRenditions;

/// <summary>
/// Request payload for adding a rendition to an asset.
/// </summary>
public sealed record AssetRenditionCreateModel
{
    /// <summary>
    /// Optional caller-supplied external ID for the rendition.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string? ExternalId { get; init; }

    /// <summary>
    /// Specifies how the original image should be transformed to produce the rendition.
    /// </summary>
    [JsonPropertyName("transformation")]
    public required ImageTransformation Transformation { get; init; }
}
