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
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    /// <summary>
    /// Represents content type actions.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonPropertyName("actions")]
    public IEnumerable<ContentTypeActionModel> Actions { get; set; }

    /// <summary>
    /// Represents content type filters.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonPropertyName("filters")]
    public ContentTypeFiltersModel Filters { get; set; }
}