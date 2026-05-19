using System;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Types;

/// <summary>
/// Content group.
/// </summary>
public sealed record ContentGroupModel
{
    /// <summary>
    /// Gets the id of the content group.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the name of the content group.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the codename of the content group.
    /// </summary>
    [JsonPropertyName("codename")]
    public string CodeName { get; init; }

    /// <summary>
    /// Gets the external identifier of the content group.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string ExternalId { get; init; }
}
