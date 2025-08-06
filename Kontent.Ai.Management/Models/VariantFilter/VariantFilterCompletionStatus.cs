using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.VariantFilter;

/// <summary>
/// Represents the completion status of a content item variant.
/// </summary>
public enum VariantFilterCompletionStatus
{
    /// <summary>
    /// The variant is unfinished.
    /// </summary>
    [EnumMember(Value = "unfinished")]
    Unfinished,

    /// <summary>
    /// The variant is completed.
    /// </summary>
    [EnumMember(Value = "completed")]
    Completed,

    /// <summary>
    /// The variant is not translated.
    /// </summary>
    [EnumMember(Value = "not_translated")]
    NotTranslated,

    /// <summary>
    /// The variant is all done (completed and translated).
    /// </summary>
    [EnumMember(Value = "all_done")]
    AllDone
}