using System;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Collections;

/// <summary>
/// Represents collection model.
/// </summary>
public sealed record CollectionModel
{
    /// <summary>
    /// Gets the id of the content collection.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the name of the content collection.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the codename of the collection.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the external identifier of the content collection.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string ExternalId { get; init; }
}
