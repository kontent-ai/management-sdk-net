
namespace Kontent.Ai.Management.Models.TaxonomyGroups.Patch;

/// <summary>
/// Represents the move operation.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-taxonomy-group
/// </summary>
public sealed record TaxonomyGroupMovePatchModel : TaxonomyGroupOperationBaseModel
{
    /// <summary>
    /// Represents the move operation.
    /// </summary>
    public override string Op => "move";

    /// <summary>
    /// Gets reference of the existing taxonomy term before which you want to move the specified taxonomy term.
    /// Note: The before, after and under properties are mutually exclusive.
    /// </summary>
    [JsonPropertyName("before")]
    public Reference Before { get; init; }

    /// <summary>
    /// Gets reference of the existing taxonomy term after which you want to move the specified taxonomy term.
    /// Note: The before, after and under properties are mutually exclusive.
    /// </summary>
    [JsonPropertyName("after")]
    public Reference After { get; init; }

    /// <summary>
    /// Gets reference of the existing taxonomy term under which you want to move the specified taxonomy term.
    /// Note: The before, after and under properties are mutually exclusive.
    /// </summary>
    [JsonPropertyName("under")]
    public Reference Under { get; init; }
}