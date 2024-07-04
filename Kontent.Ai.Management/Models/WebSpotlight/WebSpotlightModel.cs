using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.WebSpotlight;

/// <summary>
/// Represents the web spotlight model.
/// </summary>
public class WebSpotlightModel
{
    /// <summary>
    /// Gets or sets the web spotlight's Enabled.
    /// </summary>
    [JsonProperty("enabled")]
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the web spotlight's Root Type ID.
    /// </summary>
    [JsonProperty("root_type_id")]
    public Guid? RootTypeId { get; set; }
}