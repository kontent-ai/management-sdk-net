using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Webhooks.Triggers
{
    /// <summary>
    /// Represents the workflow step trigger model.
    /// </summary>
    public class WorkflowStepTriggerModel
    {
        /// <summary>
        /// Represent content types for which the webhook should be triggered.
        /// More info: https://docs.kontent.ai/reference/management-api-v2#section/Webhook-object
        /// </summary>
        [JsonProperty("type")]
        public TriggerChangeType Type => TriggerChangeType.LanguageVariant;

        /// <summary>
        /// Gets or sets a collection of references to the workflow steps that will trigger the webhook when an item transitions to them.
        /// Workflow steps must be referenced by their internal IDs.
        /// More info: https://docs.kontent.ai/reference/management-api-v2#section/Webhook-object
        /// </summary>
        [JsonProperty("transitions_to")]
        public IEnumerable<Reference> TransitionsTo { get; set; }
    }
}
