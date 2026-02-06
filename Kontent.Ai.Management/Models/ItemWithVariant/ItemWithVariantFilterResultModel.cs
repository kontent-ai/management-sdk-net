using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.ItemWithVariant;

/// <summary>
/// Represents a single result from the items with variant filter endpoint.
/// </summary>
public class ItemWithVariantFilterResultModel
{
    /// <summary>
    /// Gets or sets the item reference.
    /// </summary>
    [JsonProperty("item")]
    public Reference Item { get; set; }

    /// <summary>
    /// Gets or sets the language reference.
    /// </summary>
    [JsonProperty("language")]
    public Reference Language { get; set; }
}
