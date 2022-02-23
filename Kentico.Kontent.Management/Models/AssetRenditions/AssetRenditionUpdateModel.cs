using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.AssetRenditions
{
    public class AssetRenditionUpdateModel
    {
        [JsonProperty("transformation", Required = Required.Always)]
        public ImageTransformation Transformation { get; set; }
    }
}