using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

using KenticoCloud.ContentManagement.Modules.ActionInvoker;
using KenticoCloud.ContentManagement.Modules.HttpClient;
using KenticoCloud.ContentManagement.Models;
using KenticoCloud.ContentManagement.Models.Assets;
using KenticoCloud.ContentManagement.Models.Items;

namespace KenticoCloud.ContentManagement
{
    public sealed class ContentManagementClient
    {
        const int MAX_FILE_SIZE_MB = 100;

        private ActionInvoker _actionInvoker;
        private EndpointUrlBuilder _urlBuilder;


        public ContentManagementClient(ContentManagementOptions contentManagementOptions)
        {
            if (contentManagementOptions == null)
            {
                throw new ArgumentNullException(nameof(contentManagementOptions), "The Content Management options object is not specified.");
            }

            if (contentManagementOptions.ProjectId == null)
            {
                throw new ArgumentNullException(nameof(contentManagementOptions.ProjectId), "Kentico Cloud project identifier is not specified.");
            }

            if (contentManagementOptions.ProjectId == string.Empty)
            {
                throw new ArgumentException("Kentico Cloud project identifier is not specified.", nameof(contentManagementOptions.ProjectId));
            }

            if (!Guid.TryParse(contentManagementOptions.ProjectId, out Guid projectIdGuid))
            {
                throw new ArgumentException("Provided string is not a valid project identifier ({ProjectId}). Haven't you accidentally passed the Preview API key instead of the project identifier?", nameof(contentManagementOptions.ProjectId));
            }

            if (contentManagementOptions.ApiKey == null)
            {
                throw new ArgumentNullException(nameof(contentManagementOptions.ApiKey), "The API key is not specified.");
            }

            if (contentManagementOptions.ApiKey == string.Empty)
            {
                throw new ArgumentException("The API key is not specified.", nameof(contentManagementOptions.ApiKey));
            }

            _urlBuilder = new EndpointUrlBuilder(contentManagementOptions);
            _actionInvoker = new ActionInvoker(new ContentManagementHttpClient(), new MessageCreator(contentManagementOptions.ApiKey));
        }

        #region Manage Variants

        public async Task<List<ContentItemVariantResponseModel>> ListContentItemVariantsAsync(ContentItemIdentifier identifier)
        {

            var endpointUrl = _urlBuilder.BuildListVariantsUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<List<ContentItemVariantResponseModel>>(endpointUrl, HttpMethod.Get);

            return response;
        }

        public async Task<ContentItemVariantResponseModel> GetContentItemVariantAsync(ContentItemVariantIdentifier identifier)
        {
            var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<ContentItemVariantResponseModel>(endpointUrl, HttpMethod.Get);

            return response;
        }

        public async Task<ContentItemVariantResponseModel> UpsertVariantAsync(ContentItemVariantIdentifier identifier, ContentItemVariantUpdateModel contentItemVariantUpdateModel)
        {

            var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
            var response = await _actionInvoker.InvokeMethodAsync<ContentItemVariantUpdateModel, ContentItemVariantResponseModel>(endpointUrl, HttpMethod.Put, contentItemVariantUpdateModel);

            return response;
        }

        public async Task DeleteContentItemVariantAsync(ContentItemVariantIdentifier identifier)
        {
            var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }

        #endregion

        #region Manage Items

        public async Task<ContentItemResponseModel> UpdateContentItemAsync(ContentItemIdentifier identifier, ContentItemUpdateModel contentItem)
        {
            var endpointUrl = _urlBuilder.BuildItemUrl(identifier);
            var response = await _actionInvoker.InvokeMethodAsync<ContentItemUpdateModel, ContentItemResponseModel>(endpointUrl, HttpMethod.Put, contentItem);

            return response;
        }

        public async Task<ContentItemResponseModel> UpsertContentItemByExternalIdAsync(string externalId, ContentItemUpsertModel contentItem)
        {
            var endpointUrl = _urlBuilder.BuildItemUrl(ContentItemIdentifier.ByExternalId(externalId));
            var response = await _actionInvoker.InvokeMethodAsync<ContentItemUpsertModel, ContentItemResponseModel>(endpointUrl, HttpMethod.Put, contentItem);

            return response;
        }

        public async Task<ContentItemResponseModel> AddContentItemAsync(ContentItemCreateModel contentItem)
        {
            var endpointUrl = _urlBuilder.BuildItemsUrl();
            var response = await _actionInvoker.InvokeMethodAsync<ContentItemCreateModel, ContentItemResponseModel>(endpointUrl, HttpMethod.Post, contentItem);

            return response;
        }

        public async Task<ContentItemResponseModel> GetContentItemAsync(ContentItemIdentifier identifier)
        {
            var endpointUrl = _urlBuilder.BuildItemUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<ContentItemResponseModel>(endpointUrl, HttpMethod.Get);

            return response;
        }

