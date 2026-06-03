namespace Kontent.Ai.Management.Models.EnvironmentReport;

/// <summary>
/// Identifies the environment a validation report belongs to.
/// </summary>
public sealed record Environment
{
    /// <summary>
    /// Project ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    /// <summary>
    /// Project name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string ProjectName { get; init; }

    /// <summary>
    /// Environment name.
    /// </summary>
    [JsonPropertyName("environment")]
    public required string EnvironmentName { get; init; }
}
