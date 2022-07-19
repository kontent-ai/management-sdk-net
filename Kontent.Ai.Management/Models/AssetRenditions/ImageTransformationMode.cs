using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.AssetRenditions;

/// <summary>
/// The transformation's mode.
/// </summary>
public enum ImageTransformationMode
{
    /// <summary>
    /// Selects a sub-region of the original image to use for processing.
    /// </summary>
    [EnumMember(Value = "rect")]
    Rect,
}
