
namespace Kontent.Ai.Management.Models.Content;

/// <summary>
/// Reference to an asset in an asset element value. On outbound writes the MAPI accepts any one of <see cref="Id"/>,
/// <see cref="Codename"/>, or <see cref="ExternalId"/>; on inbound reads it returns <see cref="Id"/> only. Mutual
/// exclusion is enforced by the API, not by this type.
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

    /// <summary>
    /// Renditions to attach. <c>null</c> omits the property (existing renditions are left unchanged); an empty list
    /// removes them; otherwise sets them (max one). When any asset in an element specifies renditions, every asset in
    /// that element must specify the property too — the SDK does not enforce this; the API does.
    /// </summary>
    [JsonPropertyName("renditions")]
    public IReadOnlyList<RenditionReference>? Renditions { get; init; }
}
