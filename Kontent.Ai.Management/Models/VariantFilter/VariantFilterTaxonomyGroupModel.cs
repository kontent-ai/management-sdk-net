using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.VariantFilter;

/// <summary>
/// Represents the variant filter taxonomy group model.
/// </summary>
public sealed record VariantFilterTaxonomyGroupModel
{
    /// <summary>
    /// Gets the taxonomy reference.
    /// </summary>
    [JsonProperty("taxonomy_identifier")]
    public Reference TaxonomyReference { get; init; }

    /// <summary>
    /// Gets the term references.
    /// </summary>
    [JsonProperty("term_identifiers")]
    public IEnumerable<Reference> TermReferences { get; init; }

    /// <summary>
    /// Gets whether to include uncategorized items.
    /// </summary>
    [JsonProperty("include_uncategorized")]
    public bool IncludeUncategorized { get; init; }
}