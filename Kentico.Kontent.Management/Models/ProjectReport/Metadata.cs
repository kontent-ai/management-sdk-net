using Newtonsoft.Json;
using System;

namespace Kentico.Kontent.Management.Models.ProjectReport
{
    /// <summary>
    /// Represents the Metadata object.
    /// </summary>
    public sealed class Metadata
    {
        /// <summary>
        /// Gets or sets the id of the metadata object.
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the metadata object.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the codename of the metadata object.
        /// </summary>
        [JsonProperty("codename")]
        public string Codename { get; set; }
    }
}
