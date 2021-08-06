using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Webhooks.Triggers
{
    public class DeliveryApiTriggerModel
    {
        [JsonProperty("type")]
        public TriggerChangeType Type { get; set; }

        [JsonProperty("operations")]
        public IEnumerable<string> Operations { get; set; }
    }
}
