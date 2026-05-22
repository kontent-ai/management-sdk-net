
namespace Kontent.Ai.Management.Models.Collections.Patch;

/// <summary>
/// Represents the addInto operation.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-collections
/// </summary>
public sealed record CollectionAddIntoPatchModel : CollectionOperationBaseModel
{
    /// <summary>
    /// Represents the addInto operation.
    /// </summary>
    public override string Op => "addInto";

    /// <summary>
    /// Gets the collection to be added.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-collections
    /// </summary>
    [JsonPropertyName("value")]
    public CollectionCreateModel Value { get; init; }

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
