namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Taxonomy;

/// <summary>
/// A taxonomy action that fires the webhook.
/// </summary>
public sealed record TaxonomyActionModel
{
    /// <summary>
    /// The action performed on the taxonomy.
    /// </summary>
    [JsonPropertyName("action")]
    public required TaxonomyAction Action { get; init; }
}
