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
    public sealed class ManagementClient
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

        #region Variants

        /// <summary>
        /// Returns strongly typed listing of language variants for specified content item.
        /// </summary>
        /// <param name="identifier">The identifier of the content item.</param>
        /// <returns>The <see cref="IEnumerable{LanguageVariantModel}"/> instance that represents the listing of language variants.</returns>
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

        /// <summary>
        /// Returns strongly typed listing of language variants for specified content type.
        /// </summary>
        /// <param name="identifier">The identifier of the content type.</param>
        /// <returns>The <see cref="IEnumerable{LanguageVariantModel}"/> instance that represents the listing of language variants.</returns>
        public async Task<IEnumerable<LanguageVariantModel>> ListLanguageVariantsByTypeAsync(Reference identifier)
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

        /// <summary>
        /// Returns strongly typed listing of language variants containing components by type.
        /// </summary>
        /// <param name="identifier">The identifier of the content type.</param>
        /// <returns>The <see cref="IEnumerable{LanguageVariantModel}"/> instance that represents the listing of language variants.</returns>
        public async Task<IEnumerable<LanguageVariantModel>> ListLanguageVariantComponentByTypeAsync(Reference identifier)
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

        /// <summary>
        /// Returns strongly typed listing of language variants for specified collection.
        /// </summary>
        /// <param name="identifier">The identifier of the collection.</param>
        /// <returns>The <see cref="IEnumerable{LanguageVariantModel}"/> instance that represents the listing of language variants.</returns>
        public async Task<IEnumerable<LanguageVariantModel>> ListLanguageVariantsByCollectionAsync(Reference identifier)
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

        /// <summary>
        /// Returns strongly typed language variant.
        /// </summary>
        /// <param name="identifier">The identifier of the language variant.</param>
        /// <returns>The <see cref="LanguageVariantModel"/> instance that represents language variant.</returns>
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

        /// <summary>
        /// Inserts or updates given language variant.
        /// </summary>
        /// <param name="identifier">The identifier of the language variant.</param>
        /// <param name="languageVariantUpsertModel">Represents inserted or updated language variant.</param>
        /// <returns>The <see cref="LanguageVariantModel"/> instance that represents inserted or updated language variant.</returns>
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

        /// <summary>
        /// Deletes given language variant.
        /// </summary>
        /// <param name="identifier">The identifier of the language variant.</param>
        public async Task DeleteLanguageVariantAsync(LanguageVariantIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }

        #endregion

        #region Types

        /// <summary>
        /// Returns listing of content type.
        /// </summary>
        /// <returns>The <see cref="ListingResponseModel{ContentTypeModel}"/> instance that represents the listing of content types.</returns>
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

        /// <summary>
        /// Returns strongly typed content type.
        /// </summary>
        /// <param name="identifier">The identifier of the content type.</param>
        /// <returns>The <see cref="ContentTypeModel"/> instance that represents requested content type.</returns>
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

        /// <summary>
        /// Creates content type.
        /// </summary>
        /// <param name="contentType">Represents content type which will be created.</param>
        /// <returns>The <see cref="ContentTypeModel"/> instance that represents created content type.</returns>
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

        /// <summary>
        /// Deletes given content type.
        /// </summary>
        /// <param name="identifier">The identifier of the content type.</param>
        public async Task DeleteContentTypeAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildTypeUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }

        /// <summary>
        /// Modifies content type.
        /// </summary>
        /// <param name="identifier">The identifier of the content type.</param>
        /// /// <param name="changes">Represents changes that will be apply to the content type.</param>
        public async Task<ContentTypeModel> ModifyContentTypeAsync(Reference identifier, IEnumerable<ContentTypeOperationBaseModel> changes)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildTypeUrl(identifier);
            return await _actionInvoker.InvokeMethodAsync<IEnumerable<ContentTypeOperationBaseModel>, ContentTypeModel>(endpointUrl, new HttpMethod("PATCH"), changes);
        }

        #endregion

        #region TypeSnippets

        /// <summary>
        /// Returns listing of content type snippets.
        /// </summary>
        /// <returns>The <see cref="ListingResponseModel{ContentTypeSnippetModel}"/> instance that represents the listing of content type snippets.</returns>
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

        /// <summary>
        /// Returns content type snippet.
        /// </summary>
        /// <param name="identifier">The identifier of the content type snippet.</param>
        /// <returns>The <see cref="ContentTypeSnippetModel"/> instance that represents requested content type snippet.</returns>
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

        /// <summary>
        /// Creates content type snippet.
        /// </summary>
        /// <param name="contentTypeSnippet">Represents content type snippet which will be created.</param>
        /// <returns>The <see cref="ContentTypeSnippetModel"/> instance that represents created content type snippet.</returns>
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

        /// <summary>
        /// Deletes given content type snippet.
        /// </summary>
        /// <param name="identifier">The identifier of the content type snippet.</param>
        public async Task DeleteContentTypeSnippetAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildSnippetsUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }

        /// <summary>
        /// Modifies content type snippet.
        /// </summary>
        /// <param name="identifier">The identifier of the content type snippet.</param>
        /// <param name="changes">Represents changes that will be apply to the content type snippet.</param>
        public async Task<ContentTypeSnippetModel> ModifyContentTypeSnippetAsync(Reference identifier, IEnumerable<ContentTypeSnippetOperationBaseModel> changes)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildSnippetsUrl(identifier);
            return await _actionInvoker.InvokeMethodAsync<IEnumerable<ContentTypeSnippetOperationBaseModel>, ContentTypeSnippetModel>(endpointUrl, new HttpMethod("PATCH"), changes);
        }

        #endregion

        #region TaxonomyGroups

        /// <summary>
        /// Returns listing of taxonomy groups.
        /// </summary>
        /// <returns>The <see cref="ListingResponseModel{TaxonomyGroupModel}"/> instance that represents the listing of taxonomy groups.</returns>
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

        /// <summary>
        /// Returns taxonomy group.
        /// </summary>
        /// <param name="identifier">The identifier of the taxonomy group.</param>
        /// <returns>The <see cref="TaxonomyGroupModel"/> instance that represents requested taxonomy group.</returns>
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

        /// <summary>
        /// Creates taxonomy group.
        /// </summary>
        /// <param name="taxonomyGroup">Represents taxonomy group which will be created.</param>
        /// <returns>The <see cref="TaxonomyGroupModel"/> instance that represents created taxonomy group.</returns>
        public async Task<TaxonomyGroupModel> CreateTaxonomyGroupAsync(TaxonomyGroupCreateModel taxonomyGroup)
        {
            if (taxonomyGroup == null)
            {
                throw new ArgumentNullException(nameof(taxonomyGroup));
            }

            var endpointUrl = _urlBuilder.BuildTaxonomyUrl();
            return await _actionInvoker.InvokeMethodAsync<TaxonomyGroupCreateModel, TaxonomyGroupModel>(endpointUrl, HttpMethod.Post, taxonomyGroup);
        }

        /// <summary>
        /// Deletes given taxonomy group.
        /// </summary>
        /// <param name="identifier">The identifier of the taxonomy group.</param>
        public async Task DeleteTaxonomyGroupAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildTaxonomyUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }

        /// <summary>
        /// Modifies given taxonomy group.
        /// </summary>
        /// <param name="identifier">The identifier of the taxonomy group.</param>
        /// <param name="changes">Represents changes that will be apply to the taxonomy group.</param>
        /// <returns>The <see cref="TaxonomyGroupModel"/> instance that represents the created taxonomy group.</returns>
        public async Task<TaxonomyGroupModel> ModifyTaxonomyGroupAsync(Reference identifier, IEnumerable<TaxonomyGroupOperationBaseModel> changes)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildTaxonomyUrl(identifier);
            return await _actionInvoker.InvokeMethodAsync<IEnumerable<TaxonomyGroupOperationBaseModel>, TaxonomyGroupModel>(endpointUrl, new HttpMethod("PATCH"), changes);
        }

        #endregion

        #region Webhooks

        /// <summary>
        /// Returns listing of webhooks.
        /// </summary>
        /// <returns>The <see cref="IEnumerable{WebhookModel}"/> instance that represents the listing of webhooks.</returns>
        public async Task<IEnumerable<WebhookModel>> ListWebhooksAsync()
        {
            var endpointUrl = _urlBuilder.BuildWebhooksUrl();
            return await _actionInvoker.InvokeReadOnlyMethodAsync<IEnumerable<WebhookModel>>(endpointUrl, HttpMethod.Get);
        }

        /// <summary>
        /// Returns the webhook.
        /// </summary>
        /// <param name="identifier">The identifier of the webhook.</param>
        /// <returns>The <see cref="WebhookModel"/> instance that represents requested webhook.</returns>
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

        /// <summary>
        /// Creates the webhook.
        /// </summary>
        /// <param name="webhook">The webhook to be created.</param>
        /// <returns>The <see cref="WebhookModel"/> instance that represents created webhook.</returns>
        public async Task<WebhookModel> CreateWebhookAsync(WebhookCreateModel webhook)
        {
            if (webhook == null)
            {
                throw new ArgumentNullException(nameof(webhook));
            }

            var endpointUrl = _urlBuilder.BuildWebhooksUrl();
            return await _actionInvoker.InvokeMethodAsync<WebhookCreateModel, WebhookModel>(endpointUrl, HttpMethod.Post, webhook);
        }

        /// <summary>
        /// Deletes the webhook.
        /// </summary>
        /// <param name="identifier">The identifier of the webhook.</param>
        public async Task DeleteWebhookAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildWebhooksUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }

        /// <summary>
        /// Enables the webhook.
        /// </summary>
        /// <param name="identifier">The identifier of the webhook.</param>
        public async Task EnableWebhookAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildWebhooksEnableUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }

        /// <summary>
        /// Disables the webhook.
        /// </summary>
        /// <param name="identifier">The identifier of the webhook.</param>
        public async Task DisableWebhookAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildWebhooksDisableUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }
        #endregion Webhooks

        #region Languages

        /// <summary>
        /// Returns listing of languages.
        /// </summary>
        /// <returns>The <see cref="ListingResponseModel{LanguageModel}"/> instance that represents the listing of languages.</returns>
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

        /// <summary>
        /// Returns the language.
        /// </summary>
        /// <param name="identifier">The identifier of the language.</param>
        /// <returns>The <see cref="LanguageModel"/> instance that represents requested language.</returns>
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

        /// <summary>
        /// Creates the language.
        /// </summary>
        /// <param name="language">The language to be created.</param>
        /// <returns>The <see cref="LanguageModel"/> instance that represents created language.</returns>
        public async Task<LanguageModel> CreateLanguageAsync(LanguageCreateModel language)
        {
            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }

            var endpointUrl = _urlBuilder.BuildLanguagesUrl();
            return await _actionInvoker.InvokeMethodAsync<LanguageCreateModel, LanguageModel>(endpointUrl, HttpMethod.Post, language);
        }

        /// <summary>
        /// Modifies the language.
        /// </summary>
        /// <param name="identifier">The language to be modified.</param>
        /// <param name="changes">Represents changes that will be apply to the language.</param>
        /// <returns>The <see cref="LanguageModel"/> instance that represents modified language.</returns>
        public async Task<LanguageModel> ModifyLanguageAsync(Reference identifier, IEnumerable<LanguagePatchModel> changes)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildLanguagesUrl(identifier);
            return await _actionInvoker.InvokeMethodAsync<IEnumerable<LanguagePatchModel>, LanguageModel>(endpointUrl, new HttpMethod("PATCH"), changes);
        }

        #endregion

        #region WorkflowSteps

        /// <summary>
        /// Returns listing of workflow steps.
        /// </summary>
        /// <returns>The <see cref="IEnumerable{WorkflowStepModel}"/> instance that represents the listing of workflow steps.</returns>
        public async Task<IEnumerable<WorkflowStepModel>> ListWorkflowStepsAsync()
        {
            var endpointUrl = _urlBuilder.BuildWorkflowUrl();
            return await _actionInvoker.InvokeReadOnlyMethodAsync<IEnumerable<WorkflowStepModel>>(endpointUrl, HttpMethod.Get);
        }

        /// <summary>
        /// Changes workflow step.
        /// </summary>
        /// <param name="identifier">The workflow step to be changed.</param>
        public async Task ChangeWorkflowStep(WorkflowIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildWorkflowChangeUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }

        /// <summary>
        /// Publishes the language variant.
        /// </summary>
        /// <param name="identifier">The language variant to be published.</param>
        public async Task PublishLanguageVariant(LanguageVariantIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildPublishVariantUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }

        /// <summary>
        /// Schedules publishing of the language variant.
        /// </summary>
        /// <param name="identifier">The identifier of the language variant  to be published.</param>
        /// <param name="scheduleModel">The time when the language variant will be published</param>
        public async Task SchedulePublishingOfLangaugeVariant(LanguageVariantIdentifier identifier, ScheduleModel scheduleModel)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildPublishVariantUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put, scheduleModel);
        }

        /// <summary>
        /// Cancels publishing of the language variant.
        /// </summary>
        /// <param name="identifier">The identifier of the language variant identifier of which publishing should be canceled.</param>
        public async Task CancelPublishingOfLanguageVariant(LanguageVariantIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildCancelPublishingVariantUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }

        /// <summary>
        /// Unpublishes the language variant.
        /// </summary>
        /// <param name="identifier">The language variant to be unpublished.</param>
        public async Task UnpublishLangaugeVariant(LanguageVariantIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildUnpublishVariantUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }

        /// <summary>
        /// Cancels unpublishing of the language variant.
        /// </summary>
        /// <param name="identifier">The identifier of the language variant identifier of which unpublishing should be canceled.</param>
        public async Task CancelUnpublishingOfLanguageVariant(LanguageVariantIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildCancelUnpublishingVariantUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }

        /// <summary>
        /// Schedules unpublishing of the language variant.
        /// </summary>
        /// <param name="identifier">The identifier of the language variant  to be unpublished.</param>
        /// <param name="scheduleModel">The time when the language variant will be unpublished</param>
        public async Task ScheduleUnpublishingOfLanguageVariant(LanguageVariantIdentifier identifier, ScheduleModel scheduleModel)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildUnpublishVariantUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put, scheduleModel);
        }

        /// <summary>
        /// Creates the new version of the language variant.
        /// </summary>
        /// <param name="identifier">The identifier of the language variant for which the new version should created .</param>
        public async Task CreateNewVersionOfLanguageVariant(LanguageVariantIdentifier identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildNewVersionVariantUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
        }
        #endregion

        #region Strongly typed Variants

        /// <summary>
        /// Returns strongly typed listing of language variants with strongly typed elements for specified content item.
        /// </summary>
        /// <typeparam name="T">Type of the content item elements</typeparam>
        /// <param name="identifier">The identifier of the content item.</param>
        /// <returns>A strongly-typed collection with language variants.</returns>
        public async Task<List<LanguageVariantModel<T>>> ListLanguageVariantsAsync<T>(Reference identifier) where T : new()
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildListVariantsByItemUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<List<LanguageVariantModel>>(endpointUrl, HttpMethod.Get);

            return response.Select(x => _modelProvider.GetLanguageVariantModel<T>(x)).ToList();
        }

        /// <summary>
        /// Returns strongly typed language variant with strongly typed elements.
        /// </summary>
        /// <typeparam name="T">Type of the content item elements</typeparam>
        /// <param name="identifier">The identifier of the language variant.</param>
        /// <returns>The <see cref="LanguageVariantModel{T}"/> instance that represents language variant.</returns>
        public async Task<LanguageVariantModel<T>> GetLangaugeVariantAsync<T>(LanguageVariantIdentifier identifier) where T : new()
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<LanguageVariantModel>(endpointUrl, HttpMethod.Get);

            return _modelProvider.GetLanguageVariantModel<T>(response);
        }

        /// <summary>
        /// Inserts or updates given language variant.
        /// </summary>
        /// <typeparam name="T">Type of the content item elements</typeparam>
        /// <param name="identifier">The identifier of the language variant.</param>
        /// <param name="variantElements">Represents inserted or updated  strongly typed language variant elements.</param>
        /// <returns>The <see cref="LanguageVariantModel{T}"/> instance that represents inserted or updated language variant.</returns>
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

        #endregion

        #region Items

        /// <summary>
        /// Updates given content item.
        /// </summary>
        /// <param name="identifier">The identifier of the content item.</param>
        /// <param name="contentItem">Represents updated content item.</param>
        /// <returns>The <see cref="ContentItemModel"/> instance that represents updated content item.</returns>
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

        /// <summary>
        /// Inserts or updates content item according to external identifier.
        /// </summary>
        /// <param name="externalId">The external identifier of the content item.</param>
        /// <param name="contentItem">Represents inserted or updated content item.</param>
        /// <returns>The <see cref="ContentItemModel"/> instance that represents inserted or updated content item.</returns>
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

        /// <summary>
        /// Creates content item.
        /// </summary>
        /// <param name="contentItem">Represents content item which will be created.</param>
        /// <returns>The <see cref="ContentItemModel"/> instance that represents created content item.</returns>
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

        /// <summary>
        /// Returns strongly typed content item.
        /// </summary>
        /// <param name="identifier">The identifier of the content item.</param>
        /// <returns>The <see cref="ContentItemModel"/> instance that represents requested content item.</returns>
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

        /// <summary>
        /// Deletes given content item.
        /// </summary>
        /// <param name="identifier">The identifier of the content item.</param>
        public async Task DeleteContentItemAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildItemUrl(identifier);

            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }

        /// <summary>
        /// Returns strongly typed listing of content items. 
        /// The Content management API returns a dynamically paginated listing response limited to up to 100 objects. 
        /// To check if the next page is available use <see cref="ListingResponseModel{T}.HasNextPage"/>,
        /// For getting next page use <see cref="ListingResponseModel{T}.GetNextPage"/>,
        /// </summary>
        /// <returns>The <see cref="ListingResponseModel{ContentItemModel}"/> instance that represents the listing of content items.</returns>
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

        #endregion

        #region Assets

        /// <summary>
        /// Returns strongly typed listing of assets.
        /// The Content management API returns a dynamically paginated listing response limited to up to 100 objects. 
        /// To check if the next page is available use <see cref="ListingResponseModel{T}.HasNextPage"/>,
        /// For getting next page use <see cref="ListingResponseModel{T}.GetNextPage"/>,
        /// </summary>
        /// <returns>The <see cref="ListingResponseModel{AssetModel}"/> instance that represents the listing of assets.</returns>
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

        /// <summary>
        /// Returns strongly typed asset.
        /// </summary>
        /// <param name="identifier">The identifier of the asset.</param>
        /// <returns>The <see cref="AssetModel"/> instance that represents requested asset.</returns>
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

        /// <summary>
        /// Get the Asset Folders
        /// </summary>
        /// <returns>Returns the hierarchy of asset folders beginning with the root level</returns>
        public async Task<AssetFoldersModel> GetAssetFoldersAsync()
        {
            var endpointUrl = _urlBuilder.BuildAssetFoldersUrl();
            var response = await _actionInvoker.InvokeReadOnlyMethodAsync<AssetFoldersModel>(endpointUrl, HttpMethod.Get);

            return response;
        }

        /// <summary>
        /// Creates the asset folder.
        /// </summary>
        /// <param name="folder">The asset folder to be created.</param>
        /// <returns>The <see cref="LanguageModel"/> instance that represents created asset folder.</returns>
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

        /// <summary>
        /// Modifies the asset folder.
        /// </summary>
        /// /// <param name="changes">Represents changes that will be apply to the asset folder.</param>
        /// <returns>The <see cref="LanguageModel"/> instance that represents modified asset folder.</returns>
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

        /// <summary>
        /// Updates given asset.
        /// </summary>
        /// <param name="identifier">The identifier of the asset.</param>
        /// <param name="asset">Represents updated asset.</param>
        /// <returns>The <see cref="AssetModel"/> instance that represents updated asset.</returns>
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

        /// <summary>
        /// Deletes given asset.
        /// </summary>
        /// <param name="identifier">The identifier of the asset.</param>
        public async Task DeleteAssetAsync(Reference identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var endpointUrl = _urlBuilder.BuildAssetsUrl(identifier);
            await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
        }

        /// <summary>
        /// Creates asset.
        /// </summary>
        /// <param name="asset">Represents asset which will be created.</param>
        /// <returns>The <see cref="AssetModel"/> instance that represents created asset.</returns>
        public async Task<AssetModel> CreateAssetAsync(AssetUpsertModel asset)
        {
            if (asset == null)
            {
                throw new ArgumentNullException(nameof(asset));
            }

            var endpointUrl = _urlBuilder.BuildAssetsUrl();
            var response = await _actionInvoker.InvokeMethodAsync<AssetUpsertModel, AssetModel>(endpointUrl, HttpMethod.Post, asset);

            return response;
        }

        /// <summary>
        /// Inserts or updates asset according to external identifier.
        /// </summary>
        /// <param name="externalId">The external identifier of the content item.</param>
        /// <param name="asset">Represents asset which will be created.</param>
        /// <returns>The <see cref="AssetModel"/> instance that represents inserted or updated asset.</returns>
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

        #endregion

        #region Binary files

        /// <summary>
        /// Uploads the given file.
        /// </summary>
        /// <param name="fileContent">Represents the content of the file</param>
        /// <returns>The <see cref="FileReference"/> instance that represents the reference to created file.</returns>
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

        #endregion

        #region Validation

        /// <summary>
        /// Validates the project.
        /// </summary>
        /// <returns><see cref="ProjectReportModel"/></returns>
        public async Task<ProjectReportModel> ValidateProjectAsync()
        {
            var endpointUrl = _urlBuilder.BuildValidationUrl();
            return await _actionInvoker.InvokeReadOnlyMethodAsync<ProjectReportModel>(endpointUrl, HttpMethod.Post);
        }

        #endregion

        #region Project

        /// <summary>
        /// Returns project information
        /// </summary>
        /// <returns>The <see cref="Project"/> instance that represents the project infornation.</returns>
        public async Task<Project> GetProjectInformation()
        {
            var endpointUrl = _urlBuilder.BuildProjectUrl();
            return await _actionInvoker.InvokeReadOnlyMethodAsync<Project>(endpointUrl, HttpMethod.Get);
        }
        #endregion

        #region Collections

        /// <summary>
        /// Returns listing of collection.
        /// </summary>
        /// <returns>The <see cref="CollectionsModel"/> instance that represents the listing of collection.</returns>
        public async Task<CollectionsModel> ListCollectionsAsync()
        {
            var endpointUrl = _urlBuilder.BuildCollectionsUrl();
            return await _actionInvoker.InvokeReadOnlyMethodAsync<CollectionsModel>(endpointUrl, HttpMethod.Get);
        }

        /// <summary>
        /// Modifies collection.
        /// </summary>
        /// <param name="changes">Represents changes that will be apply to the collection.</param>
        public async Task<CollectionsModel> ModifyCollectionAsync(IEnumerable<CollectionOperationBaseModel> changes)
        {
            if (changes == null)
            {
                throw new ArgumentNullException(nameof(changes));
            }

            var endpointUrl = _urlBuilder.BuildCollectionsUrl();
            return await _actionInvoker.InvokeMethodAsync<IEnumerable<CollectionOperationBaseModel>, CollectionsModel>(endpointUrl, new HttpMethod("PATCH"), changes);
        }
        #endregion

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
