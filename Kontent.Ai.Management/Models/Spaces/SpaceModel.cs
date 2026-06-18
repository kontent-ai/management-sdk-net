namespace Kontent.Ai.Management.Models.Spaces;

/// <summary>
/// A space (response shape).
/// </summary>
public sealed record SpaceModel
{
    /// <summary>
    /// Server-generated space ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public required string Codename { get; init; }

    /// <summary>
    /// Display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Reference to the content item that acts as the space's root for preview. Null when no root item is set.
    /// </summary>
    [JsonPropertyName("root_item")]
    public Reference? RootItem { get; init; }

    /// <summary>
    /// Collections belonging to the space. Always present; may be empty.
    /// </summary>
    [JsonPropertyName("collections")]
    public required IReadOnlyList<Reference> Collections { get; init; }
}
