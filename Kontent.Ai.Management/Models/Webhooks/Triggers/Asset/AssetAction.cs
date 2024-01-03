using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Asset;

/// <summary>
/// Represents asset actions.
/// </summary>
public enum AssetAction
{
    /// <summary>
    /// Asset created action.
    /// </summary>
    [EnumMember(Value = "created")]
    Created,
    
    /// <summary>
    /// Asset changed action.
    /// </summary>
    [EnumMember(Value = "changed")]
    Changed,
    
    /// <summary>
    /// Asset deleted action.
    /// </summary>
    [EnumMember(Value = "deleted")]
    Deleted,
    
    /// <summary>
    /// Asset metadata changed action.
    /// </summary>
    [EnumMember(Value = "metadata_changed")]
    MetadataChanged
}