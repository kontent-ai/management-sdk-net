using Kentico.Kontent.Management.Models.Items.Identifiers;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.TaxonomyGroups.Patch
{
    public class TaxonomyGroupAddIntoPatchModel : TaxonomyGroupOperationBaseModel
    {
        public override string Op => "addInto";

        [JsonProperty("value")]
        public TaxonomyGroupCreateModel Value { get; set; }

        [JsonProperty("before")]
        public Identifier Before { get; set; }

        [JsonProperty("after")]
        public Identifier After { get; set; }
    }
}
