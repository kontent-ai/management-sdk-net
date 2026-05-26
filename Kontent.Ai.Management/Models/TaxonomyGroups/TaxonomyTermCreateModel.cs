namespace Kontent.Ai.Management.Models.TaxonomyGroups;

/// <summary>
/// Represents the taxonomy term create model.
/// </summary>
public sealed record TaxonomyTermCreateModel : TaxonomyBaseModel
{
    /// <summary>
    /// Gets terms in the taxonomy group.
    /// </summary>
    [JsonPropertyName("terms")]
    public IEnumerable<TaxonomyTermCreateModel> Terms { get; init; }
}
