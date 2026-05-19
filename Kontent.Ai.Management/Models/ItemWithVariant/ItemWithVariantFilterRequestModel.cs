using Kontent.Ai.Management.Models.VariantFilter;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.ItemWithVariant;

/// <summary>
/// Represents the request model for filtering items with variants.
/// </summary>
public sealed record ItemWithVariantFilterRequestModel
{
    /// <summary>
    /// Gets the filters.
    /// </summary>
    [JsonPropertyName("filters")]
    public VariantFilterFiltersModel Filters { get; init; }

    /// <summary>
    /// Gets the order.
    /// </summary>
    [JsonPropertyName("order")]
    public VariantFilterOrderModel Order { get; init; }
}
