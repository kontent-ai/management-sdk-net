using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    /// <summary>
    /// Represents a rich text element in content types.
    /// </summary>
    public class RichTextElementMetadataModel : ElementMetadataBase
    {
        /// <summary>
        /// Gets or sets the element's display name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a flag determining whether the element must be filled in.
        /// </summary>
        [JsonProperty("is_required")]
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the element's guidelines, providing instructions on what to fill in.
        /// </summary>
        [JsonProperty("guidelines")]
        public string Guidelines { get; set; }

        /// <summary>
        /// Specifies the maximum text length.
        /// </summary>
        [JsonProperty("maximum_text_length")]
        public MaximumTextLengthModel MaximumTextLength { get; set; }

        /// <summary>
        /// Specifies the maximum image size in bytes.
        /// </summary>
        [JsonProperty("maximum_image_size")]
        public long? MaximumImageSize { get; set; }

        /// <summary>
        /// Specifies a list of allowed content types as an array of references.
        /// </summary>
        [JsonProperty("allowed_content_types")]
        public IEnumerable<ObjectIdentifier> AllowedTypes { get; set; }

        /// <summary>
        /// Specifies the width limitation for images.
        /// </summary>
        [JsonProperty("image_width_limit")]
        public LimitModel ImageWidth { get; set; }

        /// <summary>
        /// Specifies the height limitation for images.
        /// </summary>
        [JsonProperty("image_height_limit")]
        public LimitModel ImageHeight { get; set; }

        /// <summary>
        /// Specifies which image types are allowed.
        /// </summary>
        [JsonProperty("allowed_image_types")]
        public FileType AllowedImageTypes { get; set; }

        /// <summary>
        /// Specifies which blocks are allowed inside your rich text element. You can allow text, tables, images, and components and items. To allow all blocks, leave the array empty.
        /// </summary>
        [JsonProperty("allowed_blocks")]
        public ISet<RichTextBlockType> AllowedBlocks { get; set; }

        /// <summary>
        /// Specifies which text formatting is allowed inside your rich text element. To allow all formatting, leave the array empty.
        /// </summary>
        [JsonProperty("allowed_formatting")]
        public ISet<RichTextFormattingType> AllowedFormatting { get; set; }

        /// <summary>
        /// Specifies which text blocks are allowed inside your rich text element. You can allow paragraphs, headings, and lists. To allow all text blocks, leave the array empty.
        /// </summary>
        [JsonProperty("allowed_text_blocks")]
        public ISet<RichTextTextBlockType> AllowedTextBlocks { get; set; }

        /// <summary>
        /// Specifies which blocks are allowed inside tables in your rich text element. Either use <see cref="RichTextTableBlockType.Text"></see> to allow only text or leave the array empty to allow both text and images.
        /// </summary>
        [JsonProperty("allowed_table_blocks")]
        public ISet<RichTextTableBlockType> AllowedTableBlocks { get; set; }

        /// <summary>
        /// Specifies which text formatting is allowed inside tables in your rich text element.
        /// To allow all formatting, leave the array empty. To allow only plaintext, use <see cref="RichTextFormattingType.Unstyled"></see>.
        /// </summary>
        [JsonProperty("allowed_table_formatting")]
        public ISet<RichTextFormattingType> AllowedTableFormatting { get; set; }

        /// <summary>
        /// Specifies which text blocks are allowed inside tables in your rich text element. You can allow paragraphs, headings, and lists. To allow all text blocks, leave the array empty.
        /// </summary>
        [JsonProperty("allowed_table_text_blocks")]
        public ISet<RichTextTextBlockType> AllowedTableTextBlocks { get; set; }

        /// <summary>
        /// Represents type of the content type element.
        /// </summary>
        public override ElementMetadataType Type => ElementMetadataType.RichText;
    }
}
