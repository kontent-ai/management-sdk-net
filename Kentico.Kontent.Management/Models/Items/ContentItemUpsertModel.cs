using System.Linq;
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
        /// Gets or sets name of the content item.
        /// </summary>
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets codename of the content item.
        /// </summary>
        [JsonProperty("codename")]
        public string Codename { get; set; }

        /// <summary>
        /// Gets or sets type of the content item.
        /// </summary>
        [JsonProperty("type")]
        public Reference Type { get; set; }

        /// <summary>
        /// Gets or sets sitemap locations of the content item.
        /// </summary>
        [JsonProperty("sitemap_locations", Required = Required.Always)]
        public IEnumerable<NoExternalIdIdentifier> SitemapLocations { get; set; } = Enumerable.Empty<NoExternalIdIdentifier>();

        /// <summary>
        /// Gets or sets collection of the content item.
        /// </summary>
        [JsonProperty("collection")]
        public NoExternalIdIdentifier Collection { get; set; }
    }
}
