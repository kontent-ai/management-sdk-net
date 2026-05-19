using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Types.Patch;

/// <summary>
/// Represents the replace operation.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-content-type
/// </summary>
public sealed record ContentTypeReplacePatchModel : ContentTypeOperationBaseModel
{
    /// <summary>
    /// Represents the replace operation.
    /// </summary>
    public override string Op => "replace";

    /// <summary>
    /// Gets the value to replace into the property specified in the path where the format depends on the specific property.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-content-type
    /// </summary>
    [JsonPropertyName("value")]
    public object Value { get; init; }
}
