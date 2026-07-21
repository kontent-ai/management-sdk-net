namespace Kontent.Ai.Management.Models.Shared;

/// <summary>
/// Represents general identifier of object. Construction is factory-only (<see cref="ById"/> / <see cref="ByCodename"/> /
/// <see cref="ByExternalId"/>) so exactly one identifier is ever set — the invalid zero/multiple-identifier states the
/// MAPI rejects are unrepresentable.
/// </summary>
public sealed record Reference
{
    private Reference() { }

    /// <summary>
    /// Gets the id of the identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid? Id { get; private init; }

    /// <summary>
    /// Gets the codename of the identifier; <c>null</c> unless this reference was created by codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public string? Codename { get; private init; }

    /// <summary>
    /// Gets the external id of the identifier; <c>null</c> unless this reference was created by external id.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string? ExternalId { get; private init; }

    /// <summary>
    /// Creates the reference by id.
    /// </summary>
    /// <param name="id">The id of the identifier.</param>
    public static Reference ById(Guid id) => new() { Id = id };

    /// <summary>
    /// Creates the reference by codename.
    /// </summary>
    /// <param name="codename">The codename of the identifier; must be non-empty.</param>
    public static Reference ByCodename(string codename)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(codename);

        return new() { Codename = codename };
    }

    /// <summary>
    /// Creates the reference by external id.
    /// </summary>
    /// <param name="externalId">The external id of the identifier; must be non-empty.</param>
    public static Reference ByExternalId(string externalId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(externalId);

        return new() { ExternalId = externalId };
    }

    /// <summary>
    /// Creates a reference to the default object by id — the zero GUID (<see cref="Guid.Empty"/>), which the MAPI
    /// treats as the default (e.g. the default language / variant).
    /// </summary>
    public static Reference ByDefaultId() => ById(Guid.Empty);

    /// <summary>
    /// Creates a reference to the default object by the reserved <c>"default"</c> codename (e.g. the default language / variant).
    /// </summary>
    public static Reference ByDefaultCodename() => ByCodename("default");
}
