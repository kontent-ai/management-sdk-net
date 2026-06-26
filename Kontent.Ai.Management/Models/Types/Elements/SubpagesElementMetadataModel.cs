namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// A subpages element on a content type. Lets authors link sibling/child content items that form a sitemap branch.
/// </summary>
public sealed record SubpagesElementMetadataModel : ContentElementMetadataBase
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
    /// Content types allowed as linked subpages. Null means no restriction.
    /// </summary>
    [JsonPropertyName("allowed_content_types")]
    public IEnumerable<Reference>? AllowedContentTypes { get; init; }

    /// <inheritdoc/>
    [JsonPropertyName("type")]
    public override ElementMetadataType Type => ElementMetadataType.Subpages;
}
