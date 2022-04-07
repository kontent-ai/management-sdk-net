using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.AssetRenditions;

/// <summary>
/// Represents rectangle resize type of transformation.
/// </summary>
public class RectangleResizeTransformation : ImageTransformation
{
    /// <summary>
    /// The rect mode selects a sub-region of the original image to use for processing.
    /// </summary>
    [JsonProperty("mode", Required = Required.Always)]
    public override ImageTransformationMode Mode => ImageTransformationMode.Rect;

    /// <summary>
    /// Gets or sets the fit parameter.
    /// 
    /// The fit parameter controls how the output image is constrained within the provided width and height boundaries after resizing.
    /// Only the clip resize fit mode is allowed.
    /// </summary>
    [JsonProperty("fit", Required = Required.Always)]
    public ImageTransformationFit Fit { get; set; }

    /// <summary>
    /// Gets or sets the output image's width in pixels. Must be smaller than width.
    /// 
    /// Use custom_width if you want to resize the rectangle region selected via x, y, width, and height.
    /// That is to keep the contents of the selected rectangle region but make the output image smaller.
    /// </summary>
    [JsonProperty("custom_width", Required = Required.Always)]
    public int CustomWidth { get; set; }

    /// <summary>
    /// Gets or sets the output image's height in pixels. Must be smaller than height.
    /// 
    /// Use custom_height if you want to resize the rectangle region selected via x, y, width, and height.
    /// That is to keep the contents of the selected rectangle region but make the output image smaller.
    /// </summary>
    [JsonProperty("custom_height", Required = Required.Always)]
    public int CustomHeight { get; set; }

    /// <summary>
    /// Gets or sets the X coordinate.
    /// 
    /// Represents a position on the horizontal axis of the original image as the distance from the image's top-left corner in pixels.
    /// Combined with the y property, the coordinates specify the top-left corner of the source rectangle region defined by width and height.
    ///
    /// The x's maximum value depends on the width parameter. The whole rectangle must fit within the borders of the original image.
    /// </summary>
    [JsonProperty("x", Required = Required.Always)]
    public int X { get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate.
    /// 
    /// Represents a position on the vertical axis of the original image as the distance from the image's top-left corner in pixels.
    /// Combined with the x property, the coordinates specify the top-left corner of the source rectangle region defined by width and height.
    ///
    /// The y's maximum value depends on the height parameter. The whole rectangle must fit within the borders of the original image.
    /// </summary>
    [JsonProperty("y", Required = Required.Always)]
    public int Y { get; set; }

    /// <summary>
    /// Gets or sets the width of the rectangle area.
    /// The rectangle starts at the coordinates defined by x and y.
    /// </summary>
    [JsonProperty("width", Required = Required.Always)]
    public int Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the rectangle area.
    /// The rectangle starts at the coordinates defined by x and y.
    /// </summary>
    [JsonProperty("height", Required = Required.Always)]
    public int Height { get; set; }
}
