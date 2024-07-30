using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.AssetFolders;

/// <summary>
/// Represents the Asset Folder Hierarchy (recursive)
/// </summary>
public sealed class AssetFolderHierarchy
{
    /// <summary>
    /// The referenced folder's ID. Not present if the asset is not in a folder. "00000000-0000-0000-0000-000000000000" means outside of any folder.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; private set; }

    /// <summary>
    /// Gets external id of the identifier. The folder's external ID. Only present if specified when adding folders or modifying the folders collection to add new folders.
    /// </summary>
    [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string ExternalId { get; set; }
    
    /// <summary>
    /// Gets or sets the codename of the folder.
    /// </summary>
    [JsonProperty("codename", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string Codename { get; set; }

    /// <summary>
    /// Gets or sets the name of the folder
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets nested folders
    /// </summary>
    [JsonProperty("folders")]
    public IEnumerable<AssetFolderHierarchy> Folders { get; set; }
}
