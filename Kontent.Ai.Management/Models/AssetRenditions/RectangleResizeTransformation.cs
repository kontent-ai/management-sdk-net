
namespace Kontent.Ai.Management.Models.AssetRenditions;

/// <summary>
/// Represents rectangle resize type of transformation.
/// </summary>
public sealed record RectangleResizeTransformation : ImageTransformation
{
    /// <summary>
    /// The rect mode selects a sub-region of the original image to use for processing.
    /// </summary>
    [JsonPropertyName("mode")]
    public override ImageTransformationMode Mode => ImageTransformationMode.Rect;

    /// <summary>
    /// Gets the fit parameter.
    ///
    /// The fit parameter controls how the output image is constrained within the provided width and height boundaries after resizing.
    /// Only the clip resize fit mode is allowed.
    /// </summary>
    [JsonPropertyName("fit")]
    public ImageTransformationFit Fit { get; init; }

    /// <summary>
    /// Gets the output image's width in pixels. Must be smaller than width.
    ///
    /// Use custom_width if you want to resize the rectangle region selected via x, y, width, and height.
    /// That is to keep the contents of the selected rectangle region but make the output image smaller.
    /// </summary>
    [JsonPropertyName("custom_width")]
    public int CustomWidth { get; init; }

    /// <summary>
    /// Gets the output image's height in pixels. Must be smaller than height.
    ///
    /// Use custom_height if you want to resize the rectangle region selected via x, y, width, and height.
    /// That is to keep the contents of the selected rectangle region but make the output image smaller.
    /// </summary>
    [JsonPropertyName("custom_height")]
    public int CustomHeight { get; init; }

    /// <summary>
    /// Gets the X coordinate.
    ///
    /// Represents a position on the horizontal axis of the original image as the distance from the image's top-left corner in pixels.
    /// Combined with the y property, the coordinates specify the top-left corner of the source rectangle region defined by width and height.
    ///
    /// The x's maximum value depends on the width parameter. The whole rectangle must fit within the borders of the original image.
    /// </summary>
    [JsonPropertyName("x")]
    public int X { get; init; }

    /// <summary>
    /// Gets the Y coordinate.
    ///
    /// Represents a position on the vertical axis of the original image as the distance from the image's top-left corner in pixels.
    /// Combined with the x property, the coordinates specify the top-left corner of the source rectangle region defined by width and height.
    ///
    /// The y's maximum value depends on the height parameter. The whole rectangle must fit within the borders of the original image.
    /// </summary>
    [JsonPropertyName("y")]
    public int Y { get; init; }

    /// <summary>
    /// Gets the width of the rectangle area.
    /// The rectangle starts at the coordinates defined by x and y.
    /// </summary>
    [JsonPropertyName("width")]
    public int Width { get; init; }

    /// <summary>
    /// Gets the height of the rectangle area.
    /// The rectangle starts at the coordinates defined by x and y.
    /// </summary>
    [JsonPropertyName("height")]
    public int Height { get; init; }
}
