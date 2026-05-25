using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Asset;

/// <summary>
/// Represents the asset trigger model. 
/// </summary>
public class AssetTriggerModel
{
    /// <summary>
    /// Determines if asset trigger is enabled.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    /// <summary>
    /// Represents asset actions.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonPropertyName("actions")]
    public IEnumerable<AssetActionModel> Actions { get; set; }
}