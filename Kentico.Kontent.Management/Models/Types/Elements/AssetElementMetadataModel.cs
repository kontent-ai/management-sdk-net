using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    public class AssetElementMetadataModel : ElementMetadataBase
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_required")]
        public bool IsRequired { get; set; }

        [JsonProperty("guidelines")]
        public string Guidelines { get; set; }

        [JsonProperty("asset_count_limit")]
        public LimitModel AssetCountLimit { get; set; }

        [JsonProperty("maximum_file_size")]
        public long? MaximumFileSize { get; set; }

        [JsonProperty("allowed_file_types")]
        public FileType AllowedFileTypes { get; set; }

        [JsonProperty("image_width_limit")]
        public LimitModel ImageWidthLimit { get; set; }

        [JsonProperty("image_height_limit")]
        public LimitModel ImageHeightLimit { get; set; }

        public override ElementMetadataType Type => ElementMetadataType.Asset;
    }
}
