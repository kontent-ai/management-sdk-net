using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Items
{
    public sealed class ContentItemUpdateModel
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("sitemap_locations", Required = Required.Always)]
        public IEnumerable<SitemapNodeIdentifier> SitemapLocations { get; set; }

        internal ContentItemUpdateModel()
        {
        }

        internal ContentItemUpdateModel(ContentItemModel contentItem)
        {
            Name = contentItem.Name;
            SitemapLocations = contentItem.SitemapLocations.Select(s => SitemapNodeIdentifier.ById(s.Id));
        }
    }
}
