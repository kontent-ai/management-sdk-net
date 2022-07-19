using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.Environments;

/// <summary>
/// Represents the state on environment cloning.
/// </summary>
public enum CloningState
{
    /// <summary>
    /// Environment cloning is in progress.
    /// </summary>
    [EnumMember(Value = "in_progress")]
    InProgress,

    /// <summary>
    /// Environment cloning failed.
    /// </summary>
    [EnumMember(Value = "failed")]
    Failed,

    /// <summary>
    /// Environment cloning is succesfully done.
    /// </summary>
    [EnumMember(Value = "done")]
    Done
}
