using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Languages
{
    public class LanguageCreateModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("codename")]
        public string Codename { get; set; }

        [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExternalId { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("fallback_language")]
        public Reference FallbackLanguage { get; set; }
    }
}
