using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.Environments;

/// <summary>
/// Represents environment model.
/// </summary>
public sealed record EnvironmentModel
{
    /// <summary>
    /// Gets the id of the environment
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the name of the environment
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets a flag determining whether the environment is production
    /// </summary>
    [JsonProperty("is_production")]
    public bool IsProduction { get; init; }
}
