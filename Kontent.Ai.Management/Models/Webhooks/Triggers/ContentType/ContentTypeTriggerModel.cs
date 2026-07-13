namespace Kontent.Ai.Management.Models.Webhooks.Triggers.ContentType;

/// <summary>
/// Content type event trigger for a webhook.
/// </summary>
public sealed record ContentTypeTriggerModel
{
    /// <summary>
    /// Whether this trigger is enabled.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; init; }

    /// <summary>
    /// Content type actions that fire the webhook.
    /// </summary>
    [JsonPropertyName("actions")]
    public IReadOnlyList<ContentTypeActionModel>? Actions { get; init; }

    /// <summary>
    /// Filters narrowing which content types fire the webhook.
    /// </summary>
    [JsonPropertyName("filters")]
    public ContentTypeFiltersModel? Filters { get; init; }
}
