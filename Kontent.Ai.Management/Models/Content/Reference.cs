using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Content;

/// <summary>
/// Identifier reference to any Kontent.ai object — asset, asset rendition, taxonomy term, content type, language,
/// etc. The MAPI accepts any one of <see cref="Id"/>, <see cref="Codename"/>, or <see cref="ExternalId"/> on
/// outbound writes and returns <see cref="Id"/> on inbound reads. Mutual exclusion is enforced by the API.
/// </summary>
/// <remarks>
/// STJ-formatted counterpart to the legacy <c>Kontent.Ai.Management.Models.Shared.Reference</c>; the two coexist
/// by namespace until phase 5 sunsets the legacy type. Use the <see cref="ById"/> / <see cref="ByCodename"/> /
/// <see cref="ByExternalId"/> factories for ergonomic construction.
/// </remarks>
public sealed record Reference
{
    /// <summary>Object identifier (GUID). Populated on inbound responses.</summary>
    [JsonPropertyName("id")]
    public Guid? Id { get; init; }

    /// <summary>Object codename. Optional outbound identifier.</summary>
    [JsonPropertyName("codename")]
    public string? Codename { get; init; }

    /// <summary>External identifier supplied by the consumer when the object was created. Optional outbound identifier.</summary>
    [JsonPropertyName("external_id")]
    public string? ExternalId { get; init; }

    /// <summary>Creates a reference by id.</summary>
    public static Reference ById(Guid id) => new() { Id = id };

    /// <summary>Creates a reference by codename.</summary>
    public static Reference ByCodename(string codename) => new() { Codename = codename };

    /// <summary>Creates a reference by external id.</summary>
    public static Reference ByExternalId(string externalId) => new() { ExternalId = externalId };
}
