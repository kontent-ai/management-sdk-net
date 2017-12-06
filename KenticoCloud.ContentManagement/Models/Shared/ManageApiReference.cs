using System;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models
{
    /// <summary>
    /// Represents the identifierof the api object reference.
    /// </summary>
    public sealed class ManageApiReference
    {
        /// <summary>
        /// Gets or sets id of the reference.
        /// </summary>
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets codename of the reference.
        /// </summary>
        [JsonProperty("codename", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string CodeName { get; set; }

        /// <summary>
        /// Gets or sets external id of the reference.
        /// </summary>
        [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExternalId { get; set; }
    }
}
