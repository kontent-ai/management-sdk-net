using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Webhooks.Triggers
{
    public class ManagementApiTriggerModel
    {
        [JsonProperty("type")]
        public TriggerChangeType Type => TriggerChangeType.ContentItemVariant;

        [JsonProperty("operations")]
        public IEnumerable<string> Operations { get; set; }
    }
}
