using Kentico.Kontent.Management.Modules.ActionInvoker;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.AssetRenditions
{
    [JsonConverter(typeof(ImageTransformationConverter))]
    public abstract class ImageTransformation
    {
        [JsonProperty("mode", Required = Required.Always)]
        public abstract ImageTransformationMode Mode { get; }
    }
}