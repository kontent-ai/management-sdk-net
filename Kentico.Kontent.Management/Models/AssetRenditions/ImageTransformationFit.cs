using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.AssetRenditions;

/// <summary>
/// Controls how the output image is constrained within the provided width and height boundaries after resizing  
/// </summary>
public enum ImageTransformationFit
{
    /// <summary>
    /// Default mode. Resizes the image to fit within the width and height boundaries without cropping or distorting the image.
    /// </summary>
    [EnumMember(Value = "clip")]
    Clip,
}
