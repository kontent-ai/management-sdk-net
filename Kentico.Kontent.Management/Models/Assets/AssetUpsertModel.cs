using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Assets
{
    /// <summary>
    /// Represents an asset upsert model.
    /// </summary>
    public sealed class AssetUpsertModel
    {
        /// <summary>
        /// Gets or sets file reference for the asset.
        /// </summary>
        [JsonProperty("file_reference", Required = Required.Always)]
        public FileReference FileReference { get; set; }

        /// <summary>
        /// Gets or sets description for the asset.
        /// </summary>
        [JsonProperty("descriptions", Required = Required.Always)]
        public IEnumerable<AssetDescription> Descriptions { get; set; } = Enumerable.Empty<AssetDescription>();

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
