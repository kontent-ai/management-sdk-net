using Kentico.Kontent.Management.Modules.ActionInvoker;
using Newtonsoft.Json;
using System;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    [JsonConverter(typeof(ElementMetadataConverter))]
    public abstract class ElementMetadataBase
    {
        [JsonProperty("type", Required = Required.Always)]
        public abstract ElementMetadataType Type { get; }

        [JsonProperty("external_id")]
        public string ExternalId { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; private set; }

        [JsonProperty("codename")]
        public string Codename { get; set; }

        [JsonProperty("content_group", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ObjectIdentifier ContentGroup { get; set; }
    }
}
