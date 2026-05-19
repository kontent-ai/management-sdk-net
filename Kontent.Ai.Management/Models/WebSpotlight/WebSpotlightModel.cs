using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.WebSpotlight;

/// <summary>
/// Represents the web spotlight model.
/// </summary>
public sealed record WebSpotlightModel
{
    /// <summary>
    /// Gets the web spotlight's Enabled.
    /// </summary>
    [JsonProperty("enabled")]
    public bool Enabled { get; init; }

    /// <summary>
    /// Gets the web spotlight's Root Type ID.
    /// </summary>
    [JsonProperty("root_type")]
    public Guid? RootTypeId { get; init; }
}