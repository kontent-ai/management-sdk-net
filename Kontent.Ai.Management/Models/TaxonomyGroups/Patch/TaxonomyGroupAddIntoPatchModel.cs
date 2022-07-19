using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.TaxonomyGroups.Patch;

/// <summary>
/// Represents the addInto operation.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-taxonomy-group
/// </summary>
public class TaxonomyGroupAddIntoPatchModel : TaxonomyGroupOperationBaseModel
{
    /// <summary>
    /// Represents the addInto operation.
    /// </summary>
    public override string Op => "addInto";

    /// <summary>
    /// Gets or sets taxonomy term object you want to add.
    /// </summary>
    [JsonProperty("value")]
    public TaxonomyTermCreateModel Value { get; set; }

    /// <summary>
    /// Gets or sets reference of the existing taxonomy term before which you want to add the new taxonomy term.
    /// Note: The before and after properties are mutually exclusive.
    /// </summary>
    [JsonProperty("before")]
    public Reference Before { get; set; }

    /// <summary>
    /// Gets or sets reference of the existing taxonomy term before which you want to add the new taxonomy term.
    /// Note: The before and after properties are mutually exclusive.
    /// </summary>
    [JsonProperty("after")]
    public Reference After { get; set; }
}
