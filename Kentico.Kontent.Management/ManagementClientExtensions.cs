using System;
using System.Collections.Generic;
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
    }
}
