using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Language;

/// <summary>
/// Represents language actions.
/// </summary>
public enum LanguageAction
{
    /// <summary>
    /// Language created action.
    /// </summary>
    [EnumMember(Value = "created")]
    Created,
    
    /// <summary>
    /// Language changed action.
    /// </summary>
    [EnumMember(Value = "changed")]
    Changed,
    
    /// <summary>
    /// Language deleted action.
    /// </summary>
    [EnumMember(Value = "deleted")]
    Deleted
}