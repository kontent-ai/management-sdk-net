using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.ContentType;

/// <summary>
/// Represents content type actions.
/// </summary>
public enum ContentTypeActionEnum
{
    /// <summary>
    /// Content type created action.
    /// </summary>
    [EnumMember(Value = "created")]
    Created,
    
    /// <summary>
    /// Content type changed action.
    /// </summary>
    [EnumMember(Value = "changed")]
    Changed,
    
    /// <summary>
    /// Content type deleted action.
    /// </summary>
    [EnumMember(Value = "deleted")]
    Deleted
}