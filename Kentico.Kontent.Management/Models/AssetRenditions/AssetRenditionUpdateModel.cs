using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.AssetRenditions;

/// <summary>
/// Represents an asset rendition update model.
/// </summary>
public class AssetRenditionUpdateModel
{
    /// <summary>
    /// Gets or sets the image transformation.
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
    [JsonProperty("transformation", Required = Required.Always)]
    public ImageTransformation Transformation { get; set; }
}
