using Kentico.Kontent.Management.Models.Items.Identifiers;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.TaxonomyGroups.Patch
{
    public abstract class TaxonomyGroupOperationBaseModel
    {
        [JsonProperty("op", Required = Required.Always)]
        public abstract string Op { get; }

        [JsonProperty("reference", Required = Required.Always)]
        public Identifier Reference { get; set; }

    }
}
