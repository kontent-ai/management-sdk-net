using System.Collections.Generic;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Assets
{
    internal sealed class AssetUpsertServerModel
    {
        [JsonProperty("file_reference")]
        public FileReferenceModel FileReference { get; set; }

        [JsonProperty("descriptions", Required = Required.Always)]
        public IEnumerable<AssetDescriptionsModel> Descriptions { get; set; }

        [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExternalId { get; set; }
    }
}
