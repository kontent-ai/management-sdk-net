namespace Kontent.Ai.Management.Models.Webhooks.Triggers.ContentType;

/// <summary>
/// Filters narrowing which content types fire a webhook.
/// </summary>
public sealed record ContentTypeFiltersModel
{
    /// <summary>
    /// Restrict to these content types.
    /// </summary>
    [JsonPropertyName("content_types")]
    public IEnumerable<Reference>? ContentTypes { get; init; }
}
