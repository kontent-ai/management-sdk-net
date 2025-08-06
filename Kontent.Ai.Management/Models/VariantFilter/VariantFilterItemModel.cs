using Kontent.Ai.Management.Models.Items;
using Kontent.Ai.Management.Models.LanguageVariants;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.VariantFilter;

/// <summary>
/// Represents a variant filter item model.
/// </summary>
public class VariantFilterItemModel
{
    /// <summary>
    /// Gets or sets the content item.
    /// </summary>
    [JsonProperty("item")]
    public ContentItemModel Item { get; set; }

    /// <summary>
    /// Gets or sets the language variant (only includes `elements` when include_content is not set to true).
    /// </summary>
    [JsonProperty("variant", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public LanguageVariantModel Variant { get; set; }
}