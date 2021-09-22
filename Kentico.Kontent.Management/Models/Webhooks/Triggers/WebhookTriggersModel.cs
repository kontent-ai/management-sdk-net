using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Webhooks.Triggers
{
    /// <summary>
    /// Represents the specific events that trigger the webhook.
    /// </summary>
    public class WebhookTriggersModel
    {
        /// <summary>
        /// Gets or sets triggers for delivery api content changes.
        /// More info: https://docs.kontent.ai/reference/management-api-v2#section/Webhook-object
        /// </summary>
        [JsonProperty("delivery_api_content_changes")]
        public IEnumerable<DeliveryApiTriggerModel> DeliveryApiContentChanges { get; set; }

        /// <summary>
        /// Gets or sets triggers for preview delivery api content changes.
        /// More info: https://docs.kontent.ai/reference/management-api-v2#section/Webhook-object
        /// </summary>
        [JsonProperty("preview_delivery_api_content_changes")]
        public IEnumerable<DeliveryApiTriggerModel> PreviewDeliveryApiContentChanges { get; set; }

        /// <summary>
        /// Gets or sets triggers for workflow steps changes.
        /// More info: https://docs.kontent.ai/reference/management-api-v2#section/Webhook-object
        /// </summary>
        [JsonProperty("workflow_step_changes")]
        public IEnumerable<WorkflowStepTriggerModel> WorkflowStepChanges { get; set; }

        /// <summary>
        /// Gets or sets triggers for management api content changes.
        /// More info: https://docs.kontent.ai/reference/management-api-v2#section/Webhook-object
        /// </summary>
        [JsonProperty("management_api_content_changes")]
        public IEnumerable<ManagementApiTriggerModel> ManagementApiContentChanges { get; set; }
    }
}
