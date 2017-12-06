using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using KenticoCloud.ContentManagement.Modules.ActionInvoker;
using KenticoCloud.ContentManagement.Modules.HttpClient;
using KenticoCloud.ContentManagement.Models;
using KenticoCloud.ContentManagement.Models.Assets;
using KenticoCloud.ContentManagement.Models.Items;
using KenticoCloud.ContentManagement.Models.StronglyTyped;
using KenticoCloud.ContentManagement.Modules.ModelBuilders;

namespace KenticoCloud.ContentManagement
{
    public sealed class ContentManagementClient
    {
        private const int MAX_FILE_SIZE_MB = 100;

        private ActionInvoker _actionInvoker;
        private EndpointUrlBuilder _urlBuilder;
        private IModelProvider _modelProvider;

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
                throw new ArgumentException($"Provided string is not a valid project identifier ({contentManagementOptions.ProjectId}). Haven't you accidentally passed the Preview API key instead of the project identifier?", nameof(contentManagementOptions.ProjectId));
            }

            if (string.IsNullOrEmpty(contentManagementOptions.ApiKey))
            {
                throw new ArgumentException("The API key is not specified.", nameof(contentManagementOptions.ApiKey));
            }

            _urlBuilder = new EndpointUrlBuilder(contentManagementOptions);
            _actionInvoker = new ActionInvoker(new ContentManagementHttpClient(), new MessageCreator(contentManagementOptions.ApiKey));
            _modelProvider = contentManagementOptions.ModelProvider ?? new ModelProvider();
        }

        internal ContentManagementClient(EndpointUrlBuilder urlBuilder, ActionInvoker actionInvoker)
        {
            _urlBuilder = urlBuilder ?? throw new ArgumentNullException(nameof(urlBuilder));
            _actionInvoker = actionInvoker ?? throw new ArgumentNullException(nameof(actionInvoker));
        }
        
        #region Variants

        public async Task<IEnumerable<ContentItemVariantModel>> ListContentItemVariantsAsync(ContentItemIdentifier identifier)
        {
            var endpointUrl = _urlBuilder.BuildListVariantsUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<IEnumerable<ContentItemVariantModel>>(endpointUrl, HttpMethod.Get);

            return response;
        }

        public async Task<ContentItemVariantModel> GetContentItemVariantAsync(ContentItemVariantIdentifier identifier)
        {
            var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<ContentItemVariantModel>(endpointUrl, HttpMethod.Get);

            return response;
        }

        public async Task<ContentItemVariantModel> UpsertContentItemVariantAsync(ContentItemVariantIdentifier identifier, ContentItemVariantUpsertModel contentItemVariantUpsertModel)
        { 
            var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
            var response = await _actionInvoker.InvokeMethodAsync<ContentItemVariantUpsertModel, ContentItemVariantModel>(endpointUrl, HttpMethod.Put, contentItemVariantUpsertModel);

            return response;
        }

        public async Task DeleteContentItemVariantAsync(ContentItemVariantIdentifier identifier)
        {
            var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }

        #endregion

        #region Strongly typed Variants

        public async Task<List<ContentItemVariantModel<T>>> ListContentItemVariantsAsync<T>(ContentItemIdentifier identifier) where T : new()
        {

            var endpointUrl = _urlBuilder.BuildListVariantsUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<List<ContentItemVariantModel>>(endpointUrl, HttpMethod.Get);

            return response.Select(x => _modelProvider.GetContentItemVariantModel<T>(x)).ToList();
        }

        public async Task<ContentItemVariantModel<T>> GetContentItemVariantAsync<T>(ContentItemVariantIdentifier identifier) where T : new()
        {
            var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<ContentItemVariantModel>(endpointUrl, HttpMethod.Get);

            return _modelProvider.GetContentItemVariantModel<T>(response);
        }

        public async Task<ContentItemVariantModel<T>> UpsertContentItemVariantAsync<T>(ContentItemVariantIdentifier identifier, T variantElements) where T : new()
        {
            var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
            var variantUpsertModel = _modelProvider.GetContentItemVariantUpsertModel(variantElements);
            var response = await _actionInvoker.InvokeMethodAsync<ContentItemVariantUpsertModel, ContentItemVariantModel>(endpointUrl, HttpMethod.Put, variantUpsertModel);

            return _modelProvider.GetContentItemVariantModel<T>(response);
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

        public async Task DeleteContentItemAsync(ContentItemIdentifier identifier)
        {
            var endpointUrl = _urlBuilder.BuildItemUrl(identifier);

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
