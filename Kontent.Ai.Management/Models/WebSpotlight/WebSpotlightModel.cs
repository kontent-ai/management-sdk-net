namespace Kontent.Ai.Management.Models.WebSpotlight;

/// <summary>
/// Represents the web spotlight model.
/// </summary>
public sealed record WebSpotlightModel
{
    /// <summary>
    /// Gets the web spotlight's Enabled.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool Enabled { get; init; }

    /// <summary>
    /// Gets the web spotlight's Root Type ID.
    /// </summary>
    [JsonPropertyName("root_type")]
    public Guid? RootTypeId { get; init; }
}