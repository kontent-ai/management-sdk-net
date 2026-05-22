
namespace Kontent.Ai.Management.Models.Environments.Patch;

/// <summary>
/// Represents the rename operation.
/// </summary>
public sealed record EnvironmentRenamePatchModel : EnvironmentOperationBaseModel
{
    /// <summary>
    /// Represents the rename_environment operation.
    /// </summary>
    public override string Op => "rename_environment";

    /// <summary>
    /// Gets the environment name.
    /// </summary>
    [JsonPropertyName("value")]
    public string Value { get; init; }
}
