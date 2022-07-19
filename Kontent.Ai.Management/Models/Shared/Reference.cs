using Kontent.Ai.Management.Extensions;
using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.Shared;

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

    /// <summary>
    /// Transforms the dynamic object to the <see cref="Reference"/>
    /// </summary>
    public static Reference FromDynamic(dynamic source)
    {
        try
        {
            if (DynamicExtensions.HasProperty(source, "id"))
            {
                var id = source.id.GetType() == typeof(string) ? Guid.Parse(source.id) : source.id;

                return ById(id);
            }

            if (DynamicExtensions.HasProperty(source, "codename"))
            {
                return ByCodename(source.codename);
            }

            if (DynamicExtensions.HasProperty(source, "external_id"))
            {
                return ByExternalId(source.external_id);
            }
        }
        catch (Exception exception)
        {
            throw new DataMisalignedException(
                "Object could not be converted to the strongly-typed reference. Please check if it has expected properties with expected type",
                exception);
        }

        throw new DataMisalignedException("Dynamic element reference does not contain any identifier.");
    }

    /// <summary>
    /// Transforms the <see cref="Reference"/> to the dynamic object.
    /// </summary>
    public dynamic ToDynamic()
    {
        if (Id != null)
        {
            return new {
                id = Id,
            };
        }

        if (Codename != null)
        {
            return new {
                codename = Codename,
            };
        }

        if (ExternalId != null)
        {
            return new {
                external_id = ExternalId,
            };
        }

        throw new DataMisalignedException("Element reference does not contain any identifier.");
    }
}
