using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Assets
{
    public class AssetModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("file_reference")]
        public FileReferenceModel FileReference { get; set; }

        [JsonProperty("descriptions")]
        public IEnumerable<AssetDescriptionsModel> Descriptions { get; set; }

        [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExternalId { get; set; }

        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }
    }
}
