using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

using KenticoCloud.ContentManagement.Models.Assets;
using KenticoCloud.ContentManagement.Modules.HttpClient;
using KenticoCloud.ContentManagement.Modules.RequestMapper;

namespace KenticoCloud.ContentManagement
{
    public sealed class ContentManagementClient
    {
        const int MAX_FILE_SIZE_MB = 100;

        private ActionInvoker _actionInvoker;
        private EndpointUrlBuilder _urlBuilder;


        public ContentManagementClient(ContentManagementOptions contentManagementOptions)
        {
            if(contentManagementOptions == null)
            {
                throw new ArgumentNullException(nameof(contentManagementOptions), "The Content" +
                "Management options object is not specified.");
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

        public async Task<ContentItemVariantResponseModel> UpsertVariantAsync(ContentItemVariantIdentifier identifier, ContentItemVariantUpdateModel contentItemVariantUpdateModel)
        {

            var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
            var variantReponse = await _actionInvoker.InvokeMethodAsync<ContentItemVariantUpdateModel, ContentItemVariantResponseModel>(endpointUrl, HttpMethod.Put, contentItemVariantUpdateModel);

            return variantReponse;
        }

        public async Task<List<ContentItemVariantResponseModel>> ListContentItemVariantsAsync(ContentItemIdentifier identifier)
        {

            var endpointUrl = _urlBuilder.BuildListVariantsUrl(identifier);
            var variantsReponse = await _actionInvoker.InvokeReadOnlyMethodAsync<List<ContentItemVariantResponseModel>>(endpointUrl, HttpMethod.Get);

            return variantsReponse;
        }

        public async Task DeleteContentItemVariantAsync(ContentItemVariantIdentifier identifier)
        {
            var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }

        #endregion

        #region Manage Items

        public async Task<ContentItemResponseModel> UpdateContentItemAsync(ContentItemIdentifier identifier, ContentItemPutModel contentItem)
        {
            var endpointUrl = _urlBuilder.BuildItemUrl(identifier);     
            var contentItemReponse =  await _actionInvoker.InvokeMethodAsync<ContentItemPutModel, ContentItemResponseModel>(endpointUrl, HttpMethod.Put, contentItem);

            return contentItemReponse;
        }

        public async Task<ContentItemResponseModel> UpdateContentItemByExternalIdAsync(ContentItemIdentifier identifier, ContentItemUpsertModel contentItem)
        {
            var endpointUrl = _urlBuilder.BuildItemUrl(identifier);
            var contentItemReponse = await _actionInvoker.InvokeMethodAsync<ContentItemUpsertModel, ContentItemResponseModel>(endpointUrl, HttpMethod.Put, contentItem);

            return contentItemReponse;
        }

        public async Task<ContentItemResponseModel> AddContentItemAsync(ContentItemPostModel contentItem)
        {

            var endpointUrl = _urlBuilder.BuildItemsUrl();
            var contentItemReponse = await _actionInvoker.InvokeMethodAsync<ContentItemPostModel, ContentItemResponseModel>(endpointUrl, HttpMethod.Post, contentItem);

            return contentItemReponse;
        }

        public async Task<ContentItemResponseModel> GetContentItemAsync(ContentItemIdentifier identifier)
        {
            var endpointUrl = _urlBuilder.BuildItemUrl(identifier);
            var contentItemReponse = await _actionInvoker.InvokeReadOnlyMethodAsync<ContentItemResponseModel>(endpointUrl, HttpMethod.Get);

            return contentItemReponse;
        }

        public async Task DeleteContentItemAsync(ContentItemIdentifier contentItem)
        {
            var endpointUrl = _urlBuilder.BuildItemUrl(contentItem);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }

        public async Task<ContentItemsResponseModel> ListContentItemsAsync()
        {
            var endpointUrl = _urlBuilder.BuildItemsUrl();

            return await _actionInvoker.InvokeReadOnlyMethodAsync<ContentItemsResponseModel>(endpointUrl, HttpMethod.Get);
        }

        public async Task<HttpResponseMessage> GetContentItemVariantAsync(ContentItemVariantIdentifier identifier)
        {
            var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);

            return await _actionInvoker.InvokeReadOnlyMethodAsync<HttpResponseMessage>(endpointUrl, HttpMethod.Get);
        }

        #endregion

        #region Assets

        public async Task<GetAssetsResponseModel> ListAssets()
        {
            string continuationToken = null;

            var assets = new List<AssetModel>();

            AssetListingResponseModel response;
            do
            {
                var endpointUrl = _urlBuilder.BuildAssetListingUrl(continuationToken);
                response = await _actionInvoker.InvokeReadOnlyMethodAsync<AssetListingResponseModel>(endpointUrl,
                    HttpMethod.Get);
                continuationToken = response.Pagination?.Token;
                assets.AddRange(response.Assets);
            }
            while(response.Pagination != null);

            return new GetAssetsResponseModel {Assets = assets};
        }

        public async Task<AssetModel> UpdateAssetById(string id, AssetUpdateModel update)
        {
            var endpoint = _urlBuilder.BuildAssetsUrlFromId(id);

            return await _actionInvoker.InvokeMethodAsync<AssetUpdateModel, AssetModel>(endpoint, HttpMethod.Put, update);

        }

        public async Task<AssetModel> ViewAssetById(string id)
        {
            var endpoint = _urlBuilder.BuildAssetsUrlFromId(id);
            return await _actionInvoker.InvokeReadOnlyMethodAsync<AssetModel>(endpoint, HttpMethod.Get);

        }

        public async Task<AssetModel> ViewAssetByExternalId(string externalId)
        {
            var endpoint = _urlBuilder.BuildAssetsUrlFromExternalId(externalId);
            return await _actionInvoker.InvokeReadOnlyMethodAsync<AssetModel>(endpoint, HttpMethod.Get);

        }

        public async Task DeleteAssetById(string id)
        {
            var endpoint = _urlBuilder.BuildAssetsUrlFromId(id);
            await _actionInvoker.InvokeMethodAsync(endpoint, HttpMethod.Delete);
        }

        public async Task DeleteAssetByExternalId(string externalId)
        {
            var endpoint = _urlBuilder.BuildAssetsUrlFromExternalId(externalId);
            await _actionInvoker.InvokeMethodAsync(endpoint, HttpMethod.Delete);
        }

        public async Task<AssetModel> UpsertAssetByExternalId(string externalId, AssetUpsertModel upsert)
        {
            var endpoint = _urlBuilder.BuildAssetsUrlFromExternalId(externalId);
            return await _actionInvoker.InvokeMethodAsync<AssetUpsertServerModel, AssetModel>(
                endpoint,
                HttpMethod.Put, 
                new AssetUpsertServerModel {
                    Descriptions = upsert.Descriptions,
                    FileReference = upsert.FileReference,
                    ExternalId = externalId
                }
            );
        }

        #endregion

        #region Binary files

        public async Task<FileReferenceModel> UploadFile(Stream stream, string fileName, string contentType)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentException(nameof(fileName));

            if (String.IsNullOrEmpty(contentType))
                throw new ArgumentException(nameof(contentType));

            if (stream.Length > MAX_FILE_SIZE_MB)
            {
                throw new ArgumentException($"Maximum supported file size is {MAX_FILE_SIZE_MB} MB.", nameof(stream));
            }

            var endpointUrl = _urlBuilder.BuildUploadFileUrl(fileName);

            return await _actionInvoker.UploadFileAsync<FileReferenceModel>(endpointUrl, stream, contentType);
        }

        #endregion
    }
}
