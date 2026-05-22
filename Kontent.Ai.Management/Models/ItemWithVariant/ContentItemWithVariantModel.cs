using Kontent.Ai.Management.Models.Items;
using Kontent.Ai.Management.Models.LanguageVariants;

namespace Kontent.Ai.Management.Models.ItemWithVariant;

/// <summary>
/// Represents a content item together with its language variant.
/// </summary>
public sealed record ContentItemWithVariantModel
{
    /// <summary>
    /// Gets the content item.
    /// </summary>
    [JsonPropertyName("item")]
    public ContentItemModel Item { get; init; }

    /// <summary>
    /// Gets the language variant.
    /// </summary>
    [JsonPropertyName("variant")]
    public LanguageVariantModel Variant { get; init; }
}
