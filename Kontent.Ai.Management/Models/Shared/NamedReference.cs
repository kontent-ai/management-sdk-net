namespace Kontent.Ai.Management.Models.Shared;

/// <summary>
/// A server-returned reference to a content-model object — its ID, display name, and codename.
/// </summary>
public sealed record NamedReference
{
    /// <summary>
    /// The object's ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// The object's display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// The object's codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public required string Codename { get; init; }
}
