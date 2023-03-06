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
    
    /// <summary>
    /// The web spotlight root item of the space.
    /// </summary>
    [EnumMember(Value = "web_spotlight_root_item")]
    WebSpotlightRootItem,
}