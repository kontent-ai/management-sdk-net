using Kontent.Ai.Management.Models.VariantFilter;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.ItemWithVariant;

/// <summary>
/// Represents the request model for filtering items with variants.
/// </summary>
public class ItemWithVariantFilterRequestModel
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
}
