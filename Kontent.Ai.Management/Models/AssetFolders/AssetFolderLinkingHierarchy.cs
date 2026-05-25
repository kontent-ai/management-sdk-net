using Kontent.Ai.Management.Extensions;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.AssetFolders;

/// <summary>
/// Represents the Asset Folder Hierarchy with parent folder traversal links. 
/// This class is a derivation of the <see cref="AssetFolderHierarchy"/> class. To receive an instance of this class call <see cref="AssetExtensions.GetParentLinkedFolderHierarchy"/>
/// </summary>
public sealed class AssetFolderLinkingHierarchy
{
    /// <summary>
    /// The referenced folder's ID. Not present if the asset is not in a folder. "00000000-0000-0000-0000-000000000000" means outside of any folder.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets external id of the identifier. The folder's external ID. Only present if specified when adding folders or modifying the folders collection to add new folders.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string ExternalId { get; set; }

    /// <summary>
    /// Gets or sets the folder's codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; set; }

    /// <summary>
    /// Name of the folder
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Nested folders
    /// </summary>
    [JsonPropertyName("folders")]
    public IEnumerable<AssetFolderLinkingHierarchy> Folders { get; set; }

    /// <summary>
    /// Reference to the parent folder
    /// </summary>
    [JsonIgnore()]
    public AssetFolderLinkingHierarchy Parent { get; set; }
}
