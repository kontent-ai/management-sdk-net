namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Taxonomy;

/// <summary>
/// Filters narrowing which taxonomies fire a webhook.
/// </summary>
public sealed record TaxonomyFiltersModel
{
    /// <summary>
    /// Restrict to these taxonomies.
    /// </summary>
    [JsonPropertyName("taxonomies")]
    public IEnumerable<Reference>? Taxonomies { get; init; }
}
