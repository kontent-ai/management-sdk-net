using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Spaces.Patch;

/// <summary>
/// Represents the replace operation.
/// </summary>
public class SpaceOperationReplaceModel
{
    /// <summary>
    /// Represents the replace operation.
    /// </summary>
    [JsonProperty("op", Required = Required.Always)]
    public static string Op => "replace";

    /// <summary>
    /// Gets or sets the name of the property to modify.
    /// </summary>
    [JsonProperty("property_name", Required = Required.Always)]
    public PropertyName PropertyName { get; set; }
    
    /// <summary>
    /// Gets or sets the value to insert in the specified property.
    /// </summary>
    [JsonProperty("value", Required = Required.Always)]
    public dynamic Value { get; set; }
}