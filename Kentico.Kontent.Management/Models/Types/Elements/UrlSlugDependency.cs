using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    public class UrlSlugDependency
    {
        [JsonProperty("snippet", NullValueHandling = NullValueHandling.Ignore)]
        public ObjectIdentifier Snippet { get; set; }

        [JsonProperty("element")]
        public ObjectIdentifier Element { get; set; }
    }
}