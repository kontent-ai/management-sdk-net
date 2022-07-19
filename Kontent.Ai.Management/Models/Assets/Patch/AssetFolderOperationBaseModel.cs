using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Assets.Patch;

/// <summary>
/// Represents the operation on folders.
/// </summary>
public abstract class AssetFolderOperationBaseModel
{
    /// <summary>
    /// Gets specification of the operation to perform.
    /// </summary>
    [JsonProperty("op", Required = Required.Always)]
    public abstract string Op { get; }

    /// <summary>
    /// Gets or sets the reference to the existing folder on which the operation will be performed.
    /// </summary>
    [JsonProperty("reference", Required = Required.Always)]
    public Reference Reference { get; set; }

}
