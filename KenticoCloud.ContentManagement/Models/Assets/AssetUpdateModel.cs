using System.Collections.Generic;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Assets
{
    /// <summary>
    /// Represents an asset update model.
    /// </summary>
    public sealed class AssetUpdateModel
    {
        /// <summary>
        /// Gets or sets descriptions of the asset.
        /// </summary>
        [JsonProperty("descriptions", Required = Required.Always)]
        public IEnumerable<AssetDescription> Descriptions { get; set; }

        /// <summary>
        /// Gets or sets title for the asset.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
