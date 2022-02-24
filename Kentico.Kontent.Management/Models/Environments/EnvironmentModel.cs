using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kentico.Kontent.Management.Models.Environments
{
    /// <summary>
    /// Represents environment model.
    /// </summary>
    public class EnvironmentModel
    {
        /// <summary>
        /// Gets or sets the id of the environment
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the environment
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Gets or sets a flag determining whether the environment is production
        /// </summary>
        [JsonProperty("is_production")]
        public bool IsProduction { get; set; }
    }
}
