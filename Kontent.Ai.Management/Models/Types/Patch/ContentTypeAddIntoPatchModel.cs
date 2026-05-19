using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Types.Patch;

/// <summary>
/// Represents the addInto operation.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-content-type
/// </summary>
public sealed record ContentTypeAddIntoPatchModel : ContentTypeOperationBaseModel
{
    /// <summary>
    /// Represents the addInto operation.
    /// </summary>
    public override string Op => "addInto";

    /// <summary>
    /// Gets the object to be added. The value depends on the selected path.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-content-type
    /// </summary>
    [JsonProperty("value")]
    public dynamic Value { get; init; }

    /// <summary>
    /// Gets reference of the existing object before which you want to add the new object.
    /// Note: The before and after properties are mutually exclusive.
    /// </summary>
    [JsonProperty("before")]
    public Reference Before { get; init; }

    /// <summary>
    /// Gets reference of the existing object after which you want to add the new object.
    /// Note: The before and after properties are mutually exclusive.
    /// </summary>
    [JsonProperty("after")]
    public Reference After { get; init; }
}
