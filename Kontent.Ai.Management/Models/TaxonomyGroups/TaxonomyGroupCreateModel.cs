namespace Kontent.Ai.Management.Models.TaxonomyGroups;

/// <summary>
/// Payload for creating a taxonomy group.
/// </summary>
public sealed record TaxonomyGroupCreateModel : TaxonomyBaseModel
{
    /// <summary>
    /// Initial root-level terms. Optional; omit to create an empty group.
    /// </summary>
    [JsonPropertyName("terms")]
    public IEnumerable<TaxonomyTermCreateModel>? Terms { get; init; }
}
