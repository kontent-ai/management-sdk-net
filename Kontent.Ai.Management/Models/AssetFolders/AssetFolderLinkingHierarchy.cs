using Kontent.Ai.Management.Extensions;

namespace Kontent.Ai.Management.Models.AssetFolders;

/// <summary>
/// Asset folder hierarchy with parent-folder traversal links. Built client-side from <see cref="AssetFolderHierarchy"/> by <see cref="AssetExtensions.GetParentLinkedFolderHierarchy"/>; the parent links make this a mutable, self-referential tree rather than a wire DTO.
/// </summary>
public sealed class AssetFolderLinkingHierarchy
{
    /// <summary>
    /// The folder's ID. The zero Guid string means the asset sits outside any folder.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// The folder's external ID. Only present if specified when the folder was created.
    /// </summary>
    public string? ExternalId { get; set; }

    /// <summary>
    /// The folder's codename.
    /// </summary>
    public string? Codename { get; set; }

    /// <summary>
    /// The folder's name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Nested folders. Null or empty for a leaf folder.
    /// </summary>
    public IEnumerable<AssetFolderLinkingHierarchy>? Folders { get; set; }

    /// <summary>
    /// Reference to the parent folder. Null for a root folder.
    /// </summary>
    public AssetFolderLinkingHierarchy? Parent { get; set; }
}
