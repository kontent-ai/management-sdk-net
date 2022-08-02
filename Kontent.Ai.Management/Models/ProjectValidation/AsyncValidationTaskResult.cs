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
    [EnumMember(Value = "none")]
    None,

    /// <summary>
    /// Project doesn't contain any issues.
    /// </summary>
    [EnumMember(Value = "no_issues")]
    NoIssues,

    /// <summary>
    /// Project contains validation issues.
    /// </summary>
    [EnumMember(Value = "issues_found")]
    IssuesFound,
}
