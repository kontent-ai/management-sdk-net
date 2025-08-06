using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.VariantFilter;

/// <summary>
/// Represents the variant filter taxonomy group model.
/// </summary>
public class VariantFilterTaxonomyGroupModel
{
    /// <summary>
    /// Gets or sets the taxonomy reference.
    /// </summary>
    [JsonProperty("taxonomy_identifier")]
    public Reference TaxonomyReference { get; set; }

    /// <summary>
    /// Gets or sets the term references.
    /// </summary>
    [JsonProperty("term_identifiers")]
    public IEnumerable<Reference> TermReferences { get; set; }

    /// <summary>
    /// Gets or sets whether to include uncategorized items.
    /// </summary>
    [JsonProperty("include_uncategorized")]
    public bool IncludeUncategorized { get; set; }
}