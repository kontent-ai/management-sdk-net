
namespace Kontent.Ai.Management.Models.AssetFolders.Patch;

/// <summary>
/// Represents rename operation to perform on the folder.
/// </summary>
public sealed record AssetFolderRenamePatchModel : AssetFolderOperationBaseModel
{
    /// <summary>
    /// Represents the rename operation.
    /// </summary>
    public override string Op => "rename";

    /// <summary>
    /// New folder name.
    /// </summary>
    [JsonPropertyName("value")]
    public required string Value { get; init; }
}
