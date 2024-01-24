using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
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
    [JsonProperty("taxonomies")]
    public IEnumerable<Reference> Taxonomies { get; set; }
}