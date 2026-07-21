
namespace Kontent.Ai.Management.Models.Spaces.Patch;

/// <summary>
/// Represents properties of the space.
/// </summary>
public enum SpacePropertyName
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
    /// The root item of the space.
    /// </summary>
    [EnumMember(Value = "root_item")]
    RootItem,

    /// <summary>
    /// The collections belonging to the space.
    /// </summary>
    [EnumMember(Value = "collections")]
    Collections,
}