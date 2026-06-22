using Kontent.Ai.Management.Models.AssetFolders;

namespace Kontent.Ai.Management.Extensions;

/// <summary>
/// Helpful methods to make working with assets easier
/// </summary>
public static class AssetExtensions
{
    /// <summary>
    /// Gets folder hierarchy for a given folder Id
    /// </summary>
    /// <param name="folders">The <see cref="AssetFoldersModel.Folders"/> property retrieved from the <see cref="IManagementClient.GetAssetFoldersAsync"/> method.</param>
    /// <param name="folderId">Folder Identifier</param>
    /// <returns>The <see cref="AssetFolderHierarchy"/> instance that represents the folder found for a given folderId. Null if not found.</returns>
    public static AssetFolderHierarchy? GetFolderHierarchyById(this IEnumerable<AssetFolderHierarchy> folders, string folderId)
        => FindRecursive(folders, folder => folder.Id == folderId, folder => folder.Folders);

    /// <summary>
    /// Gets folder hierarchy for a given folder externalId
    /// </summary>
    /// <param name="folders">The <see cref="AssetFoldersModel.Folders"/> property retrieved from the <see cref="IManagementClient.GetAssetFoldersAsync"/> method.</param>
    /// <param name="externalId">Folder externalId</param>
    /// <returns>The <see cref="AssetFolderHierarchy"/> instance that represents the folder found for a given externalId. Null if not found.</returns>
    public static AssetFolderHierarchy? GetFolderHierarchyByExternalId(this IEnumerable<AssetFolderHierarchy> folders, string externalId)
        => FindRecursive(folders, folder => folder.ExternalId == externalId, folder => folder.Folders);

    /// <summary>
    /// Gets folder hierarchy for a given folder codename
    /// </summary>
    /// <param name="folders">The <see cref="AssetFoldersModel.Folders"/> property retrieved from the <see cref="IManagementClient.GetAssetFoldersAsync"/> method.</param>
    /// <param name="codename">Folder codename</param>
    /// <returns>The <see cref="AssetFolderHierarchy"/> instance that represents the folder found for a given folder codename. Null if not found.</returns>
    public static AssetFolderHierarchy? GetFolderHierarchyByCodename(this IEnumerable<AssetFolderHierarchy> folders, string codename)
        => FindRecursive(folders, folder => folder.Codename == codename, folder => folder.Folders);

    /// <summary>
    /// Gets the folder hierarchy for a given folder identifier.
    /// To use this method first convert your <see cref="AssetFoldersModel.Folders"/> property retrieved from <see cref="IManagementClient.GetAssetFoldersAsync"/> to a <see cref="IEnumerable{AssetFolderLinkingHierarchy}">IEnumerable&lt;AssetFolderLinkingHierarchy&gt;</see> by using the <see cref="GetParentLinkedFolderHierarchy"/> method.
    /// </summary>
    /// <param name="folders">The <see cref="IEnumerable{AssetFolderLinkingHierarchy}"/> instance.</param>
    /// <param name="folderId">Folder Identifier</param>
    /// <returns>Returns the <see cref="AssetFolderLinkingHierarchy"/> instance found via a given folder identifier.</returns>
    public static AssetFolderLinkingHierarchy? GetParentLinkedFolderHierarchyById(this IEnumerable<AssetFolderLinkingHierarchy> folders, string folderId)
        => FindRecursive(folders, folder => folder.Id == folderId, folder => folder.Folders);

    /// <summary>
    /// Gets the folder hierarchy for a given folder external id.
    /// To use this method first convert your <see cref="AssetFoldersModel.Folders"/> property retrieved from <see cref="IManagementClient.GetAssetFoldersAsync"/> to a <see cref="IEnumerable{AssetFolderLinkingHierarchy}">IEnumerable&lt;AssetFolderLinkingHierarchy&gt;</see> by using the <see cref="GetParentLinkedFolderHierarchy"/> method.
    /// </summary>
    /// <param name="folders">The <see cref="IEnumerable{AssetFolderLinkingHierarchy}"/> instance.</param>
    /// <param name="externalId">Folder external id</param>
    /// <returns>Returns the <see cref="AssetFolderLinkingHierarchy"/> instance found via a given folder external id.</returns>
    public static AssetFolderLinkingHierarchy? GetParentLinkedFolderHierarchyByExternalId(this IEnumerable<AssetFolderLinkingHierarchy> folders, string externalId)
        => FindRecursive(folders, folder => folder.ExternalId == externalId, folder => folder.Folders);

    /// <summary>
    /// Gets the folder hierarchy for a given folder codename.
    /// To use this method first convert your <see cref="AssetFoldersModel.Folders"/> property retrieved from <see cref="IManagementClient.GetAssetFoldersAsync"/> to a <see cref="IEnumerable{AssetFolderLinkingHierarchy}">IEnumerable&lt;AssetFolderLinkingHierarchy&gt;</see> by using the <see cref="GetParentLinkedFolderHierarchy"/> method.
    /// </summary>
    /// <param name="folders">The <see cref="IEnumerable{AssetFolderLinkingHierarchy}"/> instance.</param>
    /// <param name="codename">Folder codename</param>
    /// <returns>Returns the <see cref="AssetFolderLinkingHierarchy"/> instance found via a given folder codename.</returns>
    public static AssetFolderLinkingHierarchy? GetParentLinkedFolderHierarchyByCodename(this IEnumerable<AssetFolderLinkingHierarchy> folders, string codename)
        => FindRecursive(folders, folder => folder.Codename == codename, folder => folder.Folders);

    /// <summary>
    /// Gets the full folder path string
    /// </summary>
    /// <param name="folder">The <see cref="AssetFolderLinkingHierarchy"/> instance.</param>
    /// <returns>Folder path string containing backslashes (\)</returns>
    public static string GetFullFolderPath(this AssetFolderLinkingHierarchy folder)
    {
        var segments = new List<string>();
        if (folder.Parent is not null)
        {
            segments.Add(folder.Parent.GetFullFolderPath());
        }
        if (!string.IsNullOrEmpty(folder.Name))
        {
            segments.Add(folder.Name);
        }
        return string.Join("\\", segments);
    }

    /// <summary>
    /// Retrieves a list of folders with the <see cref="AssetFolderLinkingHierarchy.Parent"/> property filled in.
    /// </summary>
    /// <param name="folders">The <see cref="AssetFoldersModel.Folders"/> instance that contains the entire list of folders retrieved from the <see cref="IManagementClient.GetAssetFoldersAsync"/> method.</param>
    /// <param name="parentLinked">Parent linked folder</param>
    /// <returns>A <see cref="AssetFolderLinkingHierarchy"/> containing the parent linking folder hierarchy.</returns>
    public static IReadOnlyList<AssetFolderLinkingHierarchy> GetParentLinkedFolderHierarchy(this IEnumerable<AssetFolderHierarchy> folders,
        AssetFolderLinkingHierarchy? parentLinked = null)
    {
        var folderList = new List<AssetFolderLinkingHierarchy>();
        foreach (var folder in folders)
        {
            var newFolder = new AssetFolderLinkingHierarchy
            {
                ExternalId = folder.ExternalId,
                Id = folder.Id,
                Codename = folder.Codename,
                Name = folder.Name,
                Parent = parentLinked,
            };
            if (folder.Folders is not null)
            {
                newFolder.Folders = folder.Folders.GetParentLinkedFolderHierarchy(newFolder);
            }
            folderList.Add(newFolder);
        }
        return folderList;
    }

    private static T? FindRecursive<T>(IEnumerable<T>? folders, Func<T, bool> match, Func<T, IEnumerable<T>?> children)
        where T : class
    {
        if (folders is null)
        {
            return null;
        }

        foreach (var folder in folders)
        {
            if (match(folder))
            {
                return folder;
            }

            // Don't return early on a null nested result — sibling folders at this level still need to be searched.
            if (FindRecursive(children(folder), match, children) is { } nested)
            {
                return nested;
            }
        }

        return null;
    }
}
