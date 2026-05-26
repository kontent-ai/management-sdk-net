using Kontent.Ai.Management.Models.Webhooks.Triggers;

namespace Kontent.Ai.Management.Models.Webhooks;

/// <summary>
/// Represents the webhook model.
/// </summary>
public class WebhookModel
{
    /// <summary>
    /// Gets or sets the webhook's internal ID.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the webhook's display name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the URL to which the webhook notification will be sent.
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; }

    /// <summary>
    /// Gets or sets the webhook's secret key, used to authenticate that the webhook was sent by Kontent.
    /// </summary>
    [JsonPropertyName("secret")]
    public string Secret { get; set; }

    /// <summary>
    /// Gets or sets webhook's custom HTTP headers, used to send extra information in webhook notifications.
    /// </summary>
    [JsonPropertyName("headers")]
    public IEnumerable<CustomHeaderModel> Headers { get; set; }

    /// <summary>
    /// Determines if the webhook is enabled. By default, the enabled property is set to true.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets ISO-8601 formatted date/time of the last change to the webhook.
    /// </summary>
    [JsonPropertyName("last_modified")]
    public DateTime? LastModified { get; set; }

    /// <summary>
    /// The webhook's health status tells you whether the webhook is operational.
    /// </summary>
    [JsonPropertyName("health_status")]
    public WebhookHealthStatus HealthStatus { get; set; }

    /// <summary>
    /// Gets or sets the specific events that trigger the webhook.
    /// The events can be set for the published or preview data in Delivery API.
    /// </summary>
    [JsonPropertyName("delivery_triggers")]
    public DeliveryTriggersModel DeliveryTriggers { get; set; }
}