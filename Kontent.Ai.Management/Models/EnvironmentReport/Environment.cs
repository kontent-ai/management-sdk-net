using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.EnvironmentReport;

/// <summary>
/// Represents information about the specified environment
/// </summary>
public sealed record Environment
{
    /// <summary>
    /// Gets the id of the environment
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; }

    /// <summary>
    /// Gets the name of the project
    /// </summary>
    [JsonPropertyName("name")]
    public string ProjectName { get; init; }

    /// <summary>
    /// Gets the environment name
    /// </summary>
    [JsonPropertyName("environment")]
    public string EnvironmentName { get; init; }
}
