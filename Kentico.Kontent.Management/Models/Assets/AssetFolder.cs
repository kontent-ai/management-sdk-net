using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Assets;

/// <summary>
/// Represents an asset folder
/// </summary>
public sealed class AssetFolder
{
    /// <summary>
    /// The referenced folder's ID. Not present if the asset is not in a folder.
    /// </summary>
    [JsonProperty("id", Required = Required.AllowNull)]
    public string Id { get; set; }
}
