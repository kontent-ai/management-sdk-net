using Kontent.Ai.Management.Models.Shared;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.AssetFolders.Patch;

/// <summary>
/// Represents addInto operation to perform on the folder.
/// </summary>
public sealed record AssetFolderAddIntoModel : AssetFolderOperationBaseModel
{
    /// <summary>
    /// Represents addInto operation.
    /// </summary>
    public override string Op => "addInto";

    /// <summary>
    /// Gets the folder object you want to add.
    /// </summary>
    [JsonPropertyName("value")]
    public AssetFolderHierarchy Value { get; init; }

    /// <summary>
    /// Gets reference of the existing folder after which you want to add the new folder.
    /// Note: The before and after properties are mutually exclusive.
    /// </summary>
    [JsonPropertyName("before")]
    public Reference Before { get; init; }

    /// <summary>
    /// Gets reference of the existing folder after which you want to add the new folder.
    /// Note: The before and after properties are mutually exclusive.
    /// </summary>
    [JsonPropertyName("after")]
    public Reference After { get; init; }
}
