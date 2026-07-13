using Kontent.Ai.Management.Models.Types.Elements.DefaultValues;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// A linked items element on a content type. Lets authors reference other content items.
/// </summary>
public sealed record LinkedItemsElementMetadataModel : ContentElementMetadataBase
{
    /// <summary>
    /// Display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Limits the number of items authors can link. Null means no count restriction.
    /// </summary>
    [JsonPropertyName("item_count_limit")]
    public LimitModel? ItemCountLimit { get; init; }

    /// <summary>
    /// Content types allowed as linked items. Null means no restriction.
    /// </summary>
    [JsonPropertyName("allowed_content_types")]
    public IReadOnlyList<Reference>? AllowedContentTypes { get; init; }

    /// <summary>
    /// Default value applied when authors create a new language variant.
    /// </summary>
    [JsonPropertyName("default")]
    public LinkedItemsDefaultValueModel? DefaultValue { get; init; }

    /// <inheritdoc/>
    [JsonPropertyName("type")]
    public override ElementMetadataType Type => ElementMetadataType.LinkedItems;
}
