
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
    /// New environment name.
    /// </summary>
    [JsonPropertyName("value")]
    public required string Value { get; init; }
}
