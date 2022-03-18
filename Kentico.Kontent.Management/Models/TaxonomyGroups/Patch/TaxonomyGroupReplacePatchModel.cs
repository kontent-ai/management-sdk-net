using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.TaxonomyGroups.Patch;

/// <summary>
/// Represents the replace operation.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-taxonomy-group
/// </summary>
public class TaxonomyGroupReplacePatchModel : TaxonomyGroupOperationBaseModel
{
    /// <summary>
    /// Represents the replace operation.
    /// </summary>
    public override string Op => "replace";

    /// <summary>
    /// Specifies the property of the taxonomy group or term that you want to replace.
    /// </summary>
    [JsonProperty("property_name")]
    public PropertyName PropertyName { get; set; }

    /// <summary>
    /// Gets or sets the new value. Based on the value of PropertyName, the value can be either string or an array of taxonomy terms.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-taxonomy-group
    /// </summary>
    [JsonProperty("value")]
    public dynamic Value { get; set; }
}
