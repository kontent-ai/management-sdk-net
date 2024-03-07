using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.EnvironmentReport;

/// <summary>
/// Represents information about the specified environment
/// </summary>
public sealed class Environment
{
    /// <summary>
    /// Gets or sets the id of the environment
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the project
    /// </summary>
    [JsonProperty("name")]
    public string ProjectName { get; set; }

    /// <summary>
    /// Gets or sets the environment name
    /// </summary>
    [JsonProperty("environment")]
    public string EnvironmentName { get; set; }
}
