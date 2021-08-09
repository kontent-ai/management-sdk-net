using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    public class GuidelinesElementMetadataModel : ElementMetadataBase
    {
        [JsonProperty("guidelines", Required = Required.Always)]
        public string Guidelines { get; set; }


        public override ElementMetadataType Type => ElementMetadataType.Guidelines ;
    }
}
