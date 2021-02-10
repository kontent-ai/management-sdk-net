using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Assets
{
    /// <summary>
    /// Represents the asset folder list.
    /// </summary>
    public sealed class AssetFolderList
    {
        /// <summary>
        /// Folder listing (recursive)
        /// </summary>
        [JsonProperty("folders")]
        public IEnumerable<AssetFolderHierarchy> Folders { get; set; }

        /// <summary>
        /// Gets or sets the last modified timestamp of the asset.
        /// </summary>
        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }
    }
}
