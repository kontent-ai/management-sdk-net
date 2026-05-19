using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.EnvironmentValidation;

/// <summary>
/// Represents the metadata object.
/// </summary>
public sealed record Metadata
{
    /// <summary>
    /// Gets the id of the metadata object.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the name of the metadata object.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the codename of the metadata object.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; init; }
}
