using Kentico.Kontent.Management.Modules.ActionInvoker;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.AssetRenditions;

/// <summary>
/// Represents image transformation.
/// </summary>
[JsonConverter(typeof(ImageTransformationConverter))]
public abstract class ImageTransformation
{
    /// <summary>
    /// Gets the transformation's mode.
    ///
    /// Only the rect mode is allowed.
    /// The rect mode selects a sub-region of the original image to use for processing.
    /// </summary>
    [JsonProperty("mode", Required = Required.Always)]
    public abstract ImageTransformationMode Mode { get; }
}
