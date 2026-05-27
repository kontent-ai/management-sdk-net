
namespace Kontent.Ai.Management.Models.ItemWithVariant;

/// <summary>
/// An (item, language) pair that uniquely identifies a content item variant.
/// </summary>
public sealed record VariantIdentifierModel
{
    /// <summary>
    /// Reference to the content item.
    /// </summary>
    [JsonPropertyName("item")]
    public required Reference Item { get; init; }

    /// <summary>
    /// Reference to the language.
    /// </summary>
    [JsonPropertyName("language")]
    public required Reference Language { get; init; }
}
