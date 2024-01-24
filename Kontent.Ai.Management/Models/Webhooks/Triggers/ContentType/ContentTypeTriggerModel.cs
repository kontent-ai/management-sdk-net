using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.ContentType;

/// <summary>
/// Represents the content type trigger model. 
/// </summary>
public class ContentTypeTriggerModel
{
    /// <summary>
    /// Determines if content type trigger is enabled.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonProperty("enabled")]
    public bool? Enabled { get; set; }
    
    /// <summary>
    /// Represents content type actions.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonProperty("actions")]
    public IEnumerable<ContentTypeActionModel> Actions { get; set; }

    /// <summary>
    /// Represents content type filters.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonProperty("filters")]
    public ContentTypeFiltersModel Filters { get; set; }
}