using Kontent.Ai.Management.Models.Types.Elements.DefaultValues;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// An asset element on a content type.
/// </summary>
public sealed record AssetElementMetadataModel : ElementMetadataBase
{
    /// <summary>
    /// Display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Whether authors must fill in the element. Defaults to false.
    /// </summary>
    [JsonPropertyName("is_required")]
    public bool IsRequired { get; init; }

    /// <summary>
    /// Whether the element is non-localizable (shared across all languages). Defaults to false.
    /// </summary>
    [JsonPropertyName("is_non_localizable")]
    public bool IsNonLocalizable { get; init; }

    /// <summary>
    /// HTML guidelines shown to authors.
    /// </summary>
    [JsonPropertyName("guidelines")]
    public string? Guidelines { get; init; }

    /// <summary>
    /// Limits the number of assets authors can attach. Null means no count restriction.
    /// </summary>
    [JsonPropertyName("asset_count_limit")]
    public LimitModel? AssetCountLimit { get; init; }

    /// <summary>
    /// Maximum allowed file size in bytes. Null means no size restriction.
    /// </summary>
    [JsonPropertyName("maximum_file_size")]
    public long? MaximumFileSize { get; init; }

    /// <summary>
    /// File-type restriction (all, images only, ...). Null means no restriction.
    /// </summary>
    [JsonPropertyName("allowed_file_types")]
    public FileType? AllowedFileTypes { get; init; }

    /// <summary>
    /// Limits image width. Null means no width restriction.
    /// </summary>
    [JsonPropertyName("image_width_limit")]
    public LimitModel? ImageWidthLimit { get; init; }

    /// <summary>
    /// Limits image height. Null means no height restriction.
    /// </summary>
    [JsonPropertyName("image_height_limit")]
    public LimitModel? ImageHeightLimit { get; init; }

    /// <summary>
    /// Default value applied when authors create a new language variant.
    /// </summary>
    [JsonPropertyName("default")]
    public AssetDefaultValueModel? DefaultValue { get; init; }

    /// <inheritdoc/>
    [JsonPropertyName("type")]
    public override ElementMetadataType Type => ElementMetadataType.Asset;
}
