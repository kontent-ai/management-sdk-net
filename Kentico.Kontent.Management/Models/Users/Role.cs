using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Users
{
    /// <summary>
    /// Represents user's role
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Gets or sets id of user collection
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets reference to languages
        /// </summary>
        [JsonProperty("languages")]
        public IEnumerable<Reference> Languages { get; set; }
    }
}