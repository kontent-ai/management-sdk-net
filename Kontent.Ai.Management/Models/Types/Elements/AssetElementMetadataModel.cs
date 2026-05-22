using Kontent.Ai.Management.Models.Types.Elements.DefaultValues;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Represents asset element in type.
/// </summary>
public sealed record AssetElementMetadataModel : ElementMetadataBase
{
    /// <summary>
    /// Gets the element's display name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Determines whether the element must be filled in.
    /// </summary>
    [JsonPropertyName("is_required")]
    public bool IsRequired { get; init; }

    /// <summary>
    /// Gets element is non-localizable
    /// </summary>
    [JsonPropertyName("is_non_localizable")]
    public bool IsNonLocalizable { get; init; }

    /// <summary>
    /// Gets the element's guidelines.
    /// Guidelines are used to providing instructions on what to fill in.
    /// </summary>
    [JsonPropertyName("guidelines")]
    public string Guidelines { get; init; }

    /// <summary>
    /// Gets the specification of the limitation for the number of assets allowed within the element.
    /// </summary>
    [JsonPropertyName("asset_count_limit")]
    public LimitModel AssetCountLimit { get; init; }

    /// <summary>
    /// Gets the specification of the maximum file size in bytes.
    /// </summary>
    [JsonPropertyName("maximum_file_size")]
    public long? MaximumFileSize { get; init; }

    /// <summary>
    /// Gets the specification of the allowed file types.
    /// </summary>
    [JsonPropertyName("allowed_file_types")]
    public FileType AllowedFileTypes { get; init; }

    /// <summary>
    /// Gets the specification of the width limitation for the asset.
    /// </summary>
    [JsonPropertyName("image_width_limit")]
    public LimitModel ImageWidthLimit { get; init; }

    /// <summary>
    /// Gets the specification of the height limitation for the asset.
    /// </summary>
    [JsonPropertyName("image_height_limit")]
    public LimitModel ImageHeightLimit { get; init; }

    /// <summary>
    /// Specifies the default value for the element value.
    /// </summary>
    [JsonPropertyName("default")]
    public AssetDefaultValueModel DefaultValue { get; init; }

    /// <summary>
    /// Gets the element's type.
    /// </summary>
    [JsonPropertyName("type")]
    public override ElementMetadataType Type => ElementMetadataType.Asset;
}
