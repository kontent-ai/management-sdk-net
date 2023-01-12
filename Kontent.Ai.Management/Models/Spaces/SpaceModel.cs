using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.Spaces;

/// <summary>
/// Represents the space model.
/// </summary>
public class SpaceModel
{
    /// <summary>
    /// Gets or sets the space's internal ID.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the space's codename.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; set; }

    /// <summary>
    /// Gets or sets the space's name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }
}