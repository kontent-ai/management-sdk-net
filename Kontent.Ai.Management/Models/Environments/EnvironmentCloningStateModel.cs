using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Environments;

/// <summary>
/// Represents state of environment cloning.
/// </summary>
public class EnvironmentCloningStateModel
{
    /// <summary>
    /// Gets or sets the state of the environment cloning.
    /// </summary>
    [JsonProperty("cloning_state")]
    public CloningState CloningState { get; set; }
}
