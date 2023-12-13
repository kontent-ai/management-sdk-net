using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.LegacyWebhooks.Triggers;

/// <summary>
/// Represents the delivery api trigger model.
/// </summary>
public class DeliveryApiTriggerModel
{
    /// <summary>
    /// Gets or sets content types for which the webhook should be triggered.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Legacy-webhook-object
    /// </summary>
    [JsonProperty("type")]
    public TriggerChangeType Type { get; set; }

    /// <summary>
    /// Gets or sets operations for which the webhook should be triggered.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Legacy-webhook-object
    /// </summary>
    [JsonProperty("operations")]
    public IEnumerable<string> Operations { get; set; }
}
