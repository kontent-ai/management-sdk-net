using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using KenticoCloud.ContentManagement.Models.Assets;
using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement
{
    public sealed class ContentManagementClient
    {
        private readonly ContentManagementOptions _contentManagementOptions;

        private HttpClient _httpClient;
        private EndpointUrlBuilder _urlBuilder;
        private JsonSerializerSettings _serializeSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

        private EndpointUrlBuilder UrlBuilder
        {
            get { return _urlBuilder ?? (_urlBuilder = new EndpointUrlBuilder(_contentManagementOptions)); }
        }

        /// <summary>
        /// An instance of <see cref="System.Net.Http.HttpClient"/> for sending requests to KC endpoints.
        /// </summary>
        public HttpClient HttpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    _httpClient = new HttpClient();
                }
                return _httpClient;
            }
            set { _httpClient = value; }
        }

        public ContentManagementClient(ContentManagementOptions contentManagementOptions)
        {
            _contentManagementOptions = contentManagementOptions ?? throw new ArgumentNullException(nameof(contentManagementOptions), "The Content" +
                "Management options object is not specified.");

            if (_contentManagementOptions.ProjectId == null)
            {
                throw new ArgumentNullException(nameof(_contentManagementOptions.ProjectId), "Kentico Cloud project identifier is not specified.");
            }

            if (_contentManagementOptions.ProjectId == string.Empty)
            {
                throw new ArgumentException("Kentico Cloud project identifier is not specified.", nameof(_contentManagementOptions.ProjectId));
            }

            if (!Guid.TryParse(_contentManagementOptions.ProjectId, out Guid projectIdGuid))
            {
                throw new ArgumentException("Provided string is not a valid project identifier ({ProjectId}). Haven't you accidentally passed the Preview API key instead of the project identifier?", nameof(_contentManagementOptions.ProjectId));
            }

            if (_contentManagementOptions.ApiKey == null)
            {
                throw new ArgumentNullException(nameof(_contentManagementOptions.ApiKey), "The API key is not specified.");
            }

            if (_contentManagementOptions.ApiKey == string.Empty)
            {
                throw new ArgumentException("The API key is not specified.", nameof(_contentManagementOptions.ApiKey));
            }


            _contentManagementOptions.ProjectId = projectIdGuid.ToString("D");
        }

        #region Manage Variants

        public async Task<ContentItemVariantResponseModel> UpsertVariantAsync(ContentItemVariantIdentifier identifier, ContentItemVariantUpdateModel contentItemVariantUpdateModel)
        {

            var endpointUrl = UrlBuilder.BuildVariantsUrl(identifier);
            var variantReponse = await GetContentManagementResponseAsync<ContentItemVariantResponseModel, object>(endpointUrl, HttpMethod.Put, contentItemVariantUpdateModel);

            return variantReponse;
        }

        public async Task<List<ContentItemVariantResponseModel>> ListContentItemVariantsAsync(ContentItemIdentifier identifier)
        {

            var endpointUrl = UrlBuilder.BuildListVariantsUrl(identifier);
            var variantsReponse = await GetContentManagementResponseAsync<List<ContentItemVariantResponseModel>, object>(endpointUrl, HttpMethod.Get);

            return variantsReponse;
        }

        public async Task DeleteContentItemVariantAsync(ContentItemVariantIdentifier identifier)
        {
            var endpointUrl = UrlBuilder.BuildVariantsUrl(identifier);
            await GetContentManagementResponseAsync<object, object>(endpointUrl, HttpMethod.Delete);
        }

        #endregion

        #region Manage Items

        public async Task<ContentItemResponseModel> UpdateContentItemAsync(ContentItemIdentifier identifier, ContentItemPutModel contentItem)
        {
            var endpointUrl = UrlBuilder.BuildItemUrl(identifier);     
            var contentItemReponse =  await GetContentManagementResponseAsync<ContentItemResponseModel, object>(endpointUrl, HttpMethod.Put, contentItem);

            return contentItemReponse;
        }

        public async Task<ContentItemResponseModel> UpdateContentItemByExternalIdAsync(ContentItemIdentifier identifier, ContentItemUpsertModel contentItem)
        {
            var endpointUrl = UrlBuilder.BuildItemUrl(identifier);
            var contentItemReponse = await GetContentManagementResponseAsync<ContentItemResponseModel, object>(endpointUrl, HttpMethod.Put, contentItem);

            return contentItemReponse;
        }

        public async Task<ContentItemResponseModel> AddContentItemAsync(ContentItemPostModel contentItem)
        {

            var endpointUrl = UrlBuilder.BuildItemsUrl();
            var contentItemReponse = await GetContentManagementResponseAsync<ContentItemResponseModel, object>(endpointUrl, HttpMethod.Post, contentItem);

            return contentItemReponse;
        }

        public async Task<ContentItemResponseModel> GetContentItemAsync(ContentItemIdentifier identifier)
        {
            var endpointUrl = UrlBuilder.BuildItemUrl(identifier);
            var contentItemReponse = await GetContentManagementResponseAsync<ContentItemResponseModel, object>(endpointUrl, HttpMethod.Get);

            return contentItemReponse;
        }

        public async Task<HttpResponseMessage> DeleteContentItemAsync(ContentItemIdentifier contentItem)
        {
            var endpointUrl = UrlBuilder.BuildItemUrl(contentItem);

            return await GetContentManagementResponseAsync<HttpResponseMessage, object>(endpointUrl, HttpMethod.Delete);
        }

        public async Task<ContentItemsResponseModel> ListContentItemsAsync()
        {
            var endpointUrl = UrlBuilder.BuildItemsUrl();

            return await GetContentManagementResponseAsync<ContentItemsResponseModel, object>(endpointUrl, HttpMethod.Get);
        }

        public async Task<HttpResponseMessage> GetContentItemVariantAsync(ContentItemVariantIdentifier identifier)
        {
            var endpointUrl = UrlBuilder.BuildVariantsUrl(identifier);

            return await GetContentManagementResponseAsync<HttpResponseMessage, object>(endpointUrl, HttpMethod.Get);
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
                var endpointUrl = UrlBuilder.BuildAssetListingUrl(continuationToken);
                response = await GetContentManagementResponseAsync<AssetListingResponseModel, object>(endpointUrl,
                    HttpMethod.Get);
                continuationToken = response.Pagination?.Token;
                assets.AddRange(response.Assets);
            }
            while(response.Pagination != null);

            return new GetAssetsResponseModel {Assets = assets};
        }

        public async Task<AssetModel> UpdateAssetById(string id, AssetUpdateModel update)
        {
            var endpoint = UrlBuilder.BuildAssetsUrlFromId(id);

            return await GetContentManagementResponseAsync<AssetModel, object>(endpoint, HttpMethod.Put, update);

        }

        public async Task<AssetModel> ViewAssetById(string id)
        {
            var endpoint = UrlBuilder.BuildAssetsUrlFromId(id);
            return await GetContentManagementResponseAsync<AssetModel, object>(endpoint, HttpMethod.Get);

        }

        public async Task<AssetModel> DeleteAssetById(string id)
        {
            var endpoint = UrlBuilder.BuildAssetsUrlFromId(id);
            return await GetContentManagementResponseAsync<AssetModel, object>(endpoint, HttpMethod.Delete);
        }

        public async Task<AssetModel> DeleteAssetByExternalId(string externalId)
        {
            var endpoint = UrlBuilder.BuildAssetsUrlFromExternalId(externalId);
            return await GetContentManagementResponseAsync<AssetModel, object>(endpoint, HttpMethod.Delete);
        }

        public async Task<AssetModel> UpsertAssetByExternalId(string externalId, AssetUpsertModel upsert)
        {
            var endpoint = UrlBuilder.BuildAssetsUrlFromExternalId(externalId);
            return await GetContentManagementResponseAsync<AssetModel, object>(
                endpoint,
                HttpMethod.Put, 
                new AssetUpsertServerModel {
                    Descriptions = upsert.Descriptions,
                    FileReference = upsert.FileReference,
                    ExternalId = externalId
                }
            );
        }

        public async Task<AssetModel> ViewAssetByExternalId(string externalId)
        {
            var endpoint = UrlBuilder.BuildAssetsUrlFromExternalId(externalId);
            return await GetContentManagementResponseAsync<AssetModel, object>(endpoint, HttpMethod.Get);

        }

        public async Task<HttpResponseMessage> UploadAsset(string fileName, byte[] file, string contentType)
        {
            // TODO: add file size limitation
            var endpointUrl = UrlBuilder.BuildUploadAssetUrl(fileName);

            var body = new {
                dataBinary = file, contentType, contentLength = file.Length
            };

            return await GetContentManagementResponseAsync<HttpResponseMessage, object>(endpointUrl, HttpMethod.Post, body);
        }

        #endregion

        private async Task<T> GetContentManagementResponseAsync<T, U>(string endpointUrl, HttpMethod method, U body = null) where U : class
        {
            var message = new HttpRequestMessage(method, endpointUrl);
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _contentManagementOptions.ApiKey);

            if (body != null)
            {
                string json = JsonConvert.SerializeObject(body, Formatting.None, _serializeSettings);
                message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var response = await HttpClient.SendAsync(message);

            if (response.IsSuccessStatusCode)
            {
                // TODO: Temp solution for Delete - delete request does not send body data
                if(method == HttpMethod.Delete)
                {
                    return default(T);
                }

                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseString);
            }

            throw new ContentManagementException(response, await response.Content.ReadAsStringAsync());
        }
    }
}
