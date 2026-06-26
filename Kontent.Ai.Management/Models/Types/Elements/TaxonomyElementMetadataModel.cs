using Kontent.Ai.Management.Models.Types.Elements.DefaultValues;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// A taxonomy element on a content type. Authors tag the content item with terms from a taxonomy group.
/// </summary>
public sealed record TaxonomyElementMetadataModel : ContentElementMetadataBase
{
    /// <summary>
    /// Display name. Defaults to the taxonomy group's name when omitted on create.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Reference to the taxonomy group whose terms this element exposes.
    /// </summary>
    [JsonPropertyName("taxonomy_group")]
    public required Reference TaxonomyGroup { get; init; }

    /// <summary>
    /// Limits the number of terms authors can select. Null means no count restriction.
    /// </summary>
    [JsonPropertyName("term_count_limit")]
    public LimitModel? TermCountLimit { get; init; }

    /// <summary>
    /// Default value applied when authors create a new language variant.
    /// </summary>
    [JsonPropertyName("default")]
    public TaxonomyElementDefaultValueModel? DefaultValue { get; init; }

    /// <inheritdoc/>
    [JsonPropertyName("type")]
    public override ElementMetadataType Type => ElementMetadataType.Taxonomy;
}
