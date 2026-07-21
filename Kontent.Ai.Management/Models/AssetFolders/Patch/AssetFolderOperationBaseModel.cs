
namespace Kontent.Ai.Management.Models.AssetFolders.Patch;

/// <summary>
/// Represents the operation on folders.
/// </summary>
public abstract record AssetFolderOperationBaseModel
{
    /// <summary>
    /// Gets specification of the operation to perform.
    /// </summary>
    [JsonPropertyName("op")]
    public abstract string Op { get; }

    /// <summary>
    /// Reference to an existing folder. Required for <c>remove</c> and <c>rename</c>; optional for <c>addInto</c>, where it identifies the parent folder to add into.
    /// </summary>
    [JsonPropertyName("reference")]
    public Reference? Reference { get; init; }

}
