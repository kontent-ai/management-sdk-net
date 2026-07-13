namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Taxonomy;

/// <summary>
/// Taxonomy event trigger for a webhook.
/// </summary>
public sealed record TaxonomyTriggerModel
{
    /// <summary>
    /// Whether this trigger is enabled.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; init; }

    /// <summary>
    /// Taxonomy actions that fire the webhook.
    /// </summary>
    [JsonPropertyName("actions")]
    public IReadOnlyList<TaxonomyActionModel>? Actions { get; init; }

    /// <summary>
    /// Filters narrowing which taxonomies fire the webhook.
    /// </summary>
    [JsonPropertyName("filters")]
    public TaxonomyFiltersModel? Filters { get; init; }
}
