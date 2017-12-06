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
        /// Gets or sets description fo the asset.
        /// </summary>
        [JsonProperty("descriptions", Required = Required.Always)]
        public IEnumerable<AssetDescriptionsModel> Descriptions { get; set; }
    }
}
