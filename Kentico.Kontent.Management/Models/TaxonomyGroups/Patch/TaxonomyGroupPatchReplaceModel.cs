using Kentico.Kontent.Management.Models.Items.Identifiers;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.TaxonomyGroups.Patch
{
    public class TaxonomyGroupPatchReplaceModel : TaxonomyGroupOperationBaseModel
    {
        public override string Op => "replace";

        [JsonProperty("property_name")]
        public PropertyName PropertyName { get; set; }

        [JsonProperty("value")]
        public dynamic Value { get; set; }
    }
}
