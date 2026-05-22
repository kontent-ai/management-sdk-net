using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Taxonomy;

/// <summary>
/// Represents the taxonomy filters model.
/// </summary>
public class TaxonomyFiltersModel
{
    /// <summary>
    /// References to taxonomies
    /// </summary>
    [JsonPropertyName("taxonomies")]
    public IEnumerable<Reference> Taxonomies { get; set; }
}