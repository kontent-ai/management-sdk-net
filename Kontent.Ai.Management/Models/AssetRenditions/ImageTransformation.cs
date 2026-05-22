using Kontent.Ai.Management.Serialization.Converters;

namespace Kontent.Ai.Management.Models.AssetRenditions;

/// <summary>
/// Represents image transformation.
/// </summary>
[JsonConverter(typeof(ImageTransformationJsonConverter))]
public abstract record ImageTransformation
{
    /// <summary>
    /// Gets the transformation's mode.
    ///
    /// Only the rect mode is allowed.
    /// The rect mode selects a sub-region of the original image to use for processing.
    /// </summary>
    [JsonPropertyName("mode")]
    public abstract ImageTransformationMode Mode { get; }
}
