using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.TaxonomyGroups.Patch;

/// <summary>
/// Represents the operation on the taxonomy group.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-taxonomy-group
/// </summary>
public abstract class TaxonomyGroupOperationBaseModel
{
    /// <summary>
    /// Gets specification of the operation to perform.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-taxonomy-group
    /// </summary>
    [JsonProperty("op")]
    public abstract string Op { get; }

    /// <summary>
    /// Gets or sets the reference to existing taxonomy terms you want to modify.
    /// </summary>
    [JsonProperty("reference")]
    public Reference Reference { get; set; }

}
