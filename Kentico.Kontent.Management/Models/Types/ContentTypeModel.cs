using Kentico.Kontent.Management.Models.Types.Elements;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Types
{
    /// <summary>
    /// Content type.
    /// </summary>
    public class ContentTypeModel
    {
        /// <summary>
        /// Gets or sets id of the content type.
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets codename of the content type.
        /// </summary>
        [JsonProperty("codename")]
        public string Codename { get; set; }

        /// <summary>
        /// Gets or sets last modified timestamp of the content type.
        /// </summary>
        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }

        /// <summary>
        /// Gets or sets name of the content type.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets elements of the content type.
        /// </summary>
        [JsonProperty("elements")]
        public IEnumerable<ElementMetadataBase> Elements { get; set; }

        /// <summary>
        /// Gets or sets external identifier of the content type.
        /// </summary>
        [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets content groups of the content type.
        /// </summary>
        [JsonProperty("content_groups", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<ContentGroupModel> ContentGroups { get; set; }
    }
}
