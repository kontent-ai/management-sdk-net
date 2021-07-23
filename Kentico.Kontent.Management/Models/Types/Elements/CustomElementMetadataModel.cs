using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    public class CustomElementMetadataModel : ElementMetadataBase
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_required")]
        public bool IsRequired { get; set; }

        [JsonProperty("guidelines")]
        public string Guidelines { get; set; }

        [JsonProperty("source_url")]
        public string SourceUrl { get; set; }

        [JsonProperty("json_parameters")]
        public string JsonParameters { get; set; }

        [JsonProperty("allowed_elements")]
        public IEnumerable<ObjectIdentifier> AllowedElements { get; set; }
    }
}
