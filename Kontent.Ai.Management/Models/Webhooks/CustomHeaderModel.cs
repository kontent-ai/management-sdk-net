using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Webhooks;

/// <summary>
/// Represents webhook custom HTTP headers. Custom headers are sent together with existing headers.
/// </summary>
public class CustomHeaderModel
{
    /// <summary>
    /// The custom header key defines the name of the HTTP header.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonProperty("key")]
    public string Key { get; set; }
    
    /// <summary>
    /// The custom header value.
    /// More info: https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Webhook-object
    /// </summary>
    [JsonProperty("value")]
    public string Value { get; set; }
}