using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    public class LinkedItemsElementMetadataModel : ElementMetadataBase
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_required")]
        public bool IsRequired { get; set; }

        [JsonProperty("guidelines")]
        public string Guidelines { get; set; }

        [JsonProperty("item_count_limit")]
        public LimitModel ItemCountLimit { get; set; }

        [JsonProperty("allowed_content_types")]
        public IEnumerable<ObjectIdentifier> AllowedTypes { get; set; }
    }
}
