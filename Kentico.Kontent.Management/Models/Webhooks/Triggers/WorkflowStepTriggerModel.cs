using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Webhooks.Triggers
{
    public class WorkflowStepTriggerModel
    {
        [JsonProperty("type")]
        public TriggerChangeType Type => TriggerChangeType.ContentItemVariant;

        [JsonProperty("transitions_to")]
        public IEnumerable<Reference> TransitionsTo { get; set; }
    }
}
