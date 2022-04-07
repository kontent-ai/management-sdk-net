using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Webhooks.Triggers;

/// <summary>
/// Represents the management api trigger model.
/// </summary>
public class ManagementApiTriggerModel
{
    /// <summary>
    /// Represents content types for which the webhook should be triggered.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#section/Webhook-object
    /// </summary>
    [JsonProperty("type")]
    public static TriggerChangeType Type => TriggerChangeType.LanguageVariant;

    /// <summary>
    /// Gets or sets operations for which the webhook should be triggered.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#section/Webhook-object
    /// </summary>
    [JsonProperty("operations")]
    public IEnumerable<string> Operations { get; set; }
}
