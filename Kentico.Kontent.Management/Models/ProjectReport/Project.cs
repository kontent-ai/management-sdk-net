using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.ProjectReport
{
    /// <summary>
    /// Represents information about specified project
    /// </summary>
    public sealed class Project
    {
        /// <summary>
        /// Gets or sets id of the project
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets name of the project
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets environment of the project
        /// </summary>
        [JsonProperty("environment")]
        public string Environment { get; set; }
    }
}
