using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Environments;

/// <summary>
/// Represents state of environment cloning.
/// </summary>
public sealed record EnvironmentCloningStateModel
{
    /// <summary>
    /// Gets the state of the environment cloning.
    /// </summary>
    [JsonPropertyName("cloning_state")]
    public CloningState CloningState { get; init; }
}
