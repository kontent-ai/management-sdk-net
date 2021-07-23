using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    public class GuidelinesElementMetadataModel : ElementMetadataBase
    {
        [JsonProperty("guidelines")]
        public string Guidelines { get; set; }
    }
}
