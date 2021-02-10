using System.Collections.Generic;

using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Assets
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

        /// <summary>
        /// Folder of the asset. If outside of all folders use "id" : "00000000-0000-0000-0000-000000000000".
        /// </summary>
        [JsonProperty("folder", Required = Required.Always)]
        public AssetFolderIdentifier Folder { get; set; }
    }
}
