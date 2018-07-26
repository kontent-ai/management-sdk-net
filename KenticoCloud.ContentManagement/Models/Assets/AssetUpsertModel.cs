using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Assets
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
    }
}
