using Kontent.Ai.Management.Models.Items;
using Kontent.Ai.Management.Models.LanguageVariants;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.ItemWithVariant;

/// <summary>
/// Represents a content item together with its language variant.
/// </summary>
public class ContentItemWithVariantModel
{
    /// <summary>
    /// Gets or sets the content item.
    /// </summary>
    [JsonProperty("item")]
    public ContentItemModel Item { get; set; }

    /// <summary>
    /// Gets or sets the language variant.
    /// </summary>
    [JsonProperty("variant", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public LanguageVariantModel Variant { get; set; }
}
