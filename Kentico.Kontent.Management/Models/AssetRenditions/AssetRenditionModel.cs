using System;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.AssetRenditions
{
    /// <summary>
    /// Represents asset rendition model.
    /// </summary>
    public class AssetRenditionModel
    {
        /// <summary>
        /// Gets or sets the rendition's ID.
        /// </summary>
        [JsonProperty("rendition_id")]
        public Guid RenditionId { get; set; }

        /// <summary>
        /// Gets or sets the id of the asset this rendition belongs to.
        /// </summary>
        [JsonProperty("asset_id")]
        public Guid AssetId { get; set; }

        /// <summary>
        /// Gets or sets the rendition's external ID. 
        /// </summary>
        [JsonProperty("external_id")]
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets the image transformation.
        /// The transformation specifies how to transform the original image asset.
        /// 
        /// The image area to use for the transformation is determined by the x, y, width, and height properties.
        /// The custom_width and custom_height properties set the dimensions of the output image.
        /// 
        /// The x and y coordinates define a point in the original image.
        /// The point is specified as the distance from the top-left corner of the original image asset in pixels.
        /// The whole area must be within the borders of the original image.
        /// Upscaling, that is setting the custom_width and custom_height greater than width and height, is not allowed.
        /// </summary>
        [JsonProperty("transformation")]
        public ImageTransformation Transformation { get; set; }

        /// <summary>
        /// Gets or sets the ISO-8601 formatted date/time of the last change to the rendition.
        /// </summary>
        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }
    }
}