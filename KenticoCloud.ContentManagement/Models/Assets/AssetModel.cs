using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Assets
{
    /// <summary>
    /// Represents a digital asset, such as a document or image.
    /// </summary>
    public sealed class AssetModel
    {
        /// <summary>
        /// Gets or sets the id of the asset.
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the file name of the asset.
        /// </summary>
        [JsonProperty("file_name")]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the asset size in bytes.
        /// </summary>
        [JsonProperty("size")]
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets the media type of the asset, for example "image/jpeg".
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the file reference of the the asset".
        /// </summary>
        [JsonProperty("file_reference")]
        public FileReference FileReference { get; set; }

        /// <summary>
        /// Gets or sets the descriptions of the asset.
        /// </summary>
        [JsonProperty("descriptions")]
        public IEnumerable<AssetDescription> Descriptions { get; set; }

        /// <summary>
        /// Gets or sets title for the asset.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the external id of the asset.
        /// </summary>
        [JsonProperty("external_id")]
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets the last modified timestamp of the asset.
        /// </summary>
        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }
    }
}
