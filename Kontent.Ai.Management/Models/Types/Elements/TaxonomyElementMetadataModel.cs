using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Types.Elements.DefaultValues;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Represents a taxonomy element in content types.
/// </summary>
public sealed record TaxonomyElementMetadataModel : ElementMetadataBase
{
    /// <summary>
    /// Gets the element's display name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; init; }
    /// <summary>
    /// Gets a flag determining whether the element must be filled in.
    /// </summary>
    [JsonProperty("is_required")]
    public bool IsRequired { get; init; }

    /// <summary>
    /// Gets element is non-localizable
    /// </summary>
    [JsonProperty("is_non_localizable")]
    public bool IsNonLocalizable { get; init; }

    /// <summary>
    /// Gets the element's guidelines, providing instructions on what to fill in.
    /// </summary>
    [JsonProperty("guidelines")]
    public string Guidelines { get; init; }

    /// <summary>
    /// Specifies a reference to the taxonomy group that the element uses.
    /// </summary>
    [JsonProperty("taxonomy_group")]
    public Reference TaxonomyGroup { get; init; }

    /// <summary>
    /// Specifies the limitation for the number of terms that can be selected in the element.
    /// </summary>
    [JsonProperty("term_count_limit")]
    public LimitModel TermCountLimit { get; init; }

    /// <summary>
    /// Specifies the default value for the element value.
    /// </summary>
    [JsonProperty("default")]
    public TaxonomyElementDefaultValueModel DefaultValue { get; init; }

    /// <summary>
    /// Gets terms in the taxonomy group.
    /// </summary>
    public override ElementMetadataType Type => ElementMetadataType.Taxonomy;
}