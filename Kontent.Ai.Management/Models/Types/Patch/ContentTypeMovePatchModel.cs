using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Types.Patch;

/// <summary>
/// Represents the move operation.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-content-type
/// </summary>
public sealed record ContentTypeMovePatchModel : ContentTypeOperationBaseModel
{
    /// <summary>
    /// Represents the move operation.
    /// </summary>
    public override string Op => "move";

    /// <summary>
    /// Gets reference of the existing object before which you want to move the specified object.
    /// Note: The before and after properties are mutually exclusive.
    /// </summary>
    [JsonProperty("before")]
    public Reference Before { get; init; }

    /// <summary>
    /// Gets reference of the existing object after which you want to move the specified object.
    /// Note: The before and after properties are mutually exclusive.
    /// </summary>
    [JsonProperty("after")]
    public Reference After { get; init; }
}