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
            var contentItemVariantUpdateModel = new ContentItemVariantUpsertModel(contentItemVariant);

            return await client.UpsertContentItemVariantAsync(identifier, contentItemVariant);
        }

        /// <summary>
        /// Uploads the given file from a file system.
        /// </summary>
        /// <param name="client">Content management client instance.</param>
        /// <param name="data">Binary data of the file.</param>
        /// <param name="fileName">The name of the uploaded binary file. It will be used for the asset name when creating an asset. Example: which-brewing-fits-you-1080px.jpg.</param>
        /// <param name="contentType">Specifies the media type of the binary data. Example: image/jpeg, application/zip.</param>
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
