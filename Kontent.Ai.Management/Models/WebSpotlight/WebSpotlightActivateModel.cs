using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.WebSpotlight;

/// <summary>
/// Represents the web spotlight activation model.
/// </summary>
public class WebSpotlightActivateModel
{
    /// <summary>
    /// Gets or sets the web spotlight's Root Type ID.
    /// </summary>
    [JsonProperty("root_type")]
    public Reference RootType { get; set; }
}