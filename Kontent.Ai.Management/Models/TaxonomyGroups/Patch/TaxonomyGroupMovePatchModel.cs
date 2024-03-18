using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.TaxonomyGroups.Patch;

/// <summary>
/// Represents the move operation.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-taxonomy-group
/// </summary>
public class TaxonomyGroupMovePatchModel : TaxonomyGroupOperationBaseModel
{
    /// <summary>
    /// Represents the move operation.
    /// </summary>
    public override string Op => "move";

    /// <summary>
    /// Gets or sets reference of the existing taxonomy term before which you want to move the specified taxonomy term.
    /// Note: The before, after and under properties are mutually exclusive.
    /// </summary>
    [JsonProperty("before")]
    public Reference Before { get; set; }

    /// <summary>
    /// Gets or sets reference of the existing taxonomy term after which you want to move the specified taxonomy term.
    /// Note: The before, after and under properties are mutually exclusive.
    /// </summary>
    [JsonProperty("after")]
    public Reference After { get; set; }

    /// <summary>
    /// Gets or sets reference of the existing taxonomy term under which you want to move the specified taxonomy term.
    /// Note: The before, after and under properties are mutually exclusive.
    /// </summary>
    [JsonProperty("under")]
    public Reference Under { get; set; }
}