﻿using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Kentico.Kontent.Management.Models.Shared;

namespace Kentico.Kontent.Management.Models.Items
{
    /// <summary>
    /// Represents content item upsert model.
    /// </summary>
    public sealed class ContentItemUpsertModel
    {
        /// <summary>
        /// Gets or sets the name of the content item.
        /// </summary>
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the codename of the content item.
        /// </summary>
        [JsonProperty("codename")]
        public string Codename { get; set; }

        /// <summary>
        /// Gets or sets the type of the content item.
        /// </summary>
        [JsonProperty("type")]
        public Reference Type { get; set; }

        /// <summary>
        /// Gets or sets sitemap locations of the content item.
        /// </summary>
        [JsonProperty("sitemap_locations", Required = Required.Always)]
        public IEnumerable<Reference> SitemapLocations { get; set; } = Enumerable.Empty<Reference>();

        /// <summary>
        /// Gets or sets the collection of the content item.
        /// </summary>
        [JsonProperty("collection")]
        public Reference Collection { get; set; }
    }
}
