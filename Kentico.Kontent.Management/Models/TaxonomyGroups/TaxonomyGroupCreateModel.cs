using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.TaxonomyGroups
{
    public class TaxonomyGroupCreateModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("codename")]
        public string CodeName { get; set; }

        [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExternalId { get; set; }

        [JsonProperty("terms")]
        public IEnumerable<TaxonomyGroupCreateModel> Terms { get; set; }
    }
}
