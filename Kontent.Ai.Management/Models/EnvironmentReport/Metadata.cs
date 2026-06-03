namespace Kontent.Ai.Management.Models.EnvironmentReport;

/// <summary>
/// Represents the Metadata object.
/// </summary>
public sealed record Metadata
{
    /// <summary>
    /// Gets the id of the metadata object.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Gets the name of the metadata object.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Gets the codename of the metadata object.
    /// </summary>
    [JsonPropertyName("codename")]
    public required string Codename { get; init; }
}
