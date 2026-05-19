using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Environments;

/// <summary>
/// Represents state of environment cloning.
/// </summary>
public sealed record EnvironmentCloningStateModel
{
    /// <summary>
    /// Gets the state of the environment cloning.
    /// </summary>
    [JsonProperty("cloning_state")]
    public CloningState CloningState { get; init; }
}
