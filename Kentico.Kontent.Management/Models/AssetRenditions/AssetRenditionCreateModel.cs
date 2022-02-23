using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.AssetRenditions
{
    public class AssetRenditionCreateModel
    {
        [JsonProperty("external_id", Required = Required.Always)]
        public string ExternalId { get; set; }
        
        [JsonProperty("transformation", Required = Required.Always)]
        public ImageTransformation Transformation { get; set; }
    }
}