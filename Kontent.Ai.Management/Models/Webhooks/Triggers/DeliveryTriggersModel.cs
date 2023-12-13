using Kontent.Ai.Management.Models.Webhooks.Triggers.Asset;
using Kontent.Ai.Management.Models.Webhooks.Triggers.ContentItem;
using Kontent.Ai.Management.Models.Webhooks.Triggers.ContentType;
using Kontent.Ai.Management.Models.Webhooks.Triggers.Language;
using Kontent.Ai.Management.Models.Webhooks.Triggers.Taxonomy;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers;

/// <summary>
/// Represents specific events that trigger the webhook.
/// </summary>
public class DeliveryTriggersModel
{
    /// <summary>
    /// Gets or sets triggers for content types.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonProperty("content_type")]
    public ContentTypeTriggerModel ContentType { get; set; }
    
    /// <summary>
    /// Gets or sets triggers for content items.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonProperty("content_item")]
    public ContentItemTriggerModel ContentItem { get; set; }
    
    /// <summary>
    /// Gets or sets triggers for taxonomies.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonProperty("taxonomy")]
    public TaxonomyTriggerModel Taxonomy { get; set; }
    
    /// <summary>
    /// Gets or sets triggers for assets.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonProperty("asset")]
    public AssetTriggerModel Asset { get; set; }
    
    /// <summary>
    /// Gets or sets triggers for languages.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonProperty("language")]
    public LanguageTriggerModel Language { get; set; }
    
    /// <summary>
    /// Gets or sets the delivery slot.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonProperty("slot")]
    public DeliverySlot? Slot { get; set; }
    
    /// <summary>
    /// Gets or sets webhook events.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonProperty("events")]
    public WebhookEvents? Events { get; set; }
}