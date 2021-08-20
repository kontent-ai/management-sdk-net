using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Collections.Patch
{
    public abstract class CollectionOperationBaseModel
    {
        [JsonProperty("op", Required = Required.Always)]
        public abstract string Op { get; }
    }
}
