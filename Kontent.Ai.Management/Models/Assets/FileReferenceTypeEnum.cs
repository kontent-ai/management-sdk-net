using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.Assets;

/// <summary>
/// Represents file reference type.
/// </summary>
public enum FileReferenceTypeEnum
{
    /// <summary>
    /// Internal reference type.
    /// </summary>
    [EnumMember(Value = "internal")]
    Internal
}
