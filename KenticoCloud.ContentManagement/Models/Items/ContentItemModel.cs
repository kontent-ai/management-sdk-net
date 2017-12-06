using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Items
{
    /// <summary>
    /// Represents content item model.
    /// </summary>
    public sealed class ContentItemModel
    {
        /// <summary>
        /// Gets or sets id of the content item.
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets name of the content item.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets codename of the content item.
        /// </summary>
        [JsonProperty("codename")]
        public string CodeName { get; set; }

        /// <summary>
        /// Gets or sets type of the content item.
        /// </summary>
        [JsonProperty("type")]
        public ManageApiReference Type { get; set; }

        /// <summary>
        /// Gets or sets sitemap locations of the content item.
        /// </summary>
        [JsonProperty("sitemap_locations")]
        public IEnumerable<ManageApiReference> SitemapLocations { get; set; }
        /// <summary>
        /// Gets or sets external identifier of the content item.
        /// </summary>
        [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets last modified timestamp of the content item.
        /// </summary>
        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }
    }
}
