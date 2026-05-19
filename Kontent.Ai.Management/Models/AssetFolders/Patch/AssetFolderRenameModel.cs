using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.AssetFolders.Patch;

/// <summary>
/// Represents rename operation to perform on the folder.
/// </summary>
public sealed record AssetFolderRenameModel : AssetFolderOperationBaseModel
{
    /// <summary>
    /// Represents the rename operation.
    /// </summary>
    public override string Op => "rename";

    /// <summary>
    /// Gets the reference to the folder to be renamed.
    /// </summary>
    [JsonPropertyName("value")]
    public string Value { get; init; }
}
