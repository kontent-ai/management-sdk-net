using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Kentico.Kontent.Management.Models.AssetRenditions;
using Kentico.Kontent.Management.Modules.ActionInvoker;
using Kentico.Kontent.Management.Modules.HttpClient;
using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.StronglyTyped;
using Kentico.Kontent.Management.Modules.ModelBuilders;
using Kentico.Kontent.Management.Models.ProjectReport;
using Kentico.Kontent.Management.Modules.ResiliencePolicy;
using Kentico.Kontent.Management.Models.Types;
using Kentico.Kontent.Management.Models.Types.Patch;
using Kentico.Kontent.Management.Models.TaxonomyGroups;
using Kentico.Kontent.Management.Models.TaxonomyGroups.Patch;
using Kentico.Kontent.Management.Models.Languages;
using Kentico.Kontent.Management.Models.Webhooks;
using Kentico.Kontent.Management.Models.Workflow;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Collections;
using Kentico.Kontent.Management.Models.TypeSnippets;
using Kentico.Kontent.Management.Models.TypeSnippets.Patch;
using Kentico.Kontent.Management.Models.Collections.Patch;
using Kentico.Kontent.Management.Models.Assets.Patch;
using Kentico.Kontent.Management.UrlBuilder;
using Kentico.Kontent.Management.Models.Roles;
using Kentico.Kontent.Management.Models.Users;
using Kentico.Kontent.Management.Models.Subscription;
using Kentico.Kontent.Management.Models.Environments;
using Kentico.Kontent.Management.Models.Environments.Patch;

namespace Kentico.Kontent.Management
{
    /// <summary>
    /// Executes requests against the Kontent Management API.
    /// </summary>
    public sealed partial class ManagementClient : IManagementClient
    {
        private const int MAX_FILE_SIZE_MB = 100;

        private readonly ActionInvoker _actionInvoker;
        private readonly EndpointUrlBuilder _urlBuilder;
        private readonly IModelProvider _modelProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagementClient"/> class for managing content of the specified project.
        /// </summary>
        /// <param name="ManagementOptions">The settings of the Kontent project.</param>
        public ManagementClient(ManagementOptions ManagementOptions)
        {
            if (ManagementOptions == null)
            {
                throw new ArgumentNullException(nameof(ManagementOptions));
            }

            if (string.IsNullOrEmpty(ManagementOptions.ProjectId))
            {
                throw new ArgumentException("Kontent project identifier is not specified.", nameof(ManagementOptions.ProjectId));
            }

            if (!Guid.TryParse(ManagementOptions.ProjectId, out _))
            {
                throw new ArgumentException($"Provided string is not a valid project identifier ({ManagementOptions.ProjectId}). Haven't you accidentally passed the API key instead of the project identifier?", nameof(ManagementOptions.ProjectId));
            }

            if (string.IsNullOrEmpty(ManagementOptions.ApiKey))
            {
                throw new ArgumentException("The API key is not specified.", nameof(ManagementOptions.ApiKey));
            }


            _urlBuilder = new EndpointUrlBuilder(ManagementOptions);
            _actionInvoker = new ActionInvoker(
                new ManagementHttpClient(new DefaultResiliencePolicyProvider(ManagementOptions.MaxRetryAttempts), ManagementOptions.EnableResilienceLogic),
                new MessageCreator(ManagementOptions.ApiKey));
            _modelProvider = ManagementOptions.ModelProvider ?? new ModelProvider();
        }

        internal ManagementClient(EndpointUrlBuilder urlBuilder, ActionInvoker actionInvoker, IModelProvider modelProvider = null)
        {
            _urlBuilder = urlBuilder ?? throw new ArgumentNullException(nameof(urlBuilder));
            _actionInvoker = actionInvoker ?? throw new ArgumentNullException(nameof(actionInvoker));
            _modelProvider = modelProvider ?? new ModelProvider();
        }

        /// <inheritdoc />
        public async Task<AssetModel> GetAssetAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildAssetsUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<AssetModel>(endpointUrl, HttpMethod.Get);

            return response;
        }

        /// <inheritdoc />
        public async Task<AssetModel<T>> GetAssetAsync<T>(Reference identifier) where T : new()
        {
            var response = await GetAssetAsync(identifier);

            return _modelProvider.GetAssetModel<T>(response);
        }

