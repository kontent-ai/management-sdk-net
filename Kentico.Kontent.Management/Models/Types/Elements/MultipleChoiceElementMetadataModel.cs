using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    public class MultipleChoiceElementMetadataModel : ElementMetadataBase
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_required")]
        public bool IsRequired { get; set; }

        [JsonProperty("guidelines")]
        public string Guidelines { get; set; }

        [JsonProperty("mode")]
        public MultipleChoiceMode Mode { get; set; }

        [JsonProperty("MultipleChoiceOptionModel")]
        public MultipleChoiceOptionModel Options { get; set; }
    }
}
