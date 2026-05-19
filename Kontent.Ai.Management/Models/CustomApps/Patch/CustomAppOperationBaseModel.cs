using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.CustomApps.Patch;

/// <summary>
/// Represents the operation on the custom app.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-custom-app
/// </summary>
public abstract record CustomAppOperationBaseModel
{
    /// <summary>
    /// Gets specification of the operation to perform.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-custom-app
    /// </summary>
    [JsonPropertyName("op")]
    public abstract string Op { get; }

    /// <summary>
    /// Gets the name of the property to modify.
    /// </summary>
    [JsonPropertyName("property_name")]
    public PropertyName PropertyName { get; init; }

    /// <summary>
    /// Gets the value to replace into the property specified in the path where the format depends on the specific property.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-custom-app
    /// </summary>
    [JsonPropertyName("value")]
    public object Value { get; init; }
}