using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.EnvironmentReport;

/// <summary>
/// Represents the Metadata object.
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
