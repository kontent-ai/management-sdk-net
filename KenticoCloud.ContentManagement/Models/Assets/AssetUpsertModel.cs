using Newtonsoft.Json;

using System.Collections.Generic;

namespace KenticoCloud.ContentManagement.Models.Assets
{
    public sealed class AssetUpsertModel
    {
        [JsonProperty("file_reference")]
        public FileReferenceModel FileReference { get; set; }

        [JsonProperty("descriptions", Required = Required.Always)]
        public IEnumerable<AssetDescriptionsModel> Descriptions { get; set; }

    }
}
