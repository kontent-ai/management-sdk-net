namespace Kontent.Ai.Management.Models.TaxonomyGroups.Patch;

/// <summary>
/// <c>replace</c> operation. Replaces a property of the taxonomy group or one of its terms. The targeted object is identified by <see cref="TaxonomyGroupOperationBaseModel.Reference"/>; <see cref="PropertyName"/> selects which of its properties to replace.
/// </summary>
public sealed record TaxonomyGroupReplacePatchModel : TaxonomyGroupOperationBaseModel
{
    /// <inheritdoc/>
    public override string Op => "replace";

    /// <summary>
    /// Property to replace. Valid values are <see cref="Patch.PropertyName.Codename"/>, <see cref="Patch.PropertyName.Name"/>, and <see cref="Patch.PropertyName.Terms"/>.
    /// </summary>
    [JsonPropertyName("property_name")]
    public required PropertyName PropertyName { get; init; }

    /// <summary>
    /// New value. Type depends on <see cref="PropertyName"/>: <c>string</c> for <c>codename</c> / <c>name</c>, <c>IEnumerable&lt;TaxonomyTermCreateModel&gt;</c> for <c>terms</c>.
    /// </summary>
    [JsonPropertyName("value")]
    public required object Value { get; init; }
}
