namespace Kontent.Ai.Management.Models.TaxonomyGroups.Patch;

/// <summary>
/// Base shape for a taxonomy group PATCH operation. Concrete subtypes specialize the verb (<c>addInto</c>, <c>replace</c>, <c>move</c>, <c>remove</c>).
/// </summary>
public abstract record TaxonomyGroupOperationBaseModel
{
    /// <summary>
    /// Operation verb. Pinned by each concrete subtype.
    /// </summary>
    [JsonPropertyName("op")]
    public abstract string Op { get; }

    /// <summary>
    /// Reference to the target. Required for <c>replace</c>, <c>move</c>, and <c>remove</c>. On <c>addInto</c> it points at the parent term that should receive the new child; omit it to add at the root of the taxonomy group.
    /// </summary>
    [JsonPropertyName("reference")]
    public Reference? Reference { get; init; }
}
