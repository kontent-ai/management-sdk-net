using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.PreviewConfiguration;

/// <summary>
/// Represents space domain model.
/// </summary>
public sealed record SpaceDomainModel
{
    /// <summary>
    /// Gets the space reference.
    /// </summary>
    [JsonProperty("space")]
    public Reference Space { get; init; }

    /// <summary>
    /// Gets the space domain.
    /// </summary>
    [JsonProperty("domain")]
    public string Domain { get; init; }
}