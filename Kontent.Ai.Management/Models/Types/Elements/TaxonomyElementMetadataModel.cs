using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Types.Elements.DefaultValues;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Represents a taxonomy element in content types.
/// </summary>
public class TaxonomyElementMetadataModel : ElementMetadataBase
{
    /// <summary>
    /// Gets or sets a flag determining whether the element must be filled in.
    /// </summary>
    [JsonProperty("is_required")]
    public bool IsRequired { get; set; }

    /// <summary>
    /// Gets or sets element is non-localizable
    /// </summary>
    [JsonProperty("is_non_localizable")]
    public bool IsNonLocalizable { get; set; }

    /// <summary>
    /// Gets or sets the element's guidelines, providing instructions on what to fill in.
    /// </summary>
    [JsonProperty("guidelines")]
    public string Guidelines { get; set; }

    /// <summary>
    /// Specifies a reference to the taxonomy group that the element uses.
    /// </summary>
    [JsonProperty("taxonomy_group")]
    public Reference TaxonomyGroup { get; set; }

    /// <summary>
    /// Specifies the limitation for the number of terms that can be selected in the element.
    /// </summary>
    [JsonProperty("term_count_limit")]
    public LimitModel TermCountLimit { get; set; }

    /// <summary>
    /// Specifies the default value for the element value.
    /// </summary>
    [JsonProperty("default")]
    public TaxonomyElementDefaultValueModel DefaultValue { get; set; }

    /// <summary>
    /// Gets or sets terms in the taxonomy group.
    /// </summary>
    public override ElementMetadataType Type => ElementMetadataType.Taxonomy;
}