using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.CustomApps;

/// <summary>
/// Represents the display mode of a custom app.
/// </summary>
public enum CustomAppDisplayMode
{
    /// <summary>
    /// The custom app is displayed in full screen.
    /// </summary>
    [EnumMember(Value = "fullScreen")]
    FullScreen,

    /// <summary>
    /// The custom app is displayed in a dialog.
    /// </summary>
    [EnumMember(Value = "dialog")]
    Dialog
}
