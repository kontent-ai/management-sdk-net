namespace Kontent.Ai.Management.Models.AssetRenditions;

/// <summary>
/// Represents asset rendition model.
/// </summary>
public sealed record AssetRenditionModel
{
    /// <summary>
    /// Gets the rendition's ID.
    /// </summary>
    [JsonPropertyName("rendition_id")]
    public Guid RenditionId { get; init; }

    /// <summary>
    /// Gets the id of the asset this rendition belongs to.
    /// </summary>
    [JsonPropertyName("asset_id")]
    public Guid AssetId { get; init; }

    /// <summary>
    /// Gets the rendition's external ID.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string ExternalId { get; init; }

    /// <summary>
    /// Gets the image transformation.
    /// The transformation specifies how to transform the original image asset.
    /// 
    /// The image area to use for the transformation is determined by the x, y, width, and height properties.
    /// The custom_width and custom_height properties set the dimensions of the output image.
    /// 
    /// The x and y coordinates define a point in the original image.
    /// The point is specified as the distance from the top-left corner of the original image asset in pixels.
    /// The whole area must be within the borders of the original image.
    /// Upscaling, that is setting the custom_width and custom_height greater than width and height, is not allowed.
    /// </summary>
    [JsonPropertyName("transformation")]
    public ImageTransformation Transformation { get; init; }

    /// <summary>
    /// Gets the ISO-8601 formatted date/time of the last change to the rendition.
    /// </summary>
    [JsonPropertyName("last_modified")]
    public DateTime? LastModified { get; init; }
}
