using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.LegacyWebhooks.Triggers;

/// <summary>
/// Represents the management api trigger model.
/// </summary>
public class ManagementApiTriggerModel
{
    /// <summary>
    /// Represents content types for which the webhook should be triggered.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Legacy-webhook-object
    /// </summary>
    [JsonProperty("type")]
    public static TriggerChangeType Type => TriggerChangeType.LanguageVariant;

    /// <summary>
    /// Gets or sets operations for which the webhook should be triggered.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Legacy-webhook-object
    /// </summary>
    [JsonProperty("operations")]
    public IEnumerable<string> Operations { get; set; }
}
