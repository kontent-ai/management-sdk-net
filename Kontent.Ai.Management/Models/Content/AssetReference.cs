
namespace Kontent.Ai.Management.Models.Content;

/// <summary>
/// Reference to an asset for use in generated content-type records (asset elements). On outbound writes the MAPI
/// accepts any one of <see cref="Id"/>, <see cref="Codename"/>, or <see cref="ExternalId"/>; on inbound reads it
/// returns <see cref="Id"/> only. Mutual exclusion is enforced by the API, not by this type.
/// </summary>
public sealed record AssetReference
{
    /// <summary>Asset identifier (GUID). Populated on inbound responses.</summary>
    [JsonPropertyName("id")]
    public Guid? Id { get; init; }

    /// <summary>Asset codename. Optional outbound identifier.</summary>
    [JsonPropertyName("codename")]
    public string? Codename { get; init; }

    /// <summary>External identifier supplied by the consumer when the asset was created. Optional outbound identifier.</summary>
    [JsonPropertyName("external_id")]
    public string? ExternalId { get; init; }

    /// <summary>Renditions to attach to the asset reference. <c>null</c> when no rendition selection applies.</summary>
    [JsonPropertyName("renditions")]
    public IReadOnlyList<Reference>? Renditions { get; init; }
}
