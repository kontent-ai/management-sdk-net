using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Collections
{
    public class CollectionsModel
    {
        [JsonProperty("collections")]
        public IEnumerable<CollectionModel> Collections { get; set; }

        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }
    }
}
