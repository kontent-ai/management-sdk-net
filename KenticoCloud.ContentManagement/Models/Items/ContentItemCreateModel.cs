using System.Collections.Generic;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Items
{
    public sealed class ContentItemCreateModel
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("type", Required = Required.Always)]
        public ContentTypeIdentifier Type { get; set; }

        [JsonProperty("sitemap_locations")]
        public ICollection<SitemapNodeIdentifier> SitemapLocations { get; set; }

        [JsonProperty("external_id")]
        public string ExternalId { get; set; }
    }
}
