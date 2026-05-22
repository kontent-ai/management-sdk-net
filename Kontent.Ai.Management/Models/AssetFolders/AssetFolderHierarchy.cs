using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.AssetFolders;

/// <summary>
/// Represents the Asset Folder Hierarchy (recursive)
/// </summary>
public sealed record AssetFolderHierarchy
{
    /// <summary>
    /// The referenced folder's ID. Not present if the asset is not in a folder. "00000000-0000-0000-0000-000000000000" means outside of any folder.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; }

    /// <summary>
    /// Gets external id of the identifier. The folder's external ID. Only present if specified when adding folders or modifying the folders collection to add new folders.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string ExternalId { get; init; }

    /// <summary>
    /// Gets the codename of the folder.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the name of the folder
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets nested folders
    /// </summary>
    [JsonPropertyName("folders")]
    public IEnumerable<AssetFolderHierarchy> Folders { get; init; }
}
