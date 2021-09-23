using Kentico.Kontent.Management.Models.Types.Elements;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.TypeSnippets
{
    /// <summary>
    /// Represents content snippet type model.
    /// </summary>
    public class ContentTypeSnippetModel
    {
        /// <summary>
        /// Gets or sets id of the content snippet type.
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets codename of the content snippet type.
        /// </summary>
        [JsonProperty("codename")]
        public string Codename { get; set; }

        /// <summary>
        /// Gets or sets last modified timestamp of the content snippet type.
        /// </summary>
        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }

        /// <summary>
        /// Gets or sets name of the content snippet type.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets elements of the content snippet type.
        /// </summary>
        [JsonProperty("elements")]
        public IEnumerable<ElementMetadataBase> Elements { get; set; }

        /// <summary>
        /// Gets or sets external identifier of the content snippet type.
        /// </summary>
        [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExternalId { get; set; }
    }
}
