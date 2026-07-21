namespace Kontent.Ai.Management.Models.Spaces;

/// <summary>
/// Payload for creating a space.
/// </summary>
public sealed record SpaceCreateModel
{
    /// <summary>
    /// Display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Codename. Required on create — unlike most domains, the API does not auto-generate it for spaces.
    /// </summary>
    [JsonPropertyName("codename")]
    public required string Codename { get; init; }

    /// <summary>
    /// Reference to the content item that acts as the space's root for preview. Optional.
    /// </summary>
    [JsonPropertyName("root_item")]
    public Reference? RootItem { get; init; }

    /// <summary>
    /// Collections belonging to the space. Optional.
    /// </summary>
    [JsonPropertyName("collections")]
    public IReadOnlyList<Reference>? Collections { get; init; }
}
