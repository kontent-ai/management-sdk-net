using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Represents a rich text element in content types.
/// </summary>
public sealed record RichTextElementMetadataModel : ElementMetadataBase
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
    /// Specifies the maximum text length.
    /// </summary>
    [JsonProperty("maximum_text_length")]
    public MaximumTextLengthModel MaximumTextLength { get; init; }

    /// <summary>
    /// Specifies the maximum image size in bytes.
    /// </summary>
    [JsonProperty("maximum_image_size")]
    public long? MaximumImageSize { get; init; }

    /// <summary>
    /// Specifies a list of allowed content types as an array of references.
    /// </summary>
    [JsonProperty("allowed_content_types")]
    public IEnumerable<Reference> AllowedTypes { get; init; }

    /// <summary>
    /// Specifies content types of items that are allowed to be used in links as an array of references.
    /// </summary>
    [JsonProperty("allowed_item_link_types")]
    public IEnumerable<Reference> AllowedItemLinkTypes { get; init; }

    /// <summary>
    /// Specifies the width limitation for images.
    /// </summary>
    [JsonProperty("image_width_limit")]
    public LimitModel ImageWidth { get; init; }

    /// <summary>
    /// Specifies the height limitation for images.
    /// </summary>
    [JsonProperty("image_height_limit")]
    public LimitModel ImageHeight { get; init; }

    /// <summary>
    /// Specifies which image types are allowed.
    /// </summary>
    [JsonProperty("allowed_image_types")]
    public FileType AllowedImageTypes { get; init; }

    /// <summary>
    /// Specifies which blocks are allowed inside your rich text element. You can allow text, tables, images, and components and items. To allow all blocks, leave the array empty.
    /// </summary>
    [JsonProperty("allowed_blocks")]
    public ISet<RichTextBlockType> AllowedBlocks { get; init; }

    /// <summary>
    /// Specifies which text formatting is allowed inside your rich text element. To allow all formatting, leave the array empty.
    /// </summary>
    [JsonProperty("allowed_formatting")]
    public ISet<RichTextFormattingType> AllowedFormatting { get; init; }

    /// <summary>
    /// Specifies which text blocks are allowed inside your rich text element. You can allow paragraphs, headings, and lists. To allow all text blocks, leave the array empty.
    /// </summary>
    [JsonProperty("allowed_text_blocks")]
    public ISet<RichTextTextBlockType> AllowedTextBlocks { get; init; }

    /// <summary>
    /// Specifies which blocks are allowed inside tables in your rich text element. Either use <see cref="RichTextTableBlockType.Text"></see> to allow only text or leave the array empty to allow both text and images.
    /// </summary>
    [JsonProperty("allowed_table_blocks")]
    public ISet<RichTextTableBlockType> AllowedTableBlocks { get; init; }

    /// <summary>
    /// Specifies which text formatting is allowed inside tables in your rich text element.
    /// To allow all formatting, leave the array empty. To allow only plaintext, use <see cref="RichTextFormattingType.Unstyled"></see>.
    /// </summary>
    [JsonProperty("allowed_table_formatting")]
    public ISet<RichTextFormattingType> AllowedTableFormatting { get; init; }

    /// <summary>
    /// Specifies which text blocks are allowed inside tables in your rich text element. You can allow paragraphs, headings, and lists. To allow all text blocks, leave the array empty.
    /// </summary>
    [JsonProperty("allowed_table_text_blocks")]
    public ISet<RichTextTextBlockType> AllowedTableTextBlocks { get; init; }

    /// <summary>
    /// Represents the type of the content type element.
    /// </summary>
    public override ElementMetadataType Type => ElementMetadataType.RichText;
}
