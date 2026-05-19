using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.AssetFolders.Patch;

/// <summary>
/// Represents the operation on folders.
/// </summary>
public abstract record AssetFolderOperationBaseModel
{
    /// <summary>
    /// Gets specification of the operation to perform.
    /// </summary>
    [JsonProperty("op")]
    public abstract string Op { get; }

    /// <summary>
    /// Gets the reference to the existing folder on which the operation will be performed.
    /// </summary>
    [JsonProperty("reference")]
    public Reference Reference { get; init; }

}
