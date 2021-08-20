using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Collections.Patch
{
    public sealed class CollectionAddIntoPatchModel : CollectionOperationBaseModel
    {
        public override string Op => "addInto";

        [JsonProperty("value")]
        public CollectionCreateModel Value { get; set; }

        [JsonProperty("before")]
        public Reference Before { get; set; }

        [JsonProperty("after")]
        public Reference After { get; set; }
    }
}
