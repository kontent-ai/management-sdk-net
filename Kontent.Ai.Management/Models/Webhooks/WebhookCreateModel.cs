using Kontent.Ai.Management.Models.Webhooks.Triggers;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Webhooks;

/// <summary>
/// Represents the webhook create model.
/// </summary>
public class WebhookCreateModel
{
    /// <summary>
    /// Gets or sets the webhook's display name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }
    
    /// <summary>
    /// Gets or sets the URL to which the webhook notification will be sent.
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; set; }
    
    /// <summary>
    /// Gets or sets the webhook's secret key, used to authenticate that the webhook was sent by Kontent.
    /// </summary>
    [JsonProperty("secret")]
    public string Secret { get; set; }
    
    /// <summary>
    /// Gets or sets webhook's custom HTTP headers, used to send extra information in webhook notifications.
    /// </summary>
    [JsonProperty("headers")]
    public IEnumerable<CustomHeaderModel> Headers { get; set; }
    
    /// <summary>
    /// Determines if the webhook is enabled. By default, the enabled property is set to true.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonProperty("enabled")]
    public bool Enabled { get; set; }
    
    /// <summary>
    /// Gets or sets the specific events that trigger the webhook.
    /// The events can be set for the published or preview data in Delivery API.
    /// </summary>
    [JsonProperty("delivery_triggers")]
    public DeliveryTriggersModel DeliveryTriggers { get; set; }
}