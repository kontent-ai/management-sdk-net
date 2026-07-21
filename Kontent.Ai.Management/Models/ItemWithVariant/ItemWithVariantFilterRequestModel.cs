using Kontent.Ai.Management.Models.VariantFilter;

namespace Kontent.Ai.Management.Models.ItemWithVariant;

/// <summary>
/// Request payload for the items-with-variants filter endpoint.
/// </summary>
public sealed record ItemWithVariantFilterRequestModel
{
    /// <summary>
    /// Filter criteria. Omit to match all variants in the environment.
    /// </summary>
    [JsonPropertyName("filters")]
    public VariantFilterFiltersModel? Filters { get; init; }

    /// <summary>
    /// Result ordering. Omit for the server's default order.
    /// </summary>
    [JsonPropertyName("order")]
    public VariantFilterOrderModel? Order { get; init; }
}
