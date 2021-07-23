using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    public class TextElementMetadataModel : ElementMetadataBase
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_required")]
        public bool IsRequired { get; set; }

        [JsonProperty("guidelines")]
        public string Guidelines { get; set; }

        [JsonProperty("maximum_text_length")]
        public MaximumTextLengthModel MaximumTextLength { get; set; }
    }
}
