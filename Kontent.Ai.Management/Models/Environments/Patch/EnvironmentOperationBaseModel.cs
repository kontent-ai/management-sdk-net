
namespace Kontent.Ai.Management.Models.Environments.Patch;

/// <summary>
/// Represents the operation on environment.
/// </summary>
public abstract record EnvironmentOperationBaseModel
{
    /// <summary>
    /// Gets specification of the operation to perform.
    /// </summary>
    [JsonPropertyName("op")]
    public abstract string Op { get; }
}
