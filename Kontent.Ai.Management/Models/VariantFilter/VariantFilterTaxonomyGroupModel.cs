using Kontent.Ai.Management.Models.Shared;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.VariantFilter;

/// <summary>
/// Represents the variant filter taxonomy group model.
/// </summary>
public sealed record VariantFilterTaxonomyGroupModel
{
    /// <summary>
    /// Gets the taxonomy reference.
    /// </summary>
    [JsonPropertyName("taxonomy_identifier")]
    public Reference TaxonomyReference { get; init; }

    /// <summary>
    /// Gets the term references.
    /// </summary>
    [JsonPropertyName("term_identifiers")]
    public IEnumerable<Reference> TermReferences { get; init; }

    /// <summary>
    /// Gets whether to include uncategorized items.
    /// </summary>
    [JsonPropertyName("include_uncategorized")]
    public bool IncludeUncategorized { get; init; }
}