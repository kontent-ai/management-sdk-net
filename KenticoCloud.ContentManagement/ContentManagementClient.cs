using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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

            string json = JsonConvert.SerializeObject(contentItemVariantUpdateModel, Formatting.None, _serializeSettings);
            var body = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await GetContentManagementResponseAsync(endpointUrl, HttpMethod.Put, body);

            var responseString = await response.Content.ReadAsStringAsync();
            var variantReponse = JsonConvert.DeserializeObject<ContentItemVariantResponseModel>(responseString);

            return variantReponse;
        }

        public async Task<List<ContentItemVariantResponseModel>> ListContentItemVariantsAsync(ContentItemIdentifier identifier)
        {

            var endpointUrl = UrlBuilder.BuildListVariantsUrl(identifier);

            var response = await GetContentManagementResponseAsync(endpointUrl, HttpMethod.Get);

            var responseString = await response.Content.ReadAsStringAsync();
            var variantsReponse = JsonConvert.DeserializeObject<List<ContentItemVariantResponseModel>>(responseString);

            return variantsReponse;
        }

        public async Task<HttpResponseMessage> DeleteContentItemVariantAsync(ContentItemVariantIdentifier identifier)
        {
            var endpointUrl = UrlBuilder.BuildVariantsUrl(identifier);

            return await GetContentManagementResponseAsync(endpointUrl, HttpMethod.Delete);
        }

        #endregion

        #region Manage Items

        public async Task<ContentItemResponseModel> UpdateContentItemAsync(ContentItemIdentifier identifier, ContentItemPutModel contentItem)
        {
            var endpointUrl = UrlBuilder.BuildItemUrl(identifier);

            string json = JsonConvert.SerializeObject(contentItem);
            var body = new StringContent(json, Encoding.UTF8, "application/json");

            var response =  await GetContentManagementResponseAsync(endpointUrl, HttpMethod.Put, body);
            var responseString = await response.Content.ReadAsStringAsync();
            var contentItemReponse = JsonConvert.DeserializeObject<ContentItemResponseModel>(responseString);

            return contentItemReponse;
        }

        public async Task<ContentItemResponseModel> UpdateContentItemAsync(ContentItemIdentifier identifier, ContentItemUpsertModel contentItem)
        {
            var endpointUrl = UrlBuilder.BuildItemUrl(identifier);

            string json = JsonConvert.SerializeObject(contentItem);
            var body = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await GetContentManagementResponseAsync(endpointUrl, HttpMethod.Put, body);
            var responseString = await response.Content.ReadAsStringAsync();
            var contentItemReponse = JsonConvert.DeserializeObject<ContentItemResponseModel>(responseString);

            return contentItemReponse;
        }

        public async Task<ContentItemResponseModel> AddContentItemAsync(ContentItemPostModel contentItem)
        {

            var endpointUrl = UrlBuilder.BuildItemsUrl();

            string json = JsonConvert.SerializeObject(contentItem, Formatting.None, _serializeSettings);
            var body = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await GetContentManagementResponseAsync(endpointUrl, HttpMethod.Post, body);
            var responseString = await response.Content.ReadAsStringAsync();
            var contentItemReponse = JsonConvert.DeserializeObject<ContentItemResponseModel>(responseString);

            return contentItemReponse;
        }

        public async Task<ContentItemResponseModel> UpsertContentItemAsync(string externalId, ContentItemPostModel contentItem)
        {

            var endpointUrl = UrlBuilder.BuildItemsUrl();

            string json = JsonConvert.SerializeObject(contentItem, Formatting.None, _serializeSettings);
            var body = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await GetContentManagementResponseAsync(endpointUrl, HttpMethod.Post, body);
            var responseString = await response.Content.ReadAsStringAsync();
            var contentItemReponse = JsonConvert.DeserializeObject<ContentItemResponseModel>(responseString);

            return contentItemReponse;
        }

        public async Task<ContentItemResponseModel> GetContentItemAsync(ContentItemIdentifier identifier)
        {
            var endpointUrl = UrlBuilder.BuildItemUrl(identifier);

            var response = await GetContentManagementResponseAsync(endpointUrl, HttpMethod.Get);
            var responseString = await response.Content.ReadAsStringAsync();
            var contentItemReponse = JsonConvert.DeserializeObject<ContentItemResponseModel>(responseString);

            return contentItemReponse;
        }

        public async Task<HttpResponseMessage> DeleteContentItemAsync(ContentItemIdentifier contentItem)
        {
            var endpointUrl = UrlBuilder.BuildItemUrl(contentItem);

            return await GetContentManagementResponseAsync(endpointUrl, HttpMethod.Delete);
        }

        public async Task<HttpResponseMessage> ListContentItemsAsync()
        {

            var endpointUrl = UrlBuilder.BuildItemsUrl();

            return await GetContentManagementResponseAsync(endpointUrl, HttpMethod.Get);
        }

        public async Task<HttpResponseMessage> GetContentItemVariantAsync(ContentItemVariantIdentifier identifier)
        {
            var endpointUrl = UrlBuilder.BuildVariantsUrl(identifier);

            return await GetContentManagementResponseAsync(endpointUrl, HttpMethod.Get);
        }

        #endregion

        private async Task<HttpResponseMessage> GetContentManagementResponseAsync(string endpointUrl, HttpMethod method, StringContent body = null)
        {
            var message = new HttpRequestMessage(method, endpointUrl);
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _contentManagementOptions.ApiKey);

            message.Content = body;

            var response = await HttpClient.SendAsync(message);

            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            throw new ContentManagementException(response, await response.Content.ReadAsStringAsync());
        }
    }
}
