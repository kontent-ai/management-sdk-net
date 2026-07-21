
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
    /// Controls how the output image is constrained within the provided width and height boundaries after resizing.
    /// Defaults to <see cref="ImageTransformationFit.Clip"/> — the only value the Management API currently accepts for rect-mode renditions.
    /// </summary>
    [JsonPropertyName("fit")]
    public ImageTransformationFit Fit { get; init; } = ImageTransformationFit.Clip;

    /// <summary>
    /// Output image width in pixels. Must be smaller than <see cref="Width"/>.
    /// </summary>
    [JsonPropertyName("custom_width")]
    public required int CustomWidth { get; init; }

    /// <summary>
    /// Output image height in pixels. Must be smaller than <see cref="Height"/>.
    /// </summary>
    [JsonPropertyName("custom_height")]
    public required int CustomHeight { get; init; }

    /// <summary>
    /// Horizontal offset of the source rectangle from the original image's top-left corner, in pixels.
    /// </summary>
    [JsonPropertyName("x")]
    public required int X { get; init; }

    /// <summary>
    /// Vertical offset of the source rectangle from the original image's top-left corner, in pixels.
    /// </summary>
    [JsonPropertyName("y")]
    public required int Y { get; init; }

    /// <summary>
    /// Width of the source rectangle, starting at <see cref="X"/>, <see cref="Y"/>.
    /// </summary>
    [JsonPropertyName("width")]
    public required int Width { get; init; }

    /// <summary>
    /// Height of the source rectangle, starting at <see cref="X"/>, <see cref="Y"/>.
    /// </summary>
    [JsonPropertyName("height")]
    public required int Height { get; init; }
}
