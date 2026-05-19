using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.EnvironmentReport;

/// <summary>
/// Represents information about the specified environment
/// </summary>
public sealed record Environment
{
    /// <summary>
    /// Gets the id of the environment
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; init; }

    /// <summary>
    /// Gets the name of the project
    /// </summary>
    [JsonProperty("name")]
    public string ProjectName { get; init; }

    /// <summary>
    /// Gets the environment name
    /// </summary>
    [JsonProperty("environment")]
    public string EnvironmentName { get; init; }
}