        /// <inheritdoc />
        public async Task<IListingResponseModel<AssetModel>> ListAssetsAsync()
        {
            var endpointUrl = _urlBuilder.BuildAssetsUrl();
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<AssetListingResponseServerModel>(endpointUrl, HttpMethod.Get);

            return new ListingResponseModel<AssetModel>(
                GetNextListingPageAsync<AssetListingResponseServerModel, AssetModel>,
                response.Pagination?.Token,
                endpointUrl,
                response.Assets);
        }

        /// <inheritdoc />
        public async Task<IListingResponseModel<AssetModel<T>>> ListAssetsAsync<T>() where T : new()
        {
            var endpointUrl = _urlBuilder.BuildAssetsUrl();
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<AssetListingResponseServerModel>(endpointUrl, HttpMethod.Get);

            return new ListingResponseMappedModel<AssetModel, AssetModel<T>>(
                GetNextListingPageAsync<AssetListingResponseServerModel, AssetModel>,
                response.Pagination?.Token,
                endpointUrl,
                response.Assets,
                _modelProvider.GetAssetModel<T>);
        }

        /// <inheritdoc />
        public async Task<AssetModel> CreateAssetAsync(AssetCreateModel asset)
        {
            if (asset == null)
            {
                throw new ArgumentNullException(nameof(asset));
            }

            var endpointUrl = _urlBuilder.BuildAssetsUrl();
            var response = await _actionInvoker.InvokeMethodAsync<AssetCreateModel, AssetModel>(endpointUrl, HttpMethod.Post, asset);

            return response;
        }

        /// <inheritdoc />
        public async Task<AssetModel<T>> CreateAssetAsync<T>(AssetCreateModel<T> asset) where T : new()
        {
            if (asset == null)
            {
                throw new ArgumentNullException(nameof(asset));
            }
            
            var result = await CreateAssetAsync(_modelProvider.GetAssetCreateModel(asset));

            return _modelProvider.GetAssetModel<T>(result);
        }

        /// <inheritdoc />
        public async Task<AssetModel> UpsertAssetAsync(Reference identifier, AssetUpsertModel asset)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            if (asset == null)
            {
                throw new ArgumentNullException(nameof(asset));
            }

            var endpointUrl = _urlBuilder.BuildAssetsUrl(identifier);
            var response = await _actionInvoker.InvokeMethodAsync<AssetUpsertModel, AssetModel>(endpointUrl, HttpMethod.Put, asset);

