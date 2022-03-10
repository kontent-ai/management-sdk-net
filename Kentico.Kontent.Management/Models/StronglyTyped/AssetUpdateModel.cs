using System.Collections.Generic;
using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.StronglyTyped
{
    /// <summary>
    /// Represents a strongly typed asset update model.
    /// </summary>
    public sealed class AssetUpdateModel<T> where T : new()
    {
        /// <summary>
        /// Gets or sets descriptions of the asset.
        /// </summary>
        [JsonProperty("descriptions", Required = Required.Always)]
        public IEnumerable<AssetDescription> Descriptions { get; set; }

        /// <summary>
        /// Gets or sets the title for the asset.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Folder of the asset. If outside of all folders use "id" : "00000000-0000-0000-0000-000000000000".
        /// </summary>
        [JsonProperty("folder", Required = Required.Always)]
        public Reference Folder { get; set; }
        
        /// <summary>
        /// Gets or sets elements of the asset.
        /// </summary>
        [JsonProperty("elements", Required = Required.Always)]
        public T Elements { get; set; }
    }
}
