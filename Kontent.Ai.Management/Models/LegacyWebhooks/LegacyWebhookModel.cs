using Kontent.Ai.Management.Models.LegacyWebhooks.Triggers;
using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.LegacyWebhooks;

/// <summary>
/// Represents the legacy webhook model.
/// </summary>
public class LegacyWebhookModel
{
    /// <summary>
    /// Gets or sets ISO-8601 formatted date/time of the last change to the webhook.
    /// </summary>
    [JsonProperty("last_modified")]
    public DateTime? LastModified { get; set; }
    
    /// <summary>
    /// The webhook's health status tells you whether the webhook is operational.
    /// </summary>
    [JsonProperty("health_status")]
    public LegacyWebhookHealthStatus HealthStatus { get; set; }

    /// <summary>
    /// Gets or sets the webhook's internal ID.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }

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
    /// Determines if the webhook is enabled. By default, the enabled property is set to true.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Legacy-webhook-object
    /// </summary>
    [JsonProperty("enabled")]
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the specific events that trigger the webhook. At least one valid trigger is required.
    /// </summary>
    [JsonProperty("triggers")]
    public LegacyWebhookTriggersModel Triggers { get; set; }
}
