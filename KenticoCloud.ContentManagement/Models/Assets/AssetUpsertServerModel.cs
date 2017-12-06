using System.Collections.Generic;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Assets
{
    internal sealed class AssetUpsertServerModel
    {
        [JsonProperty("file_reference")]
        public FileReference FileReference { get; set; }

        [JsonProperty("descriptions", Required = Required.Always)]
        public IEnumerable<AssetDescription> Descriptions { get; set; }

        [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExternalId { get; set; }
    }
}
