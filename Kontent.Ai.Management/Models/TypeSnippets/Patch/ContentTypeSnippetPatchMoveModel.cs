
namespace Kontent.Ai.Management.Models.TypeSnippets.Patch;

/// <summary>
/// Represents the move operation.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-content-type-snippet
/// </summary>
public sealed record ContentTypeSnippetPatchMoveModel : ContentTypeSnippetOperationBaseModel
{
    /// <summary>
    /// Represents the move operation.
    /// </summary>
    public override string Op => "move";

    /// <summary>
    /// Gets reference of the existing object before which you want to move the specified object.
    /// Note: The before and after properties are mutually exclusive.
    /// </summary>
    [JsonPropertyName("before")]
    public Reference Before { get; init; }

    /// <summary>
    /// Gets reference of the existing object after which you want to move the specified object.
    /// Note: The before and after properties are mutually exclusive.
    /// </summary>
    [JsonPropertyName("after")]
    public Reference After { get; init; }
}