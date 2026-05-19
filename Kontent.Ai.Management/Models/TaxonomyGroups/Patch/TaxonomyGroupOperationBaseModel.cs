using Kontent.Ai.Management.Models.Shared;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.TaxonomyGroups.Patch;

/// <summary>
/// Represents the operation on the taxonomy group.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-taxonomy-group
/// </summary>
public abstract record TaxonomyGroupOperationBaseModel
{
    /// <summary>
    /// Gets specification of the operation to perform.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-taxonomy-group
    /// </summary>
    [JsonPropertyName("op")]
    public abstract string Op { get; }

    /// <summary>
    /// Gets the reference to existing taxonomy terms you want to modify.
    /// </summary>
    [JsonPropertyName("reference")]
    public Reference Reference { get; init; }

}
