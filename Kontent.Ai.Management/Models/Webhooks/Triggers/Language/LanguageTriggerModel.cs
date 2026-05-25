using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Language;

/// <summary>
/// Represents the language trigger model. 
/// </summary>
public class LanguageTriggerModel
{
    /// <summary>
    /// Determines if language trigger is enabled.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    /// <summary>
    /// Represents language actions.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonPropertyName("actions")]
    public IEnumerable<LanguageActionModel> Actions { get; set; }

    /// <summary>
    /// Represents language filters.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonPropertyName("filters")]
    public LanguageFiltersModel Filters { get; set; }
}