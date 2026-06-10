namespace Kontent.Ai.Management.Models.Content;

/// <summary>
/// Reference to an asset rendition. Identified by <see cref="Id"/> or <see cref="ExternalId"/> (either-or) — renditions
/// have no codename. <see cref="ExternalId"/> is write-only (never returned on reads).
/// </summary>
public sealed record RenditionReference
{
    /// <summary>Rendition identifier (GUID). Populated on inbound responses.</summary>
    [JsonPropertyName("id")]
    public Guid? Id { get; init; }

    /// <summary>External identifier set when the rendition was created. Write-only; must not contain <c>/</c>, <c>.</c>, or <c>;</c>.</summary>
    [JsonPropertyName("external_id")]
    public string? ExternalId { get; init; }
}
