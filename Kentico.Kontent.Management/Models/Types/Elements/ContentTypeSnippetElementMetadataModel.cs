using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    public class ContentTypeSnippetElementMetadataModel : ElementMetadataBase
    {
        [JsonProperty("snippet")]
        public ObjectIdentifier Snippet { get; set; }

        public override ElementMetadataType Type => ElementMetadataType.Snippet;
    }
}
