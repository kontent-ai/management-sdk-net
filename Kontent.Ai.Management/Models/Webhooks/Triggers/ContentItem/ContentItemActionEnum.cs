using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.ContentItem;

/// <summary>
/// Represents content item actions.
/// </summary>
public enum ContentItemActionEnum
{
    /// <summary>
    /// Content item created action.
    /// </summary>
    [EnumMember(Value = "created")]
    Created,
    
    /// <summary>
    /// Content item changed action.
    /// </summary>
    [EnumMember(Value = "changed")]
    Changed,
    
    /// <summary>
    /// Content item deleted action.
    /// </summary>
    [EnumMember(Value = "deleted")]
    Deleted,
    
    /// <summary>
    /// Content item published action.
    /// </summary>
    [EnumMember(Value = "published")]
    Published,
    
    /// <summary>
    /// Content item unpublished action.
    /// </summary>
    [EnumMember(Value = "unpublished")]
    Unpublished,
    
    /// <summary>
    /// Content item workflow step changed action.
    /// </summary>
    [EnumMember(Value = "workflow_step_changed")]
    WorkflowStepChanged,
    
    /// <summary>
    /// Content item metadata changed action.
    /// </summary>
    [EnumMember(Value = "metadata_changed")]
    MetadataChanged
}