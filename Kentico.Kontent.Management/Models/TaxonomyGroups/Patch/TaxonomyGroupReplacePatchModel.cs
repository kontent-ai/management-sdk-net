using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.TaxonomyGroups.Patch
{
    public class TaxonomyGroupReplacePatchModel : TaxonomyGroupOperationBaseModel
    {
        public override string Op => "replace";

        [JsonProperty("property_name")]
        public PropertyName PropertyName { get; set; }

        //todo make it strongly typed
        [JsonProperty("value")]
        public dynamic Value { get; set; }
    }
}
