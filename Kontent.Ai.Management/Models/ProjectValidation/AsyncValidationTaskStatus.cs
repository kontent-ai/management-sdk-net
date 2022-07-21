using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.ProjectValidation;

/// <summary>
/// The status of the async validation task.
/// </summary>
public enum AsyncValidationTaskStatus
{
    /// <summary>
    /// Task is queued.
    /// </summary>
    [EnumMember(Value = "queued")]
    Queued,

    /// <summary>
    /// Task is finished.
    /// </summary>
    [EnumMember(Value = "finished")]
    Finished,

    /// <summary>
    /// Task has failed.
    /// </summary>
    [EnumMember(Value = "failed")]
    Failed,
}
