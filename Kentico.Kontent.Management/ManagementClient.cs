using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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

namespace Kentico.Kontent.Management
{
    /// <summary>
    /// Executes requests against the Kentico Kontent Content Management API.
    /// </summary>
    public sealed class ManagementClient : IManagementClient
    {
        private const int MAX_FILE_SIZE_MB = 100;

        private readonly ActionInvoker _actionInvoker;
        private readonly EndpointUrlBuilder _urlBuilder;
        private readonly IModelProvider _modelProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagementClient"/> class for managing content of the specified project.
        /// </summary>
        /// <param name="ManagementOptions">The settings of the Kentico Kontent project.</param>
        public ManagementClient(ManagementOptions ManagementOptions)
        {
            if (ManagementOptions == null)
            {
                throw new ArgumentNullException(nameof(ManagementOptions));
            }

            if (string.IsNullOrEmpty(ManagementOptions.ProjectId))
            {
                throw new ArgumentException("Kentico Kontent project identifier is not specified.", nameof(ManagementOptions.ProjectId));
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
        public async Task<ListingResponseModel<LanguageVariantModel>> ListLanguageVariantsByTypeAsync(Reference identifier)
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
        public async Task<ListingResponseModel<LanguageVariantModel>> ListLanguageVariantsOfContentTypeWithComponentsAsync(Reference identifier)
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
        public async Task<ListingResponseModel<LanguageVariantModel>> ListLanguageVariantsByCollectionAsync(Reference identifier)
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
        public async Task<ListingResponseModel<ContentTypeModel>> ListContentTypesAsync()
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

            var endpointUrl = _urlBuilder.BuildTypeUrl(identifier);
            return await _actionInvoker.InvokeMethodAsync<IEnumerable<ContentTypeOperationBaseModel>, ContentTypeModel>(endpointUrl, new HttpMethod("PATCH"), changes);
        }

        /// <inheritdoc />
        public async Task<ListingResponseModel<ContentTypeSnippetModel>> ListContentTypeSnippetsAsync()
        {
            var endpointUrl = _urlBuilder.BuildSnippetsUrl();
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<SnippetsListingResponseServerModel>(endpointUrl, HttpMethod.Get);

            return new ListingResponseModel<ContentTypeSnippetModel>(
                (token, url) => GetNextListingPageAsync<SnippetsListingResponseServerModel, ContentTypeSnippetModel>(token, url),
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
        public async Task<ContentTypeSnippetModel> CreateContentTypeSnippetAsync(CreateContentSnippetCreateModel contentTypeSnippet)
        {
            if (contentTypeSnippet == null)
            {
                throw new ArgumentNullException(nameof(contentTypeSnippet));
            }

            var endpointUrl = _urlBuilder.BuildSnippetsUrl();
            var response = await _actionInvoker.InvokeMethodAsync<CreateContentSnippetCreateModel, ContentTypeSnippetModel>(endpointUrl, HttpMethod.Post, contentTypeSnippet);

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

            var endpointUrl = _urlBuilder.BuildSnippetsUrl(identifier);
            return await _actionInvoker.InvokeMethodAsync<IEnumerable<ContentTypeSnippetOperationBaseModel>, ContentTypeSnippetModel>(endpointUrl, new HttpMethod("PATCH"), changes);
        }

        /// <inheritdoc />
        public async Task<ListingResponseModel<TaxonomyGroupModel>> ListTaxonomyGroupsAsync()
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
        public async Task<ListingResponseModel<LanguageModel>> ListLanguagesAsync()
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
        public async Task<IEnumerable<WorkflowStepModel>> ListWorkflowStepsAsync()
        {
            var endpointUrl = _urlBuilder.BuildWorkflowUrl();
            return await _actionInvoker.InvokeReadOnlyMethodAsync<IEnumerable<WorkflowStepModel>>(endpointUrl, HttpMethod.Get);
        }

        /// <inheritdoc />
        public async Task ChangeLanguageVariantWorkflowStep(WorkflowIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildWorkflowChangeUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }

        /// <inheritdoc />
        public async Task PublishLanguageVariant(LanguageVariantIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildPublishVariantUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }

        /// <inheritdoc />
        public async Task SchedulePublishingOfLanguageVariant(LanguageVariantIdentifier identifier, ScheduleModel scheduleModel)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildPublishVariantUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put, scheduleModel);
        }

        /// <inheritdoc />
        public async Task CancelPublishingOfLanguageVariant(LanguageVariantIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildCancelPublishingVariantUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }

        /// <inheritdoc />
        public async Task UnpublishLanguageVariant(LanguageVariantIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildUnpublishVariantUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }

        /// <inheritdoc />
        public async Task CancelUnpublishingOfLanguageVariant(LanguageVariantIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildCancelUnpublishingVariantUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }

        /// <inheritdoc />
        public async Task ScheduleUnpublishingOfLanguageVariant(LanguageVariantIdentifier identifier, ScheduleModel scheduleModel)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildUnpublishVariantUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put, scheduleModel);
        }

        /// <inheritdoc />
        public async Task CreateNewVersionOfLanguageVariant(LanguageVariantIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildNewVersionVariantUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
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
        public async Task<LanguageVariantModel<T>> UpsertLanguageVariantAsync<T>(LanguageVariantIdentifier identifier, T variantElements) where T : new()
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
            var variantUpsertModel = _modelProvider.GetLanguageVariantUpsertModel(variantElements);
            var response = await _actionInvoker.InvokeMethodAsync<LanguageVariantUpsertModel, LanguageVariantModel>(endpointUrl, HttpMethod.Put, variantUpsertModel);

            return _modelProvider.GetLanguageVariantModel<T>(response);
        }

        /// <inheritdoc />
        public async Task<ContentItemModel> UpdateContentItemAsync(Reference identifier, ContentItemUpdateModel contentItem)
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
            var response = await _actionInvoker.InvokeMethodAsync<ContentItemUpdateModel, ContentItemModel>(endpointUrl, HttpMethod.Put, contentItem);

            return response;
        }

        /// <inheritdoc />
        public async Task<ContentItemModel> UpsertContentItemByExternalIdAsync(string externalId, ContentItemUpsertModel contentItem)
        {
            if (string.IsNullOrEmpty(externalId))
            {
                throw new ArgumentException("The external id is not specified.", nameof(externalId));
            }

            if (contentItem == null)
            {
                throw new ArgumentNullException(nameof(contentItem));
            }

            var endpointUrl = _urlBuilder.BuildItemUrl(Reference.ByExternalId(externalId));
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
        public async Task<ListingResponseModel<ContentItemModel>> ListContentItemsAsync()
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
        public async Task<ListingResponseModel<AssetModel>> ListAssetsAsync()
        {
            var endpointUrl = _urlBuilder.BuildAssetsUrl();
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<AssetListingResponseServerModel>(endpointUrl, HttpMethod.Get);

            return new ListingResponseModel<AssetModel>(
                (token, url) => GetNextListingPageAsync<AssetListingResponseServerModel, AssetModel>(token, url),
                response.Pagination?.Token,
                endpointUrl,
                response.Assets);
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
        public async Task<AssetModel> UpdateAssetAsync(Reference identifier, AssetUpdateModel asset)
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
            var response = await _actionInvoker.InvokeMethodAsync<AssetUpdateModel, AssetModel>(endpointUrl, HttpMethod.Put, asset);

            return response;
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
        public async Task<AssetModel> UpsertAssetByExternalIdAsync(string externalId, AssetUpsertModel asset)
        {
            if (string.IsNullOrEmpty(externalId))
            {
                throw new ArgumentException("The external id is not specified.", nameof(externalId));
            }

            if (asset == null)
            {
                throw new ArgumentNullException(nameof(asset));
            }

            var endpointUrl = _urlBuilder.BuildAssetsUrl(Reference.ByExternalId(externalId));
            var response = await _actionInvoker.InvokeMethodAsync<AssetUpsertModel, AssetModel>(
                endpointUrl,
                HttpMethod.Put,
                asset
            );

            return response;
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
        public async Task<ProjectReportModel> ValidateProjectAsync()
        {
            var endpointUrl = _urlBuilder.BuildValidationUrl();
            return await _actionInvoker.InvokeReadOnlyMethodAsync<ProjectReportModel>(endpointUrl, HttpMethod.Post);
        }

        /// <inheritdoc />
        public async Task<Project> GetProjectInformation()
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
