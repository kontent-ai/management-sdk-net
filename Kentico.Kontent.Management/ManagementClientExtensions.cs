using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.Items;

namespace Kentico.Kontent.Management
{
    /// <summary>
    /// Extra simplifying methods available for ManagementClient
    /// </summary>
    public static class ManagementClientExtensions
    {
        /// <summary>
        /// Updates the given content item.
        /// </summary>
        /// <param name="client">Content management client instance.</param>
        /// <param name="identifier">Identifies which content item will be updated. </param>
        /// <param name="contentItem">Specifies data for updated content item.</param>
        /// <returns>The <see cref="ContentItemModel"/> instance that represents updated content item.</returns>
        public static async Task<ContentItemModel> UpdateContentItemAsync(this ManagementClient client, ContentItemIdentifier identifier, ContentItemModel contentItem)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            if (contentItem == null)
            {
                throw new ArgumentNullException(nameof(contentItem));
            }

            var contentItemUpdateModel = new ContentItemUpdateModel(contentItem);

            return await client.UpdateContentItemAsync(identifier, contentItemUpdateModel);
        }

        /// <summary>
        /// Creates or updates the given content item variant.
        /// </summary>
        /// <param name="client">Content management client instance.</param>
        /// <param name="identifier">Identifies which content item variant will be created or updated. </param>
        /// <param name="contentItemVariant">Specifies data for created ur updated content item variant.</param>
        /// <returns>The <see cref="ContentItemVariantModel"/> instance that represents created or updated content item variant.</returns>
        public static async Task<ContentItemVariantModel> UpsertContentItemVariantAsync(this ManagementClient client, ContentItemVariantIdentifier identifier, ContentItemVariantModel contentItemVariant)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            if (contentItemVariant == null)
            {
                throw new ArgumentNullException(nameof(contentItemVariant));
            }

            var contentItemVariantUpsertModel = new ContentItemVariantUpsertModel(contentItemVariant);

