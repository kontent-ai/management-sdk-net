using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.ItemWithVariant;

/// <summary>
/// Represents an item and language reference pair used to identify a specific variant.
/// </summary>
public class VariantIdentifierModel
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
