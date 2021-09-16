using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Webhooks.Triggers
{
    /// <summary>
    /// Represents the delivery api trigger model.
    /// </summary>
    public class DeliveryApiTriggerModel
    {
        /// <summary>
        /// Gets or sets content types for which the webhook should be triggered.
        /// More info: https://docs.kontent.ai/reference/management-api-v2#section/Webhook-object
        /// </summary>
        [JsonProperty("type")]
        public TriggerChangeType Type { get; set; }

        /// <summary>
        /// Gets or sets operations for which the webhook should be triggered.
        /// More info: https://docs.kontent.ai/reference/management-api-v2#section/Webhook-object
        /// </summary>
        [JsonProperty("operations")]
        public IEnumerable<string> Operations { get; set; }
    }
}
