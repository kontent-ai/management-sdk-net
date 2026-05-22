
namespace Kontent.Ai.Management.Models.ItemWithVariant;

/// <summary>
/// Represents a single result from the items with variant filter endpoint.
/// </summary>
public sealed record ItemWithVariantFilterResultModel
{
    /// <summary>
    /// Gets the item reference.
    /// </summary>
    [JsonPropertyName("item")]
    public Reference Item { get; init; }

    /// <summary>
    /// Gets the language reference.
    /// </summary>
    [JsonPropertyName("language")]
    public Reference Language { get; init; }
}