            return await client.UpsertContentItemVariantAsync(identifier, contentItemVariantUpsertModel);
        }

        /// <summary>
        /// Creates asset.
        /// </summary>
        /// <param name="client">Content management client instance.</param>
        /// <param name="fileContent">Represents the content of the file.</param>
        /// <param name="descriptions">Represents description for individual language.</param>
        /// <returns>The <see cref="AssetModel"/> instance that represents created asset.</returns>
        [Obsolete("CreateAssetAsync is deprecated, please use overloading method with fileContent and assetUpdateModel parameter.")]
        public static async Task<AssetModel> CreateAssetAsync(this ManagementClient client, FileContentSource fileContent, IEnumerable<AssetDescription> descriptions)
        {
            if (fileContent == null)
            {
                throw new ArgumentNullException(nameof(fileContent));
            }

            if (descriptions == null)
            {
                throw new ArgumentNullException(nameof(descriptions));
            }

            var fileResult = await client.UploadFileAsync(fileContent);

            var asset = new AssetUpsertModel
            {
                FileReference = fileResult,
                Descriptions = descriptions
            };

            var response = await client.CreateAssetAsync(asset);

            return response;
        }

        /// <summary>
        /// Creates asset.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="fileContent">Represents the content of the file.</param>
        /// <param name="assetUpdateModel">Updated values for asset.</param>
        public static async Task<AssetModel> CreateAssetAsync(this ManagementClient client, FileContentSource fileContent, AssetUpdateModel assetUpdateModel)
        {
            if (fileContent == null)
            {
                throw new ArgumentNullException(nameof(fileContent));
            }
            if (assetUpdateModel == null)
            {
                throw new ArgumentNullException(nameof(assetUpdateModel));
            }

            if (assetUpdateModel.Descriptions == null)
            {
                throw new ArgumentNullException(nameof(assetUpdateModel.Descriptions));
            }

            var fileResult = await client.UploadFileAsync(fileContent);

            var asset = new AssetUpsertModel
            {
                FileReference = fileResult,
                Descriptions = assetUpdateModel.Descriptions,
                Title = assetUpdateModel.Title
            };

            var response = await client.CreateAssetAsync(asset);

            return response;
        }

        /// <summary>
        /// Creates or updates the given asset.
        /// </summary>
        /// <param name="client">Content management client instance.</param>
        /// <param name="externalId">The external identifier of the asset.</param>
        /// <param name="fileContent">Represents the content of the file.</param>
        /// <param name="updatedAsset">Updated values for asset.</param>
        /// <returns>The <see cref="AssetModel"/> instance that represents created or updated asset.</returns>
        public static async Task<AssetModel> UpsertAssetByExternalIdAsync(this ManagementClient client, string externalId, FileContentSource fileContent, AssetUpdateModel updatedAsset)
        {
            if (string.IsNullOrEmpty(externalId))
            {
                throw new ArgumentException("The external id is not specified.", nameof(externalId));
            }

            if (fileContent == null)
            {
                throw new ArgumentNullException(nameof(fileContent));
            }

            if (updatedAsset == null)
            {
                throw new ArgumentNullException(nameof(updatedAsset));
            }

            if (updatedAsset.Descriptions == null)
            {
                throw new ArgumentNullException(nameof(updatedAsset.Descriptions));
            }

            var fileResult = await client.UploadFileAsync(fileContent);

            var asset = new AssetUpsertModel
            {
                FileReference = fileResult,
                Descriptions = updatedAsset.Descriptions
            };

            UpdateAssetTitle(updatedAsset, asset);

            var response = await client.UpsertAssetByExternalIdAsync(externalId, asset);

            return response;
        }

        /// <summary>
        /// Creates or updates the given asset.
        /// </summary>
        /// <param name="client">Content management client instance.</param>
        /// <param name="externalId">The external identifier of the asset.</param>
        /// <param name="fileContent">Represents the content of the file.</param>
        /// <param name="descriptions">Represents description for individual language.</param>
        /// <returns>The <see cref="AssetModel"/> instance that represents created or updated asset.</returns>
        [Obsolete("UpsertAssetByExternalIdAsync is deprecated, please use overloading method with externalId, fileContent and updatedAsset parameter.")]
        public static async Task<AssetModel> UpsertAssetByExternalIdAsync(this ManagementClient client, string externalId, FileContentSource fileContent, IEnumerable<AssetDescription> descriptions)
        {
            if (string.IsNullOrEmpty(externalId))
            {
                throw new ArgumentException("The external id is not specified.", nameof(externalId));
            }

            if (fileContent == null)
            {
                throw new ArgumentNullException(nameof(fileContent));
            }

            if (descriptions == null)
            {
                throw new ArgumentNullException(nameof(descriptions));
            }

            var fileResult = await client.UploadFileAsync(fileContent);

            var asset = new AssetUpsertModel
            {
                FileReference = fileResult,
                Descriptions = descriptions
            };

            var response = await client.UpsertAssetByExternalIdAsync(externalId, asset);

            return response;
        }
        private static void UpdateAssetTitle(AssetUpdateModel updatedAsset, AssetUpsertModel asset)
        {
            if (updatedAsset.Title != null)
            {
                asset.Title = updatedAsset.Title;
            }
        }

        /// <summary>
        /// Get Folder Hiearchy for a given folder Id
        /// </summary>
        /// <param name="folders">Folders</param>
        /// <param name="folderId">Folder Id</param>
        /// <returns></returns>
        public static IEnumerable<AssetFolderHierarchy> GetFolderHierarchy(this IEnumerable<AssetFolderHierarchy> folders, string folderId)
        {
            if (folders == null)
                return null;
            //Recursively search for the folder hierarchy that an asset is in. Returns null if file is not in a folder.
            var folderList = new List<AssetFolderHierarchy>();
            foreach (var itm in folders)
            {
                if (itm.Id == folderId)
                {
                    folderList.Add(itm);
                }
                if (itm.Folders != null)
                {
                    itm.Folders.GetFolderHierarchy(folderId);
                }
            }
            return folderList;
        }

        /// <summary>
        /// Gets the full folder path string
        /// </summary>
        /// <param name="folder">Folder</param>
        /// <returns></returns>
        public static string GetFullFolderPath(this AssetFolderLinkingHierarchy folder)
        {
            List<string> folderName = new List<string>();
            if (folder.Parent != null)
                folderName.Add(GetFullFolderPath(folder.Parent));
            folderName.Add(folder.Name);
            return string.Join("\\", folderName);
        }

        /// <summary>
        /// Gets the full path to a specific folder id so you can walk back up the parent linking tree
        /// </summary>
        /// <param name="folders">Folder</param>
        /// <param name="folderId">Folder Id</param>
        /// <returns></returns>
        public static AssetFolderLinkingHierarchy GetParentLinkedFolderHierarchyById(this IEnumerable<AssetFolderLinkingHierarchy> folders, string folderId)
        {
            if (folders == null)
                return null;
            foreach (var folder in folders)
            {
                if (folder.Id == folderId)
                    return folder;
                else if (folder.Folders != null && folder.Folders.Count() > 0)
                {
                    var nestedFolder = folder.Folders.GetParentLinkedFolderHierarchyById(folderId);
                    if (nestedFolder != null)
                        return nestedFolder;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves a recursive linked list of folders with the parent property filled in
        /// </summary>
        /// <param name="folders">Folder list from Kentico Kontent</param>
        /// <param name="parentLinked">Parent linked folder</param>
        /// <returns></returns>
        public static IEnumerable<AssetFolderLinkingHierarchy> GetParentLinkedFolderHierarchy(this IEnumerable<AssetFolderHierarchy> folders,
            AssetFolderLinkingHierarchy parentLinked = null)
        {
            //Recursively search for the folder hierarchy that an asset is in. Returns null if file is not in a folder.
            var folderList = new List<AssetFolderLinkingHierarchy>();
            foreach (var itm in folders)
            {
                var newFolder = new AssetFolderLinkingHierarchy()
                {
                    ExternalId = itm.ExternalId,
                    Folders = (itm.Folders != null && itm.Folders.Count() > 0) ? new List<AssetFolderLinkingHierarchy>() : null,
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