        public async Task DeleteContentItemAsync(ContentItemIdentifier contentItem)
        {
            var endpointUrl = _urlBuilder.BuildItemUrl(contentItem);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }

        private async Task<IListingResponse<ContentItemResponseModel>> GetNextItemsListingPageAsync(string continuationToken)
        {
            var endpointUrl = _urlBuilder.BuildItemsListingUrl(continuationToken);
            return await _actionInvoker.InvokeReadOnlyMethodAsync<ContentItemListingResponseServerModel>(endpointUrl, HttpMethod.Get);
        }

        public async Task<ListingResponseModel<ContentItemResponseModel>> ListContentItemsAsync()
        {
            var endpointUrl = _urlBuilder.BuildItemsUrl();
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<ContentItemListingResponseServerModel>(endpointUrl, HttpMethod.Get);

            return new ListingResponseModel<ContentItemResponseModel>(GetNextItemsListingPageAsync, response.Pagination?.Token, response.Items);
        }

        #endregion

        #region Assets

        private async Task<IListingResponse<AssetResponseModel>> GetNextAssetListingPageAsync(string continuationToken)
        {
            var endpointUrl = _urlBuilder.BuildAssetListingUrl(continuationToken);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<IListingResponse<AssetResponseModel>>(endpointUrl, HttpMethod.Get);

            return response;
        }

        public async Task<ListingResponseModel<AssetResponseModel>> ListAssetsAsync()
        {
            var endpointUrl = _urlBuilder.BuildAssetListingUrl();
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<AssetListingResponseServerModel>(endpointUrl, HttpMethod.Get);

            return new ListingResponseModel<AssetResponseModel>(GetNextAssetListingPageAsync, response.Pagination?.Token, response.Assets);
        }

        public async Task<AssetResponseModel> GetAssetAsync(AssetIdentifier identifier)
        {
            var endpointUrl = _urlBuilder.BuildAssetsUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<AssetResponseModel>(endpointUrl, HttpMethod.Get);

            return response;
        }

        public async Task<AssetResponseModel> UpdateAssetAsync(AssetIdentifier identifier, AssetUpdateModel update)
        {
            var endpointUrl = _urlBuilder.BuildAssetsUrl(identifier);
            var response = await _actionInvoker.InvokeMethodAsync<AssetUpdateModel, AssetResponseModel>(endpointUrl, HttpMethod.Put, update);

            return response;
        }

        public async Task DeleteAssetAsync(AssetIdentifier identifier)
        {
            var endpointUrl = _urlBuilder.BuildAssetsUrl(identifier);
            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }
        
        public async Task<AssetResponseModel> AddAssetAsync(AssetUpsertModel asset)
        {
            var endpointUrl = _urlBuilder.BuildAssetsUrl();
            var response = await _actionInvoker.InvokeMethodAsync<AssetUpsertModel, AssetResponseModel>(endpointUrl, HttpMethod.Post, asset);

            return response;
        }

        public async Task<AssetResponseModel> UpsertAssetByExternalIdAsync(string externalId, AssetUpsertModel upsert)
        {
            var endpointUrl = _urlBuilder.BuildAssetsUrlFromExternalId(externalId);
            var response = await _actionInvoker.InvokeMethodAsync<AssetUpsertServerModel, AssetResponseModel>(
                endpointUrl,
                HttpMethod.Put,
                new AssetUpsertServerModel
                {
                    Descriptions = upsert.Descriptions,
                    FileReference = upsert.FileReference,
                    ExternalId = externalId
                }
            );

            return response;
        }

        #endregion

        #region Binary files

        /// <summary>
        /// Uploads the given file.
        /// </summary>
        /// <param name="stream">File stream with the binary file data.</param>
        /// <param name="fileName">The name of the uploaded binary file. It will be used for the asset name when creating an asset. Example: which-brewing-fits-you-1080px.jpg.</param>
        /// <param name="contentType">Specifies the media type of the binary data. Example: image/jpeg, application/zip.</param>
        public async Task<FileReferenceModel> UploadFileAsync(Stream stream, string fileName, string contentType)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentException(nameof(fileName));

            if (String.IsNullOrEmpty(contentType))
                throw new ArgumentException(nameof(contentType));

            if (stream.Length > MAX_FILE_SIZE_MB * 1024 * 1024)
            {
                throw new ArgumentException($"Maximum supported file size is {MAX_FILE_SIZE_MB} MB.", nameof(stream));
            }

            var endpointUrl = _urlBuilder.BuildUploadFileUrl(fileName);
            var response = await _actionInvoker.UploadFileAsync<FileReferenceModel>(endpointUrl, stream, contentType);

            return response;
        }

        #endregion
    }
}
