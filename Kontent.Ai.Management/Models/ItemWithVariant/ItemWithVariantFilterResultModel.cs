
namespace Kontent.Ai.Management.Models.ItemWithVariant;

/// <summary>
/// A single result from the items-with-variants filter endpoint: the (item, language) identifier of a matched variant.
/// </summary>
public sealed record ItemWithVariantFilterResultModel
{
    /// <summary>
    /// Reference to the matched content item.
    /// </summary>
    [JsonPropertyName("item")]
    public required Reference Item { get; init; }

    /// <summary>
    /// Reference to the matched language.
    /// </summary>
    [JsonPropertyName("language")]
    public required Reference Language { get; init; }
}
