
namespace Kontent.Ai.Management.Models.AssetRenditions;

/// <summary>
/// Request payload for updating an existing rendition's transformation.
/// </summary>
public sealed record AssetRenditionUpdateModel
{
    /// <summary>
    /// New transformation to apply to the rendition.
    /// </summary>
    [JsonPropertyName("transformation")]
    public required ImageTransformation Transformation { get; init; }
}
