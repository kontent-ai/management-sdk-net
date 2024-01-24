using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Taxonomy;

/// <summary>
/// Represents the taxonomy trigger model. 
/// </summary>
public class TaxonomyTriggerModel
{
    /// <summary>
    /// Determines if taxonomy trigger is enabled.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonProperty("enabled")]
    public bool? Enabled { get; set; }
    
    /// <summary>
    /// Represents taxonomy actions.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonProperty("actions")]
    public IEnumerable<TaxonomyActionModel> Actions { get; set; }

    /// <summary>
    /// Represents taxonomy filters.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonProperty("filters")]
    public TaxonomyFiltersModel Filters { get; set; }
}