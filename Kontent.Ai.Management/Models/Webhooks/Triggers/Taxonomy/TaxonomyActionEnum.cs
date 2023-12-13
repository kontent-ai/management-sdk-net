using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Taxonomy;

/// <summary>
/// Represents taxonomy actions.
/// </summary>
public enum TaxonomyActionEnum
{
    /// <summary>
    /// Taxonomy created action.
    /// </summary>
    [EnumMember(Value = "created")]
    Created,
    
    /// <summary>
    /// Taxonomy metadata changed action.
    /// </summary>
    [EnumMember(Value = "metadata_changed")]
    MetadataChanged,
    
    /// <summary>
    /// Taxonomy deleted action.
    /// </summary>
    [EnumMember(Value = "deleted")]
    Deleted,
    
    /// <summary>
    /// Taxonomy term created action.
    /// </summary>
    [EnumMember(Value = "term_created")]
    TermCreated,
    
    /// <summary>
    /// Taxonomy term changed action.
    /// </summary>
    [EnumMember(Value = "term_changed")]
    TermChanged,
    
    /// <summary>
    /// Taxonomy term deleted action.
    /// </summary>
    [EnumMember(Value = "term_deleted")]
    TermDeleted,
    
    /// <summary>
    /// Taxonomy terms moved action.
    /// </summary>
    [EnumMember(Value = "terms_moved")]
    TermsMoved
}