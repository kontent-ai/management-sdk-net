using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

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
            
            if (string.IsNullOrEmpty(contentManagementOptions.ProjectId))
            {
                throw new ArgumentException("Kentico Cloud project identifier is not specified.", nameof(contentManagementOptions.ProjectId));
            }

            if (!Guid.TryParse(contentManagementOptions.ProjectId, out Guid projectIdGuid))
            {
                throw new ArgumentException("Provided string is not a valid project identifier ({ProjectId}). Haven't you accidentally passed the Preview API key instead of the project identifier?", nameof(contentManagementOptions.ProjectId));
            }

            if (string.IsNullOrEmpty(contentManagementOptions.ApiKey))
            {
                throw new ArgumentException("The API key is not specified.", nameof(contentManagementOptions.ApiKey));
            }

            _urlBuilder = new EndpointUrlBuilder(contentManagementOptions);
            _actionInvoker = new ActionInvoker(new ContentManagementHttpClient(), new MessageCreator(contentManagementOptions.ApiKey));
        }
        
        #region Variants

        public async Task<List<ContentItemVariantModel>> ListContentItemVariantsAsync(ContentItemIdentifier identifier)
        {

            var endpointUrl = _urlBuilder.BuildListVariantsUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<List<ContentItemVariantModel>>(endpointUrl, HttpMethod.Get);

            return response;
        }

        public async Task<ContentItemVariantModel> GetContentItemVariantAsync(ContentItemVariantIdentifier identifier)
        {
            var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<ContentItemVariantModel>(endpointUrl, HttpMethod.Get);

            return response;
        }

        public async Task<ContentItemVariantModel> UpsertContentItemVariantAsync(ContentItemVariantIdentifier identifier, ContentItemVariantUpdateModel contentItemVariantUpdateModel)
        {

            var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
            var response = await _actionInvoker.InvokeMethodAsync<ContentItemVariantUpdateModel, ContentItemVariantModel>(endpointUrl, HttpMethod.Put, contentItemVariantUpdateModel);

            return response;
        }

        public async Task DeleteContentItemVariantAsync(ContentItemVariantIdentifier identifier)
        {
            var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }

        #endregion

        #region Items

        public async Task<ContentItemModel> UpdateContentItemAsync(ContentItemIdentifier identifier, ContentItemUpdateModel contentItem)
        {
            var endpointUrl = _urlBuilder.BuildItemUrl(identifier);
            var response = await _actionInvoker.InvokeMethodAsync<ContentItemUpdateModel, ContentItemModel>(endpointUrl, HttpMethod.Put, contentItem);

            return response;
        }

        public async Task<ContentItemModel> UpsertContentItemByExternalIdAsync(string externalId, ContentItemUpsertModel contentItem)
        {
            var endpointUrl = _urlBuilder.BuildItemUrl(ContentItemIdentifier.ByExternalId(externalId));
            var response = await _actionInvoker.InvokeMethodAsync<ContentItemUpsertModel, ContentItemModel>(endpointUrl, HttpMethod.Put, contentItem);

            return response;
        }

        public async Task<ContentItemModel> CreateContentItemAsync(ContentItemCreateModel contentItem)
        {
            var endpointUrl = _urlBuilder.BuildItemsUrl();
            var response = await _actionInvoker.InvokeMethodAsync<ContentItemCreateModel, ContentItemModel>(endpointUrl, HttpMethod.Post, contentItem);

            return response;
        }

        public async Task<ContentItemModel> GetContentItemAsync(ContentItemIdentifier identifier)
        {
            var endpointUrl = _urlBuilder.BuildItemUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<ContentItemModel>(endpointUrl, HttpMethod.Get);

            return response;
        }

        public async Task DeleteContentItemAsync(ContentItemIdentifier contentItem)
        {
            var endpointUrl = _urlBuilder.BuildItemUrl(contentItem);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }

        private async Task<IListingResponse<ContentItemModel>> GetNextItemsListingPageAsync(string continuationToken)
        {
            var endpointUrl = _urlBuilder.BuildItemsListingUrl(continuationToken);
            return await _actionInvoker.InvokeReadOnlyMethodAsync<ContentItemListingResponseServerModel>(endpointUrl, HttpMethod.Get);
        }

        public async Task<ListingResponseModel<ContentItemModel>> ListContentItemsAsync()
        {
            var endpointUrl = _urlBuilder.BuildItemsUrl();
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<ContentItemListingResponseServerModel>(endpointUrl, HttpMethod.Get);

            return new ListingResponseModel<ContentItemModel>(GetNextItemsListingPageAsync, response.Pagination?.Token, response.Items);
        }

        #endregion

        #region Assets

        private async Task<IListingResponse<AssetModel>> GetNextAssetListingPageAsync(string continuationToken)
        {
            var endpointUrl = _urlBuilder.BuildAssetListingUrl(continuationToken);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<IListingResponse<AssetModel>>(endpointUrl, HttpMethod.Get);

            return response;
        }

        public async Task<ListingResponseModel<AssetModel>> ListAssetsAsync()
        {
            var endpointUrl = _urlBuilder.BuildAssetListingUrl();
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<AssetListingResponseServerModel>(endpointUrl, HttpMethod.Get);

            return new ListingResponseModel<AssetModel>(GetNextAssetListingPageAsync, response.Pagination?.Token, response.Assets);
        }

        public async Task<AssetModel> GetAssetAsync(AssetIdentifier identifier)
        {
            var endpointUrl = _urlBuilder.BuildAssetsUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<AssetModel>(endpointUrl, HttpMethod.Get);

            return response;
        }

        public async Task<AssetModel> UpdateAssetAsync(AssetIdentifier identifier, AssetUpdateModel update)
        {
            var endpointUrl = _urlBuilder.BuildAssetsUrl(identifier);
            var response = await _actionInvoker.InvokeMethodAsync<AssetUpdateModel, AssetModel>(endpointUrl, HttpMethod.Put, update);

            return response;
        }

        public async Task DeleteAssetAsync(AssetIdentifier identifier)
        {
            var endpointUrl = _urlBuilder.BuildAssetsUrl(identifier);
            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }
        
        public async Task<AssetModel> CreateAssetAsync(AssetUpsertModel asset)
        {
            var endpointUrl = _urlBuilder.BuildAssetsUrl();
            var response = await _actionInvoker.InvokeMethodAsync<AssetUpsertModel, AssetModel>(endpointUrl, HttpMethod.Post, asset);

            return response;
        }

        public async Task<AssetModel> UpsertAssetByExternalIdAsync(string externalId, AssetUpsertModel asset)
        {
            var endpointUrl = _urlBuilder.BuildAssetsUrlFromExternalId(externalId);
            var response = await _actionInvoker.InvokeMethodAsync<AssetUpsertServerModel, AssetModel>(
                endpointUrl,
                HttpMethod.Put,
                // TODO - Endpoint currently requires external ID in both model and URL, this is a bug
                new AssetUpsertServerModel
                {
                    Descriptions = asset.Descriptions,
                    FileReference = asset.FileReference,
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
        public async Task<FileReferenceModel> UploadFileAsync(FileContentSource fileContent)
        {
            var stream = fileContent.OpenReadStream();
            try
            {
                if (stream.Length > MAX_FILE_SIZE_MB * 1024 * 1024)
                {
                    throw new ArgumentException($"Maximum supported file size is {MAX_FILE_SIZE_MB} MB.", nameof(stream));
                }

                var endpointUrl = _urlBuilder.BuildUploadFileUrl(fileContent.FileName);
                var response = await _actionInvoker.UploadFileAsync<FileReferenceModel>(endpointUrl, stream, fileContent.ContentType);

                return response;
            }
            finally
            {
                // Dispose the stream only in case new stream was created
                if (fileContent.CreatesNewStream)
                {
                    stream.Dispose();
                }
            }
        }

        #endregion
    }
}
