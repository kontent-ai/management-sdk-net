using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Types.Patch;

/// <summary>
/// Represents the operation on the content type.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-content-type
/// </summary>
public abstract class ContentTypeOperationBaseModel
{
    /// <summary>
    /// Gets specification of the operation to perform.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-content-type
    /// </summary>
    [JsonProperty("op", Required = Required.Always)]
    public abstract string Op { get; }

    /// <summary>
    /// Gets or sets a string identifying where the new object or property should be added/replaced/removed.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-content-type
    /// </summary>
    [JsonProperty("path", Required = Required.Always)]
    public string Path { get; set; }
}
