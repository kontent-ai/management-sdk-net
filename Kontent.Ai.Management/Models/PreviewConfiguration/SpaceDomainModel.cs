namespace Kontent.Ai.Management.Models.PreviewConfiguration;

/// <summary>
/// Maps a space to the domain its preview URLs are served from.
/// </summary>
public sealed record SpaceDomainModel
{
    /// <summary>
    /// Reference to the space.
    /// </summary>
    [JsonPropertyName("space")]
    public required Reference Space { get; init; }

    /// <summary>
    /// Domain serving the space's preview URLs.
    /// </summary>
    [JsonPropertyName("domain")]
    public required string Domain { get; init; }
}
