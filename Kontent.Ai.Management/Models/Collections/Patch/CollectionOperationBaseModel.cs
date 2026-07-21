
namespace Kontent.Ai.Management.Models.Collections.Patch;

/// <summary>
/// Represents the operation on collections.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-collections
/// </summary>
public abstract record CollectionOperationBaseModel
{
    /// <summary>
    /// Gets specification of the operation to perform.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-collections
    /// </summary>
    [JsonPropertyName("op")]
    public abstract string Op { get; }
}
