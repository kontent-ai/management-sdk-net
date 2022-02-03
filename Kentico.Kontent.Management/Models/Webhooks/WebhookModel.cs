using Kentico.Kontent.Management.Models.Webhooks.Triggers;
using Newtonsoft.Json;
using System;

namespace Kentico.Kontent.Management.Models.Webhooks
{
    /// <summary>
    /// Represents the webhook model.
    /// </summary>
    public class WebhookModel
    {
        /// <summary>
        /// Gets or sets ISO-8601 formatted date/time of the last change to the webhook.
        /// </summary>
        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }

        /// <summary>
        /// Gets or sets the webhook's internal ID.
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the webhook's display name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the URL to which the webhook notification will be sent.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the webhook's secret key, used to authenticate that the webhook was sent by Kentico Kontent.
        /// </summary>
        [JsonProperty("secret")]
        public string Secret { get; set; }

        /// <summary>
        /// Determines if the webhook is enabled. By default, the enabled property is set to true.
        /// More info: https://kontent.ai/learn/reference/management-api-v2#section/Webhook-object
        /// </summary>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the specific events that trigger the webhook. At least one valid trigger is required.
        /// </summary>
        [JsonProperty("triggers")]
        public WebhookTriggersModel Triggers { get; set; }
    }
}
