using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.ProjectReport
{
    /// <summary>
    /// Represents information about the specified project
    /// </summary>
    public sealed class Project
    {
        /// <summary>
        /// Gets or sets the id of the project
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the project
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the environment of the project
        /// </summary>
        [JsonProperty("environment")]
        public string Environment { get; set; }
    }
}
