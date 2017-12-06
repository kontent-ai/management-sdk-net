using System.Collections.Generic;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Items
{
    /// <summary>
    /// Represents content item create model.
    /// </summary>
    public sealed class ContentItemCreateModel
    {
        /// <summary>
        /// Gets or sets name of the content item.
        /// </summary>
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets type of the content item.
        /// </summary>
        [JsonProperty("type", Required = Required.Always)]
        public ContentTypeIdentifier Type { get; set; }

        /// <summary>
        /// Gets or sets sitemap locations of the content item.
        /// </summary>
        [JsonProperty("sitemap_locations")]
        public ICollection<SitemapNodeIdentifier> SitemapLocations { get; set; }

        /// <summary>
        /// Gets or sets exernal identifier of the content item.
        /// </summary>
        [JsonProperty("external_id")]
        public string ExternalId { get; set; }
    }
}
