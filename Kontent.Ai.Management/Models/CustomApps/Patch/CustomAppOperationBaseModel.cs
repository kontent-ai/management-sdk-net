using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.CustomApps.Patch;

/// <summary>
/// Represents the operation on the custom app.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-custom-app
/// </summary>
public abstract class CustomAppOperationBaseModel
{
    /// <summary>
    /// Gets specification of the operation to perform.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-custom-app
    /// </summary>
    [JsonProperty("op")]
    public abstract string Op { get; }

    /// <summary>
    /// Gets or sets the name of the property to modify.
    /// </summary>
    [JsonProperty("property_name", Required = Required.Always)]
    public PropertyName PropertyName { get; set; }

    /// <summary>
    /// Gets or sets the value to replace into the property specified in the path where the format depends on the specific property.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-custom-app
    /// </summary>
    [JsonProperty("value")]
    public dynamic Value { get; set; }
}