            return response;
        }

        /// <inheritdoc />
        public async Task<AssetModel<T>> UpsertAssetAsync<T>(Reference identifier, AssetUpsertModel<T> asset) where T : new()
        {
            if (asset == null)
            {
                throw new ArgumentNullException(nameof(asset));
            }

            var result = await UpsertAssetAsync(identifier, _modelProvider.GetAssetUpsertModel(asset));

            return _modelProvider.GetAssetModel<T>(result);
        }

        /// <inheritdoc />
        public async Task DeleteAssetAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildAssetsUrl(identifier);
            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }

        /// <inheritdoc />
        public async Task<FileReference> UploadFileAsync(FileContentSource fileContent)
        {
            if (fileContent == null)
            {
                throw new ArgumentNullException(nameof(fileContent));
            }

            var stream = fileContent.OpenReadStream();
            try
            {
                if (stream.Length > MAX_FILE_SIZE_MB * 1024 * 1024)
                {
                    throw new ArgumentException($"Maximum supported file size is {MAX_FILE_SIZE_MB} MB.", nameof(stream));
                }

                var endpointUrl = _urlBuilder.BuildUploadFileUrl(fileContent.FileName);
                var response = await _actionInvoker.UploadFileAsync<FileReference>(endpointUrl, stream, fileContent.ContentType);

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
        
                
        /// <inheritdoc />
        public async Task<AssetRenditionModel> GetAssetRenditionAsync(AssetRenditionIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }
            
            var endpointUrl = _urlBuilder.BuildAssetRenditionsUrl(identifier);
            return await _actionInvoker.InvokeReadOnlyMethodAsync<AssetRenditionModel>(endpointUrl, HttpMethod.Get);
        }

        /// <inheritdoc />
        public async Task<IListingResponseModel<AssetRenditionModel>> ListAssetRenditionsAsync(Reference assetIdentifier)
        {
            if (assetIdentifier == null)
            {
                throw new ArgumentNullException(nameof(assetIdentifier));
            }
            
            var endpointUrl = _urlBuilder.BuildAssetRenditionsUrl(assetIdentifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<AssetRenditionsListingResponseServerModel>(endpointUrl, HttpMethod.Get);
            
            return new ListingResponseModel<AssetRenditionModel>(
                GetNextListingPageAsync<AssetRenditionsListingResponseServerModel, AssetRenditionModel>,
                response.Pagination?.Token,
                endpointUrl,
                response.AssetRenditions);
        }

        /// <inheritdoc />
        public async Task<AssetRenditionModel> CreateAssetRenditionAsync(Reference assetIdentifier, AssetRenditionCreateModel createModel)
        {
            if (assetIdentifier == null)
            {
                throw new ArgumentNullException(nameof(assetIdentifier));
            }

            if (createModel == null)
            {
                throw new ArgumentNullException(nameof(createModel));
            }
            
            var endpointUrl = _urlBuilder.BuildAssetRenditionsUrl(assetIdentifier);
            return await _actionInvoker.InvokeMethodAsync<AssetRenditionCreateModel, AssetRenditionModel>(endpointUrl, HttpMethod.Post, createModel);
        }

        /// <inheritdoc />
        public async Task<AssetRenditionModel> UpdateAssetRenditionAsync(AssetRenditionIdentifier identifier, AssetRenditionUpdateModel updateModel)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }
            if (updateModel == null)
            {
                throw new ArgumentNullException(nameof(updateModel));
            }
            
            var endpointUrl = _urlBuilder.BuildAssetRenditionsUrl(identifier);
            return await _actionInvoker.InvokeMethodAsync<AssetRenditionUpdateModel, AssetRenditionModel>(endpointUrl, HttpMethod.Put, updateModel);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<LanguageVariantModel>> ListLanguageVariantsByItemAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildListVariantsByItemUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<IEnumerable<LanguageVariantModel>>(endpointUrl, HttpMethod.Get);

            return response;
        }

        /// <inheritdoc />
        public async Task<IListingResponseModel<LanguageVariantModel>> ListLanguageVariantsByTypeAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildListVariantsByTypeUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<LanguageVariantsListingResponseServerModel>(endpointUrl, HttpMethod.Get);

            return new ListingResponseModel<LanguageVariantModel>(
                    (token, url) => GetNextListingPageAsync<LanguageVariantsListingResponseServerModel, LanguageVariantModel>(token, url),
                    response.Pagination?.Token,
                    endpointUrl,
                    response.Variants);
        }

        /// <inheritdoc />
        public async Task<IListingResponseModel<LanguageVariantModel>> ListLanguageVariantsOfContentTypeWithComponentsAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildListVariantsByComponentUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<LanguageVariantsListingResponseServerModel>(endpointUrl, HttpMethod.Get);

            return new ListingResponseModel<LanguageVariantModel>(
                    (token, url) => GetNextListingPageAsync<LanguageVariantsListingResponseServerModel, LanguageVariantModel>(token, url),
                    response.Pagination?.Token,
                    endpointUrl,
                    response.Variants);
        }

        /// <inheritdoc />
        public async Task<IListingResponseModel<LanguageVariantModel>> ListLanguageVariantsByCollectionAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildListVariantsByCollectionUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<LanguageVariantsListingResponseServerModel>(endpointUrl, HttpMethod.Get);

            return new ListingResponseModel<LanguageVariantModel>(
                    (token, url) => GetNextListingPageAsync<LanguageVariantsListingResponseServerModel, LanguageVariantModel>(token, url),
                    response.Pagination?.Token,
                    endpointUrl,
                    response.Variants);
        }

        /// <inheritdoc />
        public async Task<LanguageVariantModel> GetLanguageVariantAsync(LanguageVariantIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<LanguageVariantModel>(endpointUrl, HttpMethod.Get);

            return response;
        }

        /// <inheritdoc />
        public async Task<LanguageVariantModel> UpsertLanguageVariantAsync(LanguageVariantIdentifier identifier, LanguageVariantUpsertModel languageVariantUpsertModel)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            if (languageVariantUpsertModel == null)
            {
                throw new ArgumentNullException(nameof(languageVariantUpsertModel));
            }

            var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
            var response = await _actionInvoker.InvokeMethodAsync<LanguageVariantUpsertModel, LanguageVariantModel>(endpointUrl, HttpMethod.Put, languageVariantUpsertModel);

            return response;
        }

        /// <inheritdoc />
        public async Task<LanguageVariantModel> UpsertLanguageVariantAsync(LanguageVariantIdentifier identifier, LanguageVariantModel languageVariant)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            if (languageVariant == null)
            {
                throw new ArgumentNullException(nameof(languageVariant));
            }

            var languageVariantUpsertModel = new LanguageVariantUpsertModel(languageVariant);

            return await UpsertLanguageVariantAsync(identifier, languageVariantUpsertModel);
        }

        /// <inheritdoc />
        public async Task DeleteLanguageVariantAsync(LanguageVariantIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }

        /// <inheritdoc />
        public async Task<List<LanguageVariantModel<T>>> ListLanguageVariantsByItemAsync<T>(Reference identifier) where T : new()
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildListVariantsByItemUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<List<LanguageVariantModel>>(endpointUrl, HttpMethod.Get);

            return response.Select(x => _modelProvider.GetLanguageVariantModel<T>(x)).ToList();
        }

        /// <inheritdoc />
        public async Task<LanguageVariantModel<T>> GetLanguageVariantAsync<T>(LanguageVariantIdentifier identifier) where T : new()
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<LanguageVariantModel>(endpointUrl, HttpMethod.Get);

            return _modelProvider.GetLanguageVariantModel<T>(response);
        }

        /// <inheritdoc />
        public async Task<LanguageVariantModel<T>> UpsertLanguageVariantAsync<T>(LanguageVariantIdentifier identifier, T variantElements, WorkflowStepIdentifier workflow = null) where T : new()
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            if (variantElements == null)
            {
                throw new ArgumentNullException(nameof(variantElements));
            }

            var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
            var variantUpsertModel = _modelProvider.GetLanguageVariantUpsertModel(variantElements, workflow);
            var response = await _actionInvoker.InvokeMethodAsync<LanguageVariantUpsertModel, LanguageVariantModel>(endpointUrl, HttpMethod.Put, variantUpsertModel);

            return _modelProvider.GetLanguageVariantModel<T>(response);
        }

        /// <inheritdoc />
        public async Task<IListingResponseModel<ContentTypeModel>> ListContentTypesAsync()
        {
            var endpointUrl = _urlBuilder.BuildTypeUrl();
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<ContentTypeListingResponseServerModel>(endpointUrl, HttpMethod.Get);

            return new ListingResponseModel<ContentTypeModel>(
                (token, url) => GetNextListingPageAsync<ContentTypeListingResponseServerModel, ContentTypeModel>(token, url),
                response.Pagination?.Token,
                endpointUrl,
                response.Types);
        }

        /// <inheritdoc />
        public async Task<ContentTypeModel> GetContentTypeAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildTypeUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<ContentTypeModel>(endpointUrl, HttpMethod.Get);

            return response;
        }

        /// <inheritdoc />
        public async Task<ContentTypeModel> CreateContentTypeAsync(ContentTypeCreateModel contentType)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            var endpointUrl = _urlBuilder.BuildTypeUrl();
            var response = await _actionInvoker.InvokeMethodAsync<ContentTypeCreateModel, ContentTypeModel>(endpointUrl, HttpMethod.Post, contentType);

            return response;
        }

        /// <inheritdoc />
        public async Task DeleteContentTypeAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildTypeUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }

        /// <inheritdoc />
        public async Task<ContentTypeModel> ModifyContentTypeAsync(Reference identifier, IEnumerable<ContentTypeOperationBaseModel> changes)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            if (changes == null || !changes.Any())
            {
                throw new ArgumentException("Please provide at least one operation.", nameof(changes));
            }

            var endpointUrl = _urlBuilder.BuildTypeUrl(identifier);
            return await _actionInvoker.InvokeMethodAsync<IEnumerable<ContentTypeOperationBaseModel>, ContentTypeModel>(endpointUrl, new HttpMethod("PATCH"), changes);
        }

        /// <inheritdoc />
        public async Task<IListingResponseModel<ContentTypeSnippetModel>> ListContentTypeSnippetsAsync()
        {
            var endpointUrl = _urlBuilder.BuildSnippetsUrl();
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<SnippetListingResponseServerModel>(endpointUrl, HttpMethod.Get);

            return new ListingResponseModel<ContentTypeSnippetModel>(
                (token, url) => GetNextListingPageAsync<SnippetListingResponseServerModel, ContentTypeSnippetModel>(token, url),
                response.Pagination?.Token,
                endpointUrl,
                response.Snippets);
        }

        /// <inheritdoc />
        public async Task<ContentTypeSnippetModel> GetContentTypeSnippetAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildSnippetsUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<ContentTypeSnippetModel>(endpointUrl, HttpMethod.Get);

            return response;
        }

        /// <inheritdoc />
        public async Task<ContentTypeSnippetModel> CreateContentTypeSnippetAsync(ContentTypeSnippetCreateModel contentTypeSnippet)
        {
            if (contentTypeSnippet == null)
            {
                throw new ArgumentNullException(nameof(contentTypeSnippet));
            }

            var endpointUrl = _urlBuilder.BuildSnippetsUrl();
            var response = await _actionInvoker.InvokeMethodAsync<ContentTypeSnippetCreateModel, ContentTypeSnippetModel>(endpointUrl, HttpMethod.Post, contentTypeSnippet);

            return response;
        }

        /// <inheritdoc />
        public async Task DeleteContentTypeSnippetAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildSnippetsUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }

        /// <inheritdoc />
        public async Task<ContentTypeSnippetModel> ModifyContentTypeSnippetAsync(Reference identifier, IEnumerable<ContentTypeSnippetOperationBaseModel> changes)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            if (changes == null || !changes.Any())
            {
                throw new ArgumentException("Please provide at least one operation.", nameof(changes));
            }

            var endpointUrl = _urlBuilder.BuildSnippetsUrl(identifier);
            return await _actionInvoker.InvokeMethodAsync<IEnumerable<ContentTypeSnippetOperationBaseModel>, ContentTypeSnippetModel>(endpointUrl, new HttpMethod("PATCH"), changes);
        }

        /// <inheritdoc />
        public async Task<IListingResponseModel<TaxonomyGroupModel>> ListTaxonomyGroupsAsync()
        {
            var endpointUrl = _urlBuilder.BuildTaxonomyUrl();
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<TaxonomyGroupListingResponseServerModel>(endpointUrl, HttpMethod.Get);

            return new ListingResponseModel<TaxonomyGroupModel>(
                (token, url) => GetNextListingPageAsync<TaxonomyGroupListingResponseServerModel, TaxonomyGroupModel>(token, url),
                response.Pagination?.Token,
                endpointUrl,
                response.Taxonomies);
        }

        /// <inheritdoc />
        public async Task<TaxonomyGroupModel> GetTaxonomyGroupAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildTaxonomyUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<TaxonomyGroupModel>(endpointUrl, HttpMethod.Get);

            return response;
        }

        /// <inheritdoc />
        public async Task<TaxonomyGroupModel> CreateTaxonomyGroupAsync(TaxonomyGroupCreateModel taxonomyGroup)
        {
            if (taxonomyGroup == null)
            {
                throw new ArgumentNullException(nameof(taxonomyGroup));
            }

            var endpointUrl = _urlBuilder.BuildTaxonomyUrl();
            return await _actionInvoker.InvokeMethodAsync<TaxonomyGroupCreateModel, TaxonomyGroupModel>(endpointUrl, HttpMethod.Post, taxonomyGroup);
        }

        /// <inheritdoc />
        public async Task DeleteTaxonomyGroupAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildTaxonomyUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }

        /// <inheritdoc />
        public async Task<TaxonomyGroupModel> ModifyTaxonomyGroupAsync(Reference identifier, IEnumerable<TaxonomyGroupOperationBaseModel> changes)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildTaxonomyUrl(identifier);
            return await _actionInvoker.InvokeMethodAsync<IEnumerable<TaxonomyGroupOperationBaseModel>, TaxonomyGroupModel>(endpointUrl, new HttpMethod("PATCH"), changes);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<WebhookModel>> ListWebhooksAsync()
        {
            var endpointUrl = _urlBuilder.BuildWebhooksUrl();
            return await _actionInvoker.InvokeReadOnlyMethodAsync<IEnumerable<WebhookModel>>(endpointUrl, HttpMethod.Get);
        }

        /// <inheritdoc />
        public async Task<WebhookModel> GetWebhookAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildWebhooksUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<WebhookModel>(endpointUrl, HttpMethod.Get);

            return response;
        }

        /// <inheritdoc />
        public async Task<WebhookModel> CreateWebhookAsync(WebhookCreateModel webhook)
        {
            if (webhook == null)
            {
                throw new ArgumentNullException(nameof(webhook));
            }

            var endpointUrl = _urlBuilder.BuildWebhooksUrl();
            return await _actionInvoker.InvokeMethodAsync<WebhookCreateModel, WebhookModel>(endpointUrl, HttpMethod.Post, webhook);
        }

        /// <inheritdoc />
        public async Task DeleteWebhookAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildWebhooksUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }

        /// <inheritdoc />
        public async Task EnableWebhookAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildWebhooksEnableUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }

        /// <inheritdoc />
        public async Task DisableWebhookAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildWebhooksDisableUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }

        /// <inheritdoc />
        public async Task<IListingResponseModel<LanguageModel>> ListLanguagesAsync()
        {
            var endpointUrl = _urlBuilder.BuildLanguagesUrl();
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<LanguagesListingResponseServerModel>(endpointUrl, HttpMethod.Get);

            return new ListingResponseModel<LanguageModel>(
                (token, url) => GetNextListingPageAsync<LanguagesListingResponseServerModel, LanguageModel>(token, url),
                response.Pagination?.Token,
                endpointUrl,
                response.Languages);
        }

        /// <inheritdoc />
        public async Task<LanguageModel> GetLanguageAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildLanguagesUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<LanguageModel>(endpointUrl, HttpMethod.Get);

            return response;
        }

        /// <inheritdoc />
        public async Task<LanguageModel> CreateLanguageAsync(LanguageCreateModel language)
        {
            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }

            var endpointUrl = _urlBuilder.BuildLanguagesUrl();
            return await _actionInvoker.InvokeMethodAsync<LanguageCreateModel, LanguageModel>(endpointUrl, HttpMethod.Post, language);
        }

        /// <inheritdoc />
        public async Task<LanguageModel> ModifyLanguageAsync(Reference identifier, IEnumerable<LanguagePatchModel> changes)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildLanguagesUrl(identifier);
            return await _actionInvoker.InvokeMethodAsync<IEnumerable<LanguagePatchModel>, LanguageModel>(endpointUrl, new HttpMethod("PATCH"), changes);
        }

        /// <inheritdoc />
        public async Task ChangeLanguageVariantWorkflowAsync(LanguageVariantIdentifier identifier, WorkflowStepIdentifier workflowStepIdentifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            if (workflowStepIdentifier == null)
            {
                throw new ArgumentNullException(nameof(workflowStepIdentifier));
            }

            var endpointUrl = _urlBuilder.BuildWorkflowChangeUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }

        /// <inheritdoc />
        public async Task PublishLanguageVariantAsync(LanguageVariantIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildPublishVariantUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }

        /// <inheritdoc />
        public async Task SchedulePublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier, ScheduleModel scheduleModel)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildPublishVariantUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put, scheduleModel);
        }

        /// <inheritdoc />
        public async Task CancelPublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildCancelPublishingVariantUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }

        /// <inheritdoc />
        public async Task UnpublishLanguageVariantAsync(LanguageVariantIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildUnpublishVariantUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }

        /// <inheritdoc />
        public async Task CancelUnpublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildCancelUnpublishingVariantUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }

        /// <inheritdoc />
        public async Task ScheduleUnpublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier, ScheduleModel scheduleModel)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildUnpublishVariantUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put, scheduleModel);
        }

        /// <inheritdoc />
        public async Task CreateNewVersionOfLanguageVariantAsync(LanguageVariantIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildNewVersionVariantUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }

        /// <inheritdoc />
        public async Task<ContentItemModel> UpsertContentItemAsync(Reference identifier, ContentItemUpsertModel contentItem)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            if (contentItem == null)
            {
                throw new ArgumentNullException(nameof(contentItem));
            }

            var endpointUrl = _urlBuilder.BuildItemUrl(identifier);
            var response = await _actionInvoker.InvokeMethodAsync<ContentItemUpsertModel, ContentItemModel>(endpointUrl, HttpMethod.Put, contentItem);

            return response;
        }

        /// <inheritdoc />
        public async Task<ContentItemModel> CreateContentItemAsync(ContentItemCreateModel contentItem)
        {
            if (contentItem == null)
            {
                throw new ArgumentNullException(nameof(contentItem));
            }

            var endpointUrl = _urlBuilder.BuildItemsUrl();
            var response = await _actionInvoker.InvokeMethodAsync<ContentItemCreateModel, ContentItemModel>(endpointUrl, HttpMethod.Post, contentItem);

            return response;
        }

        /// <inheritdoc />
        public async Task<ContentItemModel> GetContentItemAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildItemUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<ContentItemModel>(endpointUrl, HttpMethod.Get);

            return response;
        }

        /// <inheritdoc />
        public async Task DeleteContentItemAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildItemUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }

        /// <inheritdoc />
        public async Task<IListingResponseModel<ContentItemModel>> ListContentItemsAsync()
        {
            var endpointUrl = _urlBuilder.BuildItemsUrl();
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<ContentItemListingResponseServerModel>(endpointUrl, HttpMethod.Get);

            return new ListingResponseModel<ContentItemModel>(
                (token, url) => GetNextListingPageAsync<ContentItemListingResponseServerModel, ContentItemModel>(token, url),
                response.Pagination?.Token,
                endpointUrl,
                response.Items);
        }

        /// <inheritdoc />
        public async Task<AssetFoldersModel> GetAssetFoldersAsync()
        {
            var endpointUrl = _urlBuilder.BuildAssetFoldersUrl();
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<AssetFoldersModel>(endpointUrl, HttpMethod.Get);

            return response;
        }

        /// <inheritdoc />
        public async Task<AssetFoldersModel> CreateAssetFoldersAsync(AssetFolderCreateModel folder)
        {
            if (folder == null)
            {
                throw new ArgumentNullException(nameof(folder));
            }

            var endpointUrl = _urlBuilder.BuildAssetFoldersUrl();
            var response = await _actionInvoker.InvokeMethodAsync<AssetFolderCreateModel, AssetFoldersModel>(endpointUrl, HttpMethod.Post, folder);

            return response;
        }

        /// <inheritdoc />
        public async Task<AssetFoldersModel> ModifyAssetFoldersAsync(IEnumerable<AssetFolderOperationBaseModel> changes)
        {
            if (changes == null)
            {
                throw new ArgumentNullException(nameof(changes));
            }

            var endpointUrl = _urlBuilder.BuildAssetFoldersUrl();
            var response = await _actionInvoker.InvokeMethodAsync<IEnumerable<AssetFolderOperationBaseModel>, AssetFoldersModel>(endpointUrl, new HttpMethod("PATCH"), changes);

            return response;
        }

        /// <inheritdoc />
        public async Task<ProjectReportModel> ValidateProjectAsync()
        {
            var endpointUrl = _urlBuilder.BuildValidationUrl();
            return await _actionInvoker.InvokeReadOnlyMethodAsync<ProjectReportModel>(endpointUrl, HttpMethod.Post);
        }

        /// <inheritdoc />
        public async Task<Project> GetProjectInformationAsync()
        {
            var endpointUrl = _urlBuilder.BuildProjectUrl();
            return await _actionInvoker.InvokeReadOnlyMethodAsync<Project>(endpointUrl, HttpMethod.Get);
        }

        /// <inheritdoc />
        public async Task<CollectionsModel> ListCollectionsAsync()
        {
            var endpointUrl = _urlBuilder.BuildCollectionsUrl();
            return await _actionInvoker.InvokeReadOnlyMethodAsync<CollectionsModel>(endpointUrl, HttpMethod.Get);
        }

        /// <inheritdoc />
        public async Task<CollectionsModel> ModifyCollectionAsync(IEnumerable<CollectionOperationBaseModel> changes)
        {
            if (changes == null)
            {
                throw new ArgumentNullException(nameof(changes));
            }

            var endpointUrl = _urlBuilder.BuildCollectionsUrl();
            return await _actionInvoker.InvokeMethodAsync<IEnumerable<CollectionOperationBaseModel>, CollectionsModel>(endpointUrl, new HttpMethod("PATCH"), changes);
        }

        /// <inheritdoc />
        public async Task<UserModel> InviteUserIntoProjectAsync(UserInviteModel invitation)
        {
            if (invitation == null)
            {
                throw new ArgumentNullException(nameof(invitation));
            }

            var endpointUrl = _urlBuilder.BuildUsersUrl();
            return await _actionInvoker.InvokeMethodAsync<UserInviteModel, UserModel>(endpointUrl, HttpMethod.Post, invitation);
        }

        /// <inheritdoc />
        public async Task<UserModel> ModifyUsersRolesAsync(UserIdentifier identifier, UserModel user)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildModifyUsersRoleUrl(identifier);
            return await _actionInvoker.InvokeMethodAsync<UserModel, UserModel>(endpointUrl, HttpMethod.Put, user);
        }

        /// <inheritdoc />
        public async Task<ProjectRolesModel> ListProjectRolesAsync()
        {
            var endpointUrl = _urlBuilder.BuildProjectRolesUrl();
            return await _actionInvoker.InvokeReadOnlyMethodAsync<ProjectRolesModel>(endpointUrl, HttpMethod.Get);
        }

        /// <inheritdoc />
        public async Task<ProjectRoleModel> GetProjectRoleAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildProjectRoleUrl(identifier);
            return await _actionInvoker.InvokeReadOnlyMethodAsync<ProjectRoleModel>(endpointUrl, HttpMethod.Get);
        }

        /// <inheritdoc />
        public async Task<IListingResponseModel<SubscriptionProjectModel>> ListSubscriptionProjectsAsync()
        {
            var endpointUrl = _urlBuilder.BuildSubscriptionProjectsUrl();
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<SubscriptionProjectListingResponseServerModel>(endpointUrl, HttpMethod.Get);

            return new ListingResponseModel<SubscriptionProjectModel>(
                (token, url) => GetNextListingPageAsync<SubscriptionProjectListingResponseServerModel, SubscriptionProjectModel>(token, url),
                response.Pagination?.Token,
                endpointUrl,
                response.Projects);
        }

        /// <inheritdoc />
        public async Task<IListingResponseModel<SubscriptionUserModel>> ListSubscriptionUsersAsync()
        {
            var endpointUrl = _urlBuilder.BuildSubscriptionUsersUrl();
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<SubscriptionUserListingResponseServerModel>(endpointUrl, HttpMethod.Get);

            return new ListingResponseModel<SubscriptionUserModel>(
                (token, url) => GetNextListingPageAsync<SubscriptionUserListingResponseServerModel, SubscriptionUserModel>(token, url),
                response.Pagination?.Token,
                endpointUrl,
                response.Users);
        }

        /// <inheritdoc />
        public async Task<SubscriptionUserModel> GetSubscriptionUserAsync(UserIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildSubscriptionUserUrl(identifier);

            return await _actionInvoker.InvokeReadOnlyMethodAsync<SubscriptionUserModel>(endpointUrl, HttpMethod.Get);
        }

        /// <inheritdoc />
        public async Task ActivateSubscriptionUserAsync(UserIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildSubscriptionUserActivateUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }

        /// <inheritdoc />
        public async Task DeactivateSubscriptionUserAsync(UserIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildSubscriptionUserDeactivateDisableUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }

        /// <inheritdoc />
        public async Task<EnvironmentClonedModel> CloneEnvironmentAsync(EnvironmentCloneModel cloneEnvironmentModel)
        {
            if (cloneEnvironmentModel == null)
            {
                throw new ArgumentNullException(nameof(cloneEnvironmentModel));
            }

            var endpointUrl = _urlBuilder.BuildCloneEnvironmentUrl();
            return await _actionInvoker.InvokeMethodAsync<EnvironmentCloneModel, EnvironmentClonedModel>(endpointUrl, HttpMethod.Post, cloneEnvironmentModel);
        }

        /// <inheritdoc />
        public async Task<EnvironmentCloningStateModel> GetEnvironmentCloningStateAsync()
        {
            var endpointUrl = _urlBuilder.BuildGetEnvironmentCloningStateUrl();
            return await _actionInvoker.InvokeReadOnlyMethodAsync<EnvironmentCloningStateModel>(endpointUrl, HttpMethod.Get);
        }

        /// <inheritdoc />
        public async Task DeleteEnvironmentAsync()
        {
            var endpointUrl = _urlBuilder.BuildProjectUrl();
            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }

        /// <inheritdoc />
        public async Task MarkEnvironmentAsProductionAsync(MarkAsProductionModel markAsProductionModel)
        {
            if (markAsProductionModel == null)
            {
                throw new ArgumentNullException(nameof(markAsProductionModel));
            }

            var endpointUrl = _urlBuilder.BuildMarkEnvironmentAsProductionUrl();
            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put, markAsProductionModel);
        }

        /// <inheritdoc />
        public async Task<EnvironmentModel> ModifyEnvironmentAsync(IEnumerable<EnvironmentOperationBaseModel> changes)
        {
            if (changes == null)
            {
                throw new ArgumentNullException(nameof(changes));
            }

            var endpointUrl = _urlBuilder.BuildProjectUrl();
            return await _actionInvoker.InvokeMethodAsync<IEnumerable<EnvironmentOperationBaseModel>, EnvironmentModel>(endpointUrl, new HttpMethod("PATCH"), changes);
        }

        private async Task<IListingResponse<TModel>> GetNextListingPageAsync<TListingResponse, TModel>(string continuationToken, string url)
            where TListingResponse : IListingResponse<TModel>
        {
            var headers = new Dictionary<string, string>
            {
                { "x-continuation", continuationToken }
            };
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<TListingResponse>(url, HttpMethod.Get, headers);

            return response;
        }
    }
}
