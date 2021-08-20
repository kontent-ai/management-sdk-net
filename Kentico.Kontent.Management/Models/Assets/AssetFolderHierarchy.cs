using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Assets
{
    /// <summary>
    /// Represents the Asset Folder Hiearchy (recursive)
    /// </summary>
    public sealed class AssetFolderHierarchy
    {
        /// <summary>
        /// The referenced folder's ID. Not present if the asset is not in a folder. "00000000-0000-0000-0000-000000000000" means outside of any folder.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; private set; }

        /// <summary>
        /// Gets external id of the identifier. The folder's external ID. Only present if specified when adding folders or modifying the folders collection to add new folders.
        /// </summary>
        [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExternalId { get; set; }

        /// <summary>
        /// Name of the folder
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Nested folders
        /// </summary>
        [JsonProperty("folders")]
        public IEnumerable<AssetFolderHierarchy> Folders { get; set; }
    }
}
