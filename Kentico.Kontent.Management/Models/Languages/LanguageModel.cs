using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kentico.Kontent.Management.Models.Languages
{
    public class LanguageModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("codename")]
        public string Codename { get; set; }

        [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExternalId { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("is_default")]
        public bool IsDefault { get; set; }

        [JsonProperty("fallback_language")]
        public Reference FallbackLanguage { get; set; }


    }
}
