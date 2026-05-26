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
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the name of the metadata object.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the codename of the metadata object.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }
}
