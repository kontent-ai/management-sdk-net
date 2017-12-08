using System;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models
{
    /// <summary>
    /// Represents the identifier of the api object.
    /// </summary>
    public sealed class ObjectIdentifier
    {
        /// <summary>
        /// Gets or sets id of the reference.
        /// </summary>
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid Id { get; set; }
    }
}
