using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.VariantFilter;

/// <summary>
/// Represents the publishing state of a content item variant.
/// </summary>
public enum VariantFilterPublishingState
{
    /// <summary>
    /// The variant is published.
    /// </summary>
    [EnumMember(Value = "published")]
    Published,

    /// <summary>
    /// The variant is unpublished.
    /// </summary>
    [EnumMember(Value = "unpublished")]
    Unpublished,

    /// <summary>
    /// The variant has not been published yet.
    /// </summary>
    [EnumMember(Value = "not_published_yet")]
    NotPublishedYet
}
