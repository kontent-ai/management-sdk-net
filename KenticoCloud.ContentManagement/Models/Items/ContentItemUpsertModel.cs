using System.Collections.Generic;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Items
{
    public sealed class ContentItemUpsertModel
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("type")]
        public ContentTypeIdentifier Type { get; set; }

        [JsonProperty("sitemap_locations")]
        public IEnumerable<SitemapNodeIdentifier> SitemapLocations { get; set; }
    }
}
