using Kontent.Ai.Management.Models.Webhooks.Triggers.Asset;
using Kontent.Ai.Management.Models.Webhooks.Triggers.ContentItem;
using Kontent.Ai.Management.Models.Webhooks.Triggers.ContentType;
using Kontent.Ai.Management.Models.Webhooks.Triggers.Language;
using Kontent.Ai.Management.Models.Webhooks.Triggers.Taxonomy;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers;

/// <summary>
/// The set of events that trigger a webhook. Each category is opt-in; configure only the ones the webhook should react to.
/// </summary>
public sealed record DeliveryTriggersModel
{
    /// <summary>
    /// Content type events.
    /// </summary>
    [JsonPropertyName("content_type")]
    public ContentTypeTriggerModel? ContentType { get; init; }

    /// <summary>
    /// Content item events.
    /// </summary>
    [JsonPropertyName("content_item")]
    public ContentItemTriggerModel? ContentItem { get; init; }

    /// <summary>
    /// Taxonomy events.
    /// </summary>
    [JsonPropertyName("taxonomy")]
    public TaxonomyTriggerModel? Taxonomy { get; init; }

    /// <summary>
    /// Asset events.
    /// </summary>
    [JsonPropertyName("asset")]
    public AssetTriggerModel? Asset { get; init; }

    /// <summary>
    /// Language events.
    /// </summary>
    [JsonPropertyName("language")]
    public LanguageTriggerModel? Language { get; init; }

    /// <summary>
    /// Delivery slot (published or preview) the events apply to.
    /// </summary>
    [JsonPropertyName("slot")]
    public DeliverySlot? Slot { get; init; }

    /// <summary>
    /// Whether all events or only the specified subset trigger the webhook.
    /// </summary>
    [JsonPropertyName("events")]
    public WebhookEvents? Events { get; init; }
}
