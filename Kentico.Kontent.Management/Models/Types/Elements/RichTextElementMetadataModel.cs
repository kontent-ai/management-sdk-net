using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    public class RichTextElementMetadataModel : ElementMetadataBase
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_required")]
        public bool IsRequired { get; set; }

        [JsonProperty("guidelines")]
        public string Guidelines { get; set; }

        [JsonProperty("maximum_text_length")]
        public MaximumTextLengthModel MaximumTextLength { get; set; }

        [JsonProperty("maximum_image_size")]
        public long? MaximumImageSize { get; set; }

        [JsonProperty("allowed_content_types")]
        public IEnumerable<ObjectIdentifier> AllowedTypes { get; set; }

        [JsonProperty("image_width_limit")]
        public LimitModel ImageWidth { get; set; }

        [JsonProperty("image_height_limit")]
        public LimitModel ImageHeight { get; set; }

        [JsonProperty("allowed_image_types")]
        public FileType AllowedImageTypes { get; set; }

        [JsonProperty("allowed_blocks")]
        public ISet<RichTextBlockType> AllowedBlocks { get; set; }

        [JsonProperty("allowed_formatting")]
        public ISet<RichTextFormattingType> AllowedFormatting { get; set; }

        [JsonProperty("allowed_text_blocks")]
        public ISet<RichTextTextBlockType> AllowedTextBlocks { get; set; }

        [JsonProperty("allowed_table_blocks")]
        public ISet<RichTextTableBlockType> AllowedTableBlocks { get; set; }

        [JsonProperty("allowed_table_formatting")]
        public ISet<RichTextFormattingType> AllowedTableFormatting { get; set; }

        [JsonProperty("allowed_table_text_blocks")]
        public ISet<RichTextTextBlockType> AllowedTableTextBlocks { get; set; }

        public override ElementMetadataType Type => ElementMetadataType.RichText;
    }
}
