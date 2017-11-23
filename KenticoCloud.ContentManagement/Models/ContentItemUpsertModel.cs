using System.Collections.Generic;
using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement
{
    public sealed class ContentItemUpsertModel
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("type")]
        public ManageApiReference Type { get; set; }

        [JsonProperty("sitemap_locations")]
        public ICollection<ManageApiReference> SitemapLocations { get; set; }
    }
}
