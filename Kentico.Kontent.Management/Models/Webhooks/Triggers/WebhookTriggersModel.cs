using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Webhooks.Triggers
{
    public class WebhookTriggersModel
    {
        [JsonProperty("delivery_api_content_changes")]
        public IEnumerable<DeliveryApiTriggerModel> DeliveryApiContentChanges { get; set; }

        [JsonProperty("preview_delivery_api_content_changes")]
        public IEnumerable<DeliveryApiTriggerModel> PreviewDeliveryApiContentChanges { get; set; }

        [JsonProperty("workflow_step_changes")]
        public IEnumerable<WorkflowStepTriggerModel> WorkflowStepChanges { get; set; }

        [JsonProperty("management_api_content_changes")]
        public IEnumerable<ManagementApiTriggerModel> ManagementApiContentChanges { get; set; }
    }
}
