using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    /// <summary>
    /// Represents asset element in type.
    /// </summary>
    public class AssetElementMetadataModel : ElementMetadataBase
    {
        /// <summary>
        /// Gets or sets the element's display name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Determines whether the element must be filled in.
        /// </summary>
        [JsonProperty("is_required")]
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the element's guidelines.
        /// Guidelines are used to providing instructions on what to fill in.
        /// </summary>
        [JsonProperty("guidelines")]
        public string Guidelines { get; set; }

        /// <summary>
        /// Gets or sets the specification of the limitation for the number of assets allowed within the element.
        /// </summary>
        [JsonProperty("asset_count_limit")]
        public LimitModel AssetCountLimit { get; set; }

        /// <summary>
        /// Gets or sets the specification of the maximum file size in bytes.
        /// </summary>
        [JsonProperty("maximum_file_size")]
        public long? MaximumFileSize { get; set; }

        /// <summary>
        /// Gets or sets the specification of the allowed file types.
        /// </summary>
        [JsonProperty("allowed_file_types")]
        public FileType AllowedFileTypes { get; set; }

        /// <summary>
        /// Gets or sets the specification of the width limitation for the asset.
        /// </summary>
        [JsonProperty("image_width_limit")]
        public LimitModel ImageWidthLimit { get; set; }

        /// <summary>
        /// Gets or sets the specification of the height limitation for the asset.
        /// </summary>
        [JsonProperty("image_height_limit")]
        public LimitModel ImageHeightLimit { get; set; }

        /// <summary>
        /// Gets the element's type.
        /// </summary>
        public override ElementMetadataType Type => ElementMetadataType.Asset;
    }
}
