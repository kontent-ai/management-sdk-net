using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.VariantFilter;

/// <summary>
/// Represents the variant filter order model.
/// </summary>
public sealed record VariantFilterOrderModel
{
    /// <summary>
    /// Gets the order by column.
    /// </summary>
    [JsonProperty("by")]
    public string By { get; init; }

    /// <summary>
    /// Gets the order direction.
    /// </summary>
    [JsonProperty("direction")]
    public VariantFilterOrderDirection Direction { get; init; }
}