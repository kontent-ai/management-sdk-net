using Kentico.Kontent.Management.Models.Webhooks.Triggers;
using Newtonsoft.Json;
using System;

namespace Kentico.Kontent.Management.Models.Webhooks
{
    public class WebhookModel
    {
        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("triggers")]
        public WebhookTriggersModel Triggers { get; set; }
    }
}
