namespace Kontent.Ai.Management.Models.Webhooks.Triggers.ContentItem;

/// <summary>
/// Filters narrowing which content items fire a webhook. Each facet is optional.
/// </summary>
public sealed record ContentItemFiltersModel
{
    /// <summary>
    /// Restrict to these collections.
    /// </summary>
    [JsonPropertyName("collections")]
    public IEnumerable<Reference>? Collections { get; init; }

    /// <summary>
    /// Restrict to these content types.
    /// </summary>
    [JsonPropertyName("content_types")]
    public IEnumerable<Reference>? ContentTypes { get; init; }

    /// <summary>
    /// Restrict to these languages.
    /// </summary>
    [JsonPropertyName("languages")]
    public IEnumerable<Reference>? Languages { get; init; }
}
