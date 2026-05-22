
namespace Kontent.Ai.Management.Models.Collections.Patch;

/// <summary>
/// Represents move operation.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-collections
/// </summary>
public sealed record CollectionMovePatchModel : CollectionOperationBaseModel
{
    /// <summary>
    /// Represents move operation.
    /// </summary>
    public override string Op => "move";

    /// <summary>
    /// Gets the reference of the collection to move.
    /// </summary>
    [JsonPropertyName("reference")]
    public Reference Reference { get; init; }

    /// <summary>
    /// Gets reference of the existing collection before which you want to add the new collection.
    /// Note: The before and after properties are mutually exclusive.
    /// </summary>
    [JsonPropertyName("before")]
    public Reference Before { get; init; }

    /// <summary>
    /// Gets reference of the existing collection after which you want to add the new collection.
    /// Note: The before and after properties are mutually exclusive.
    /// </summary>
    [JsonPropertyName("after")]
    public Reference After { get; init; }
}
