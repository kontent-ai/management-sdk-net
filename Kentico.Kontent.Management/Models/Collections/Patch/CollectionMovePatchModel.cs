using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Collections.Patch
{
    public sealed class CollectionMovePatchModel : CollectionOperationBaseModel
    {
        public override string Op => "move";

        //todo naming reference vs CollectionIdentifier
        [JsonProperty("reference")]
        public Reference CollectionIdentifier { get; set; }

        [JsonProperty("before")]
        public Reference Before { get; set; }

        [JsonProperty("after")]
        public Reference After { get; set; }
    }
}
