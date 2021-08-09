using Newtonsoft.Json;
using System;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    public class MultipleChoiceOptionModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("codename")]
        public string Codename { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExternalId { get; set; }
    }
}
