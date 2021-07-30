using Newtonsoft.Json;
using System;

namespace Kentico.Kontent.Management.Models.TaxonomyGroups
{
    public class TaxonomyGroupModel : TaxonomyBaseModel
    {
        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }
    }
}
