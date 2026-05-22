
namespace Kontent.Ai.Management.Models.VariantFilter;

/// <summary>
/// Represents the variant filter order model.
/// </summary>
public sealed record VariantFilterOrderModel
{
    /// <summary>
    /// Gets the order by column.
    /// </summary>
    [JsonPropertyName("by")]
    public string By { get; init; }

    /// <summary>
    /// Gets the order direction.
    /// </summary>
    [JsonPropertyName("direction")]
    public VariantFilterOrderDirection Direction { get; init; }
}