using System;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.AssetRenditions
{
    public class AssetRenditionModel
    {
        [JsonProperty("rendition_id")]
        public Guid RenditionId { get; set; }
		        
        [JsonProperty("asset_id")]
        public Guid AssetId { get; set; }

        [JsonProperty("external_id")]
        public string ExternalId { get; set; }
		
        [JsonProperty("transformation")]
        public ImageTransformation Transformation { get; set; }
				
        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }
    }
}