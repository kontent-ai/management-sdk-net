using Kontent.Ai.Management.Models.Items;
using Kontent.Ai.Management.Models.LanguageVariants;

namespace Kontent.Ai.Management.Models.ItemWithVariant;

/// <summary>
/// A content item paired with one of its language variants.
/// </summary>
public sealed record ContentItemWithVariantModel
{
    /// <summary>
    /// The content item.
    /// </summary>
    [JsonPropertyName("item")]
    public required ContentItemModel Item { get; init; }

    /// <summary>
    /// The language variant. May be absent in list responses when the item exists but has no variant in the targeted language.
    /// </summary>
    [JsonPropertyName("variant")]
    public LanguageVariantModel? Variant { get; init; }
}
