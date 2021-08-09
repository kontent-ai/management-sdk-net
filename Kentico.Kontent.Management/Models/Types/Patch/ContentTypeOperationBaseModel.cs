using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types.Patch
{
    public abstract class ContentTypeOperationBaseModel
    {
        [JsonProperty("op", Required = Required.Always)]
        public abstract string Op { get; }

        [JsonProperty("path", Required = Required.Always)]
        public string Path { get; set; }
    }
}
