namespace Kontent.Ai.Management.Models.Environments;

/// <summary>
/// Represents environment model.
/// </summary>
public sealed record EnvironmentModel
{
    /// <summary>
    /// Gets the id of the environment
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Gets the name of the environment
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Gets a flag determining whether the environment is production
    /// </summary>
    [JsonPropertyName("is_production")]
    public required bool IsProduction { get; init; }
}
