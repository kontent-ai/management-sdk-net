using Kentico.Kontent.Management.Modules.ActionInvoker;
using Newtonsoft.Json;
using System;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    [JsonConverter(typeof(ElementMetadataConverter))]
    public abstract class ElementMetadataBase
    {
        public ElementMetadataType Type { get; set; }

        [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExternalId { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("codename")]
        public string Codename { get; set; }

        [JsonProperty("content_group", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ObjectIdentifier ContentGroup { get; set; }
    }
}
