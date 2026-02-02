using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.CustomApps.Patch;

/// <summary>
/// Represents enum of properties that can be replaced in the custom app.
/// </summary>
public enum PropertyName
{
    /// <summary>
    /// The custom app's name.
    /// </summary>
    [EnumMember(Value = "name")]
    Name,

    /// <summary>
    /// The custom app's codename.
    /// </summary>
    [EnumMember(Value = "codename")]
    Codename,

    /// <summary>
    /// The custom app's source url.
    /// </summary>
    [EnumMember(Value = "source_url")]
    SourceUrl,

    /// <summary>
    /// The custom app's config.
    /// </summary>
    [EnumMember(Value = "Config")]
    Config,

    /// <summary>
    /// The custom app's allowed_roles.
    /// </summary>
    [EnumMember(Value = "allowed_roles")]
    AllowedRoles,

    /// <summary>
    /// The custom app's display mode.
    /// </summary>
    [EnumMember(Value = "display_mode")]
    DisplayMode
}