using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.ItemWithVariant;

/// <summary>
/// Represents an item and language reference pair used to identify a specific variant.
/// </summary>
public sealed record VariantIdentifierModel
{
    /// <summary>
    /// Gets the item reference.
    /// </summary>
    [JsonProperty("item")]
    public Reference Item { get; init; }

    /// <summary>
    /// Gets the language reference.
    /// </summary>
    [JsonProperty("language")]
    public Reference Language { get; init; }
}
