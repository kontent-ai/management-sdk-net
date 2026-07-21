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
    public IReadOnlyList<Reference>? Collections { get; init; }

    /// <summary>
    /// Restrict to these content types.
    /// </summary>
    [JsonPropertyName("content_types")]
    public IReadOnlyList<Reference>? ContentTypes { get; init; }

    /// <summary>
    /// Restrict to these languages.
    /// </summary>
    [JsonPropertyName("languages")]
    public IReadOnlyList<Reference>? Languages { get; init; }
}
