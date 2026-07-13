namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// A rich text element on a content type.
/// </summary>
/// <remarks>
/// Rich text cannot be made non-localizable: the API rejects setting <see cref="ContentElementMetadataBase.IsNonLocalizable"/>
/// to <c>true</c>. The inherited property exists so the value the API returns round-trips faithfully.
/// </remarks>
public sealed record RichTextElementMetadataModel : ContentElementMetadataBase
{
    /// <summary>
    /// Display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Maximum text length (characters or words).
    /// </summary>
    [JsonPropertyName("maximum_text_length")]
    public MaximumTextLengthModel? MaximumTextLength { get; init; }

    /// <summary>
    /// Maximum image size in bytes for inline images.
    /// </summary>
    [JsonPropertyName("maximum_image_size")]
    public long? MaximumImageSize { get; init; }

    /// <summary>
    /// Content types allowed as inline components/items. Null or empty means no restriction.
    /// </summary>
    [JsonPropertyName("allowed_content_types")]
    public IReadOnlyList<Reference>? AllowedContentTypes { get; init; }

    /// <summary>
    /// Content types whose items can be linked from rich text. Null or empty means no restriction.
    /// </summary>
    [JsonPropertyName("allowed_item_link_types")]
    public IReadOnlyList<Reference>? AllowedItemLinkTypes { get; init; }

    /// <summary>
    /// Image width limit. Null means no width restriction.
    /// </summary>
    [JsonPropertyName("image_width_limit")]
    public LimitModel? ImageWidthLimit { get; init; }

    /// <summary>
    /// Image height limit. Null means no height restriction.
    /// </summary>
    [JsonPropertyName("image_height_limit")]
    public LimitModel? ImageHeightLimit { get; init; }

    /// <summary>
    /// File-type restriction for inline images.
    /// </summary>
    [JsonPropertyName("allowed_image_types")]
    public FileType? AllowedImageTypes { get; init; }

    /// <summary>
    /// Block kinds allowed inside the rich text element (text, tables, images, components/items). Null or empty allows all.
    /// </summary>
    [JsonPropertyName("allowed_blocks")]
    public IReadOnlyList<RichTextBlockType>? AllowedBlocks { get; init; }

    /// <summary>
    /// Text formatting allowed inside the rich text element. Null or empty allows all.
    /// </summary>
    [JsonPropertyName("allowed_formatting")]
    public IReadOnlyList<RichTextFormattingType>? AllowedFormatting { get; init; }

    /// <summary>
    /// Text-block kinds allowed inside the rich text element (paragraphs, headings, lists). Null or empty allows all.
    /// </summary>
    [JsonPropertyName("allowed_text_blocks")]
    public IReadOnlyList<RichTextTextBlockType>? AllowedTextBlocks { get; init; }

    /// <summary>
    /// Block kinds allowed inside tables. Use <see cref="RichTextTableBlockType.Text"/> to allow only text, or leave null/empty to allow both text and images.
    /// </summary>
    [JsonPropertyName("allowed_table_blocks")]
    public IReadOnlyList<RichTextTableBlockType>? AllowedTableBlocks { get; init; }

    /// <summary>
    /// Text formatting allowed inside tables. Null or empty allows all; use <see cref="RichTextFormattingType.Unstyled"/> for plaintext only.
    /// </summary>
    [JsonPropertyName("allowed_table_formatting")]
    public IReadOnlyList<RichTextFormattingType>? AllowedTableFormatting { get; init; }

    /// <summary>
    /// Text-block kinds allowed inside tables. Null or empty allows all.
    /// </summary>
    [JsonPropertyName("allowed_table_text_blocks")]
    public IReadOnlyList<RichTextTextBlockType>? AllowedTableTextBlocks { get; init; }

    /// <inheritdoc/>
    [JsonPropertyName("type")]
    public override ElementMetadataType Type => ElementMetadataType.RichText;
}
