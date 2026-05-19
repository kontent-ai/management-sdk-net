using Kontent.Ai.Management.Models.Shared;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.PreviewConfiguration;

/// <summary>
/// Represents space domain model.
/// </summary>
public sealed record SpaceDomainModel
{
    /// <summary>
    /// Gets the space reference.
    /// </summary>
    [JsonPropertyName("space")]
    public Reference Space { get; init; }

    /// <summary>
    /// Gets the space domain.
    /// </summary>
    [JsonPropertyName("domain")]
    public string Domain { get; init; }
}