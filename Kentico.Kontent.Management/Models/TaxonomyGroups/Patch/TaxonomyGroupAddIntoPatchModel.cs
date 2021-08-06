using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.TaxonomyGroups.Patch
{
    public class TaxonomyGroupAddIntoPatchModel : TaxonomyGroupOperationBaseModel
    {
        public override string Op => "addInto";

        [JsonProperty("value")]
        public TaxonomyGroupCreateModel Value { get; set; }

        [JsonProperty("before")]
        public Reference Before { get; set; }

        [JsonProperty("after")]
        public Reference After { get; set; }
    }
}
