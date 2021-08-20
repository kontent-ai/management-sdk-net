using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Collections.Patch
{
    public sealed class CollectionPatchRemoveModel : CollectionOperationBaseModel
    {
        public override string Op => "remove";

        //todo naming reference vs CollectionIdentifier
        [JsonProperty("reference")]
        public Reference CollectionIdentifier { get; set; }
    }
}
