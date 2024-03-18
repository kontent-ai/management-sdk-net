using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.AssetFolders.Patch;

/// <summary>
/// Represents addInto operation to perform on the folder.
/// </summary>
public class AssetFolderAddIntoModel : AssetFolderOperationBaseModel
{
    /// <summary>
    /// Represents addInto operation.
    /// </summary>
    public override string Op => "addInto";

    /// <summary>
    /// Gets or sets the folder object you want to add.
    /// </summary>
    [JsonProperty("value")]
    public AssetFolderHierarchy Value { get; set; }

    /// <summary>
    /// Gets or sets reference of the existing folder after which you want to add the new folder.
    /// Note: The before and after properties are mutually exclusive.
    /// </summary>
    [JsonProperty("before")]
    public Reference Before { get; set; }

    /// <summary>
    /// Gets or sets reference of the existing folder after which you want to add the new folder.
    /// Note: The before and after properties are mutually exclusive.
    /// </summary>
    [JsonProperty("after")]
    public Reference After { get; set; }
}
