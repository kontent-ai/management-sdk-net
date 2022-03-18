using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Types.Elements;

/// <summary>
/// Represents the allowed file types for the asset element in content types.
/// </summary>
public enum FileType
{
    /// <summary>
    /// Any file type.
    /// </summary>
    [EnumMember(Value = "any")]
    Any,

    /// <summary>
    /// Images that support image transformation.
    /// More info: https://kontent.ai/learn/reference/image-transformation
    /// </summary>
    [EnumMember(Value = "adjustable")]
    Adjustable
}
