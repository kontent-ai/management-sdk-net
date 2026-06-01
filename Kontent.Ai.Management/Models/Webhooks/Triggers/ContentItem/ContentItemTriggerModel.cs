namespace Kontent.Ai.Management.Models.Webhooks.Triggers.ContentItem;

/// <summary>
/// Content item event trigger for a webhook.
/// </summary>
public sealed record ContentItemTriggerModel
{
    /// <summary>
    /// Whether this trigger is enabled.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; init; }

    /// <summary>
    /// Content item actions that fire the webhook.
    /// </summary>
    [JsonPropertyName("actions")]
    public IEnumerable<ContentItemActionModel>? Actions { get; init; }

    /// <summary>
    /// Filters narrowing which content items fire the webhook.
    /// </summary>
    [JsonPropertyName("filters")]
    public ContentItemFiltersModel? Filters { get; init; }
}
