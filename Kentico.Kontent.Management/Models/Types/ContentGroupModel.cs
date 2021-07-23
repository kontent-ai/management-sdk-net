using Newtonsoft.Json;
using System;

namespace Kentico.Kontent.Management.Models.Types
{
    /// <summary>
    /// Content group.
    /// </summary>
    public class ContentGroupModel
    {
        /// <summary>
        /// Gets or sets id of the content group.
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets name of the content group.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets codename of the content group.
        /// </summary>
        [JsonProperty("codename")]
        public string CodeName { get; set; }

        /// <summary>
        /// Gets or sets external identifier of the content group.
        /// </summary>
        [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExternalId { get; set; }
    }
}
