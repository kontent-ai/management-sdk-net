using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.EnvironmentValidation;

/// <summary>
/// The type of the async validation task issue.
/// </summary>
public enum AsyncValidationTaskIssueType
{
    /// <summary>
    /// Language variant issue.
    /// </summary>
    [EnumMember(Value = "variant_issue")]
    VariantIssue,

    /// <summary>
    /// Content type issue.
    /// </summary>
    [EnumMember(Value = "type_issue")]
    TypeIssue,
}
