namespace Kontent.Ai.Management.Models.Spaces;

/// <summary>
/// Represents the space create model.
/// </summary>
public sealed record SpaceCreateModel
{
    /// <summary>
    /// Gets the space's name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the space's codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the space's root item.
    /// </summary>
    [JsonPropertyName("web_spotlight_root_item")]
    public Reference WebSpotlightRootItem { get; init; }

    /// <summary>
    /// Gets the space's collections
    /// </summary>
    [JsonPropertyName("collections")]
    public IEnumerable<Reference> Collections { get; init; }
}