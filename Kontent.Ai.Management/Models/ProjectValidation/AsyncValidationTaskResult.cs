using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.ProjectValidation;

/// <summary>
/// The result of the async validation task.
/// </summary>
public enum AsyncValidationTaskResult
{
    /// <summary>
    /// The async validation task is not finished yet.
    /// </summary>
    [EnumMember(Value = "unknown")]
    Unknown,

    /// <summary>
    /// Project doesn't contain any issues.
    /// </summary>
    [EnumMember(Value = "valid")]
    Valid,

    /// <summary>
    /// Project contains validation issues.
    /// </summary>
    [EnumMember(Value = "invalid")]
    Invalid,
}
