using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.VariantFilter;

/// <summary>
/// Represents the variant filter order model.
/// </summary>
public class VariantFilterOrderModel
{
    /// <summary>
    /// Gets or sets the order by column.
    /// </summary>
    [JsonProperty("by")]
    public string By { get; set; }

    /// <summary>
    /// Gets or sets the order direction.
    /// </summary>
    [JsonProperty("direction")]
    public string Direction { get; set; }
}