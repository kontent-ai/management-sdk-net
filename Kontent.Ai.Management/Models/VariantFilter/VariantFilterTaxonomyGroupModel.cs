namespace Kontent.Ai.Management.Models.VariantFilter;

/// <summary>
/// Taxonomy-based filter: restrict results to variants tagged in a specific taxonomy group, optionally narrowing to specific terms.
/// </summary>
public sealed record VariantFilterTaxonomyGroupModel
{
    /// <summary>
    /// Reference to the taxonomy group.
    /// </summary>
    [JsonPropertyName("taxonomy_identifier")]
    public required Reference TaxonomyReference { get; init; }

    /// <summary>
    /// Specific terms within the taxonomy to match. Omit to match any term in the group.
    /// </summary>
    [JsonPropertyName("term_identifiers")]
    public IReadOnlyList<Reference>? TermReferences { get; init; }

    /// <summary>
    /// When true, also include variants with no assignment in this taxonomy group.
    /// </summary>
    [JsonPropertyName("include_uncategorized")]
    public bool IncludeUncategorized { get; init; }
}