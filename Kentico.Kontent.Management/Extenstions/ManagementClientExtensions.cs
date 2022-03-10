using System;
using System.Threading.Tasks;
using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.Shared;

namespace Kentico.Kontent.Management.Extenstions
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
        public static async Task<ContentItemModel> UpdateContentItemAsync(this IManagementClient client, Reference identifier, ContentItemModel contentItem)
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
        /// Creates asset.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="fileContent">Represents the content of the file.</param>
        /// <param name="assetCreateModel">Updated values for the asset.</param>
        public static async Task<AssetModel> CreateAssetAsync(this IManagementClient client, FileContentSource fileContent, AssetCreateModel assetCreateModel)
        {
            if (fileContent == null)
            {
                throw new ArgumentNullException(nameof(fileContent));
            }
            if (assetCreateModel == null)
            {
                throw new ArgumentNullException(nameof(assetCreateModel));
            }

            if (assetCreateModel.Descriptions == null)
            {
                throw new ArgumentNullException(nameof(assetCreateModel.Descriptions));
            }

            var fileResult = await client.UploadFileAsync(fileContent);

            var asset = new AssetCreateModel
            {
                FileReference = fileResult,
                Descriptions = assetCreateModel.Descriptions,
                Title = assetCreateModel.Title
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
        /// <param name="updatedAsset">Updated values for the asset.</param>
        /// <returns>The <see cref="AssetModel"/> instance that represents created or updated asset.</returns>
        public static async Task<AssetModel> UpsertAssetByExternalIdAsync(this IManagementClient client, string externalId, FileContentSource fileContent, AssetUpdateModel updatedAsset)
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

        private static void UpdateAssetTitle(AssetUpdateModel updatedAsset, AssetUpsertModel asset)
        {
            if (updatedAsset.Title != null)
            {
                asset.Title = updatedAsset.Title;
            }
        }
    }
}
