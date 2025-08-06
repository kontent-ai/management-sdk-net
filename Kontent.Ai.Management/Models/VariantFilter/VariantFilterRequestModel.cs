using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.VariantFilter;

/// <summary>
/// Represents the variant filter request model.
/// </summary>
public class VariantFilterRequestModel
{
    /// <summary>
    /// Gets or sets the filters.
    /// </summary>
    [JsonProperty("filters")]
    public VariantFilterFiltersModel Filters { get; set; }

    /// <summary>
    /// Gets or sets the order.
    /// </summary>
    [JsonProperty("order")]
    public VariantFilterOrderModel Order { get; set; }

    /// <summary>
    /// Gets or sets whether to include content in the response. If the content is not included, the `elements` property in the language variants response will be omitted.  
    /// </summary>
    [JsonProperty("include_content")]
    public bool? IncludeContent { get; set; }
}