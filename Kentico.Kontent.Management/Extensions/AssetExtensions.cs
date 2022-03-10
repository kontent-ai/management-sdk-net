using Kentico.Kontent.Management.Models.Assets;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Extensions
{
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
        public static AssetFolderHierarchy GetFolderHierarchyById(this IEnumerable<AssetFolderHierarchy> folders, string folderId)
        {
            if (folders == null)
                return null;

            // Recursively search for the folder hierarchy that an asset is in. Returns null if file is not in a folder.
            foreach (var itm in folders)
            {
                if (itm.Id == folderId)
                {
                    return itm;
                }
                else if (itm.Folders != null)
                {
                    var nestedFolder = itm.Folders.GetFolderHierarchyById(folderId);
                    if (nestedFolder != null) //This is required so you don't stop processing if the root contains many folders (let the above foreach loop continue)
                        return nestedFolder;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets folder hierarchy for a given folder Id
        /// </summary>
        /// <param name="folders">The <see cref="AssetFoldersModel.Folders"/> property retrieved from the <see cref="IManagementClient.GetAssetFoldersAsync"/> method.</param>
        /// <param name="externalId">Folder externalId</param>
        /// <returns>The <see cref="AssetFolderHierarchy"/> instance that represents the folder found for a given folderId. Null if not found.</returns>
        public static AssetFolderHierarchy GetFolderHierarchyByExternalId(this IEnumerable<AssetFolderHierarchy> folders, string externalId)
        {
            if (folders == null)
                return null;

            // Recursively search for the folder hierarchy that an asset is in. Returns null if file is not in a folder.
            foreach (var itm in folders)
            {
                if (itm.ExternalId == externalId)
                {
                    return itm;
                }
                else if (itm.Folders != null)
                {
                    var nestedFolder = itm.Folders.GetFolderHierarchyByExternalId(externalId);
                    if (nestedFolder != null) //This is required so you don't stop processing if the root contains many folders (let the above foreach loop continue)
                        return nestedFolder;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the full folder path string
        /// </summary>
        /// <param name="folder">The <see cref="AssetFolderLinkingHierarchy"/> instance.</param>
        /// <returns>Folder path string containing backslashes (\)</returns>
        public static string GetFullFolderPath(this AssetFolderLinkingHierarchy folder)
        {
            List<string> folderName = new List<string>();
            if (folder.Parent != null)
            {
                folderName.Add(GetFullFolderPath(folder.Parent));
            }
            folderName.Add(folder.Name);
            return string.Join("\\", folderName);
        }

        /// <summary>
        /// Gets the folder hierarchy for a given folder identifier.
        /// To use this method first convert your <see cref="AssetFoldersModel.Folders"/> property retrieved from <see cref="IManagementClient.GetAssetFoldersAsync"/> to a <see cref="IEnumerable{AssetFolderLinkingHierarchy}">IEnumerable&lt;AssetFolderLinkingHierarchy&gt;</see> by using the <see cref="GetParentLinkedFolderHierarchy"/> method.
        /// </summary>
        /// <param name="folders">The <see cref="IEnumerable{AssetFolderLinkingHierarchy}"/> instance.</param>
        /// <param name="folderId">Folder Identifier</param>
        /// <returns>Returns the <see cref="AssetFolderLinkingHierarchy"/> instance found via a given folder identifier.</returns>
        public static AssetFolderLinkingHierarchy GetParentLinkedFolderHierarchyById(this IEnumerable<AssetFolderLinkingHierarchy> folders, string folderId)
        {
            if (folders != null)
            {
                foreach (var folder in folders)
                {
                    if (folder.Id == folderId)
                    {
                        return folder;
                    }
                    else if (folder.Folders != null)
                    {
                        var nestedFolder = folder.Folders.GetParentLinkedFolderHierarchyById(folderId);
                        if (nestedFolder != null) // This is required so you don't stop processing if the root contains many folders (let the above for-each loop continue)
                        {
                            return nestedFolder;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the folder hierarchy for a given folder identifier.
        /// To use this method first convert your <see cref="AssetFoldersModel.Folders"/> property retrieved from <see cref="IManagementClient.GetAssetFoldersAsync"/> to a <see cref="IEnumerable{AssetFolderLinkingHierarchy}">IEnumerable&lt;AssetFolderLinkingHierarchy&gt;</see> by using the <see cref="GetParentLinkedFolderHierarchy"/> method.
        /// </summary>
        /// <param name="folders">The <see cref="IEnumerable{AssetFolderLinkingHierarchy}"/> instance.</param>
        /// <param name="externalId">Folder external id</param>
        /// <returns>Returns the <see cref="AssetFolderLinkingHierarchy"/> instance found via a given folder identifier.</returns>
        public static AssetFolderLinkingHierarchy GetParentLinkedFolderHierarchyByExternalId(this IEnumerable<AssetFolderLinkingHierarchy> folders, string externalId)
        {
            if (folders != null)
            {
                foreach (var folder in folders)
                {
                    if (folder.ExternalId == externalId)
                    {
                        return folder;
                    }
                    else if (folder.Folders != null)
                    {
                        var nestedFolder = folder.Folders.GetParentLinkedFolderHierarchyByExternalId(externalId);
                        if (nestedFolder != null) // This is required so you don't stop processing if the root contains many folders (let the above for-each loop continue)
                        {
                            return nestedFolder;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves a list of folders with the <see cref="AssetFolderLinkingHierarchy.Parent"/> property filled in.
        /// </summary>
        /// <param name="folders">The <see cref="AssetFoldersModel.Folders"/> instance that contains the entire list of folders retrieved from the <see cref="IManagementClient.GetAssetFoldersAsync"/> method.</param>
        /// <param name="parentLinked">Parent linked folder</param>
        /// <returns>A <see cref="AssetFolderLinkingHierarchy"/> containing the parent linking folder hierarchy.</returns>
        public static IEnumerable<AssetFolderLinkingHierarchy> GetParentLinkedFolderHierarchy(this IEnumerable<AssetFolderHierarchy> folders,
            AssetFolderLinkingHierarchy parentLinked = null)
        {
            // Recursively search for the folder hierarchy that an asset is in. Returns null if file is not in a folder.
            var folderList = new List<AssetFolderLinkingHierarchy>();
            foreach (var itm in folders)
            {
                var newFolder = new AssetFolderLinkingHierarchy()
                {
                    ExternalId = itm.ExternalId,
                    Folders = (itm.Folders != null) ? new List<AssetFolderLinkingHierarchy>() : null,
                    Id = itm.Id,
                    Name = itm.Name
                };
                if (itm.Folders != null)
                {
                    newFolder.Folders = itm.Folders.GetParentLinkedFolderHierarchy(newFolder);
                }
                if (parentLinked != null)
                {
                    newFolder.Parent = parentLinked;
                }
                folderList.Add(newFolder);
            }
            return folderList;
        }
    }
}
