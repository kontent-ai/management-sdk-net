using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.Spaces.Patch;

/// <summary>
/// Represents properties of the space.
/// </summary>
public enum PropertyName
{
    /// <summary>
    /// The space's codename.
    /// </summary>
    [EnumMember(Value = "codename")]
    Codename,

    /// <summary>
    /// The space's name.
    /// </summary>
    [EnumMember(Value = "name")]
    Name,
}