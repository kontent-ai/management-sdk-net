using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.ContentItem;

/// <summary>
/// Represents the content item trigger model. 
/// </summary>
public class ContentItemTriggerModel
{
    /// <summary>
    /// Determines if content item trigger is enabled.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonProperty("enabled")]
    public bool? Enabled { get; set; }
    
    /// <summary>
    /// Represents content item actions.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonProperty("actions")]
    public IEnumerable<ContentItemActionModel> Actions { get; set; }
    
    /// <summary>
    /// Represents content item filters.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonProperty("filters")]
    public ContentItemFiltersModel Filters { get; set; }
}