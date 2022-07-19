using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Environments.Patch;

/// <summary>
/// Represents the operation on environment.
/// </summary>
public abstract class EnvironmentOperationBaseModel
{
    /// <summary>
    /// Gets specification of the operation to perform.
    /// </summary>
    [JsonProperty("op")]
    public abstract string Op { get; }
}
