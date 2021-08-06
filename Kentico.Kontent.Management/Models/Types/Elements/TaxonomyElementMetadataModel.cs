using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    public class TaxonomyElementMetadataModel : ElementMetadataBase
    {
        [JsonProperty("is_required")]
        public bool IsRequired { get; set; }

        [JsonProperty("guidelines")]
        public string Guidelines { get; set; }

        [JsonProperty("taxonomy_group")]
        public ObjectIdentifier TaxonomyGroup { get; set; }

        [JsonProperty("term_count_limit")]
        public LimitModel TermCountLimit { get; set; }

        public override ElementMetadataType Type => ElementMetadataType.Taxonomy;
    }
}
