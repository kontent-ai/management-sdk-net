using Newtonsoft.Json;
using System;

namespace Kentico.Kontent.Management.Models.Shared;

/// <summary>
/// Represents general identifier of object.
/// </summary>
public sealed class Reference
{
    private Reference() { }

    /// <summary>
    /// Gets the id of the identifier.
    /// </summary>
    [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Guid? Id { get; private set; }

    /// <summary>
    /// Gets the codename of the identifier.
    /// </summary>
    [JsonProperty("codename", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string Codename { get; private set; }

    /// <summary>
    /// Gets the external id of the identifier.
    /// </summary>
    [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string ExternalId { get; private set; }

    /// <summary>
    /// Creates the reference by id.
    /// </summary>
    /// <param name="id">The id of the identifier.</param>
    public static Reference ById(Guid id) => new() { Id = id };

    /// <summary>
    /// Creates the reference by codename.
    /// </summary>
    /// <param name="codename">The codename of the identifier.</param>
    public static Reference ByCodename(string codename) => new() { Codename = codename };

    /// <summary>
    /// Creates the reference by external id.
    /// </summary>
    /// <param name="externalId">The external id of the identifier.</param>
    public static Reference ByExternalId(string externalId) => new() { ExternalId = externalId };
}
