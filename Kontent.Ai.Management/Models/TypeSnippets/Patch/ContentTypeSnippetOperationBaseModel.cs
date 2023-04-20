using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.TypeSnippets.Patch;

/// <summary>
/// Represents the operation on the content type snippet.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-content-type-snippet
/// </summary>
public abstract class ContentTypeSnippetOperationBaseModel
{
    /// <summary>
    /// Gets specification of the operation to perform.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-content-type-snippet
    /// </summary>
    [JsonProperty("op")]
    public abstract string Op { get; }

    /// <summary>
    /// Gets or sets a string identifying where the new object or property should be added/replaced/removed.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-content-type
    /// </summary>
    [JsonProperty("path")]
    public string Path { get; set; }
}
