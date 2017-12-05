using System.Collections.Generic;
using System.Threading.Tasks;

using KenticoCloud.ContentManagement.Models.Assets;
using KenticoCloud.ContentManagement.Models.Items;

namespace KenticoCloud.ContentManagement
{
    /// <summary>
    /// Extra simplifying methods available for ContentManagementClient
    /// </summary>
    public static class ContentManagementClientExtensions
    {
        /// <summary>
        /// Updates the given content item.
        /// </summary>
        /// <param name="client">Content management client instance.</param>
        /// <param name="identifier">Identifies which content item will be updated. </param>
        /// <param name="contentItem">Specifies data for updated content item.</param>
        public static async Task<ContentItemModel> UpdateContentItemAsync(this ContentManagementClient client, ContentItemIdentifier identifier, ContentItemModel contentItem)
        {
            var contentItemUpdateModel = new ContentItemUpdateModel(contentItem);

            return await client.UpdateContentItemAsync(identifier, contentItemUpdateModel);
        }

        /// <summary>
        /// Upserts the given content item variant.
        /// </summary>
        /// <param name="client">Content management client instance.</param>
        /// <param name="identifier">Identifies which content item variant will be upserted. </param>
        /// <param name="contentItemVariant">Specifies data for upserted content item variant.</param>
        public static async Task<ContentItemVariantModel> UpsertContentItemVariantAsync(this ContentManagementClient client, ContentItemVariantIdentifier identifier, ContentItemVariantModel contentItemVariant)
        {
            var contentItemVariantUpsertModel = new ContentItemVariantUpsertModel(contentItemVariant);

            return await client.UpsertContentItemVariantAsync(identifier, contentItemVariantUpsertModel);
        }

        /// <summary>
        /// Uploads the given file from a file system.
        /// </summary>
        /// <param name="client">Content management client instance.</param>
        /// <param name="descriptions">Asset descriptions.</param>
        public static async Task<AssetModel> CreateAssetAsync(this ContentManagementClient client, FileContentSource fileContent, List<AssetDescriptionsModel> descriptions)
        {
            var fileResult = await client.UploadFileAsync(fileContent);

            var asset = new AssetUpsertModel
            {
                FileReference = fileResult,
                Descriptions = new List<AssetDescriptionsModel>()
            };

            var response = await client.CreateAssetAsync(asset);

            return response;
        }

        /// <summary>
        /// Uploads the given file from a file system.
        /// </summary>
        /// <param name="client">Content management client instance.</param>
        /// <param name="externalId"></param>
        /// <param name="fileContent">File content.</param>
        /// <param name="descriptions">Asset descriptions.</param>
        public static async Task<AssetModel> UpsertAssetByExternalIdAsync(this ContentManagementClient client, string externalId, FileContentSource fileContent, List<AssetDescriptionsModel> descriptions)
        {
            var fileResult = await client.UploadFileAsync(fileContent);

            var asset = new AssetUpsertModel
            {
                FileReference = fileResult,
                Descriptions = new List<AssetDescriptionsModel>()
            };

            var response = await client.UpsertAssetByExternalIdAsync(externalId, asset);

            return response;
        }
    }
}
