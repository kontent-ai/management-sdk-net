using Kontent.Ai.Management.Models.Shared;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.WebSpotlight;

/// <summary>
/// Represents the web spotlight activation model.
/// </summary>
public sealed record WebSpotlightActivateModel
{
    /// <summary>
    /// Gets the web spotlight's Root Type ID.
    /// </summary>
    [JsonPropertyName("root_type")]
    public Reference RootType { get; init; }
}