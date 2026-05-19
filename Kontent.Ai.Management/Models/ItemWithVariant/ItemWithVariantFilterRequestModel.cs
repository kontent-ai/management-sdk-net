using Kontent.Ai.Management.Models.VariantFilter;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.ItemWithVariant;

/// <summary>
/// Represents the request model for filtering items with variants.
/// </summary>
public sealed record ItemWithVariantFilterRequestModel
{
    /// <summary>
    /// Gets the filters.
    /// </summary>
    [JsonProperty("filters")]
    public VariantFilterFiltersModel Filters { get; init; }

    /// <summary>
    /// Gets the order.
    /// </summary>
    [JsonProperty("order")]
    public VariantFilterOrderModel Order { get; init; }
}
