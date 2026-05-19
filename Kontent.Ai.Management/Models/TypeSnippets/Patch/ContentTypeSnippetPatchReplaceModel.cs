using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.TypeSnippets.Patch;

/// <summary>
/// Represents the replace operation.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-content-type-snippet
/// </summary>
public sealed record ContentTypeSnippetPatchReplaceModel : ContentTypeSnippetOperationBaseModel
{
    /// <summary>
    /// Represents the replace operation.
    /// </summary>
    public override string Op => "replace";

    /// <summary>
    /// Gets the value to insert into the property specified in the path where the format depends on the specific property.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-content-type-snippet
    /// </summary>
    [JsonPropertyName("value")]
    public object Value { get; init; }
}
