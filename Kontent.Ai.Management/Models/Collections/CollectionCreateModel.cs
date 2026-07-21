
namespace Kontent.Ai.Management.Models.Collections;

/// <summary>
/// Payload for adding a new content collection (used as the <c>value</c> of an <c>addInto</c> patch operation).
/// </summary>
public sealed record CollectionCreateModel
{
    /// <summary>
    /// Collection name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Collection codename. Auto-generated from the name when omitted.
    /// </summary>
    [JsonPropertyName("codename")]
    public string? Codename { get; init; }

    /// <summary>
    /// Caller-supplied external ID.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string? ExternalId { get; init; }
}
