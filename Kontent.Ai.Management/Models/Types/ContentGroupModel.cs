using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.Types;

/// <summary>
/// Content group.
/// </summary>
public sealed record ContentGroupModel
{
    /// <summary>
    /// Gets the id of the content group.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the name of the content group.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the codename of the content group.
    /// </summary>
    [JsonProperty("codename")]
    public string CodeName { get; init; }

    /// <summary>
    /// Gets the external identifier of the content group.
    /// </summary>
    [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string ExternalId { get; init; }
}
