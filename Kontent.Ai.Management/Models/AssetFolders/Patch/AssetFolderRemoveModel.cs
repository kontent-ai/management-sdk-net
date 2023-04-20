namespace Kontent.Ai.Management.Models.AssetFolders.Patch;

/// <summary>
/// Represents remove operation to perform on the folder.
/// </summary>
public class AssetFolderRemoveModel : AssetFolderOperationBaseModel
{
    /// <summary>
    /// Represents remove operation.
    /// </summary>
    public override string Op => "remove";
}
