using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Items
{
    /// <summary>
    /// Represents content item update model.
    /// </summary>
    public sealed class ContentItemUpdateModel
    {
        /// <summary>
        /// Gets or sets name of the content item.
        /// </summary>
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets sitemap locations of the content item.
        /// </summary>
        [JsonProperty("sitemap_locations", Required = Required.Always)]
        public IEnumerable<SitemapNodeIdentifier> SitemapLocations { get; set; } = Enumerable.Empty<SitemapNodeIdentifier>();

        public ContentItemUpdateModel()
        {
        }

        public ContentItemUpdateModel(ContentItemModel contentItem)
        {
            Name = contentItem.Name;
            SitemapLocations = contentItem.SitemapLocations.Select(s => SitemapNodeIdentifier.ById(s.Id));
        }
    }
}
