using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.TypeSnippets.Patch;

/// <summary>
/// Represents the operation on the content type snippet.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-content-type-snippet
/// </summary>
public abstract record ContentTypeSnippetOperationBaseModel
{
    /// <summary>
    /// Gets specification of the operation to perform.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-content-type-snippet
    /// </summary>
    [JsonPropertyName("op")]
    public abstract string Op { get; }

    /// <summary>
    /// Gets a string identifying where the new object or property should be added/replaced/removed.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-content-type
    /// </summary>
    [JsonPropertyName("path")]
    public string Path { get; init; }
}
