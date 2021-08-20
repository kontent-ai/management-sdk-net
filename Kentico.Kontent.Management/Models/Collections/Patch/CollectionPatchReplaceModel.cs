using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Collections.Patch
{
    public sealed class CollectionPatchReplaceModel : CollectionOperationBaseModel
    {
        public override string Op => "replace";

        //todo naming reference (API docs) vs CollectionIdentifier
        [JsonProperty("reference")]
        public Reference CollectionIdentifier { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("property_name")]
        public PropertyName PropertyName { get; set; }
    }
}
