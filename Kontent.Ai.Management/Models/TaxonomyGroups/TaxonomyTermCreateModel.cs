namespace Kontent.Ai.Management.Models.TaxonomyGroups;

/// <summary>
/// Payload for creating a taxonomy term (used both at create-group time and via the <c>addInto</c> patch operation). Recursive: a term may seed child terms.
/// </summary>
public sealed record TaxonomyTermCreateModel : TaxonomyBaseModel
{
    /// <summary>
    /// Initial child terms. Optional; omit to create a leaf term.
    /// </summary>
    [JsonPropertyName("terms")]
    public IEnumerable<TaxonomyTermCreateModel>? Terms { get; init; }
}
