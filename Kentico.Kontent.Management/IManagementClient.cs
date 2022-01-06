using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.Assets.Patch;
using Kentico.Kontent.Management.Models.Collections;
using Kentico.Kontent.Management.Models.Collections.Patch;
using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.Languages;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.ProjectReport;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.StronglyTyped;
using Kentico.Kontent.Management.Models.TaxonomyGroups;
using Kentico.Kontent.Management.Models.TaxonomyGroups.Patch;
using Kentico.Kontent.Management.Models.Types;
using Kentico.Kontent.Management.Models.Types.Patch;
using Kentico.Kontent.Management.Models.TypeSnippets;
using Kentico.Kontent.Management.Models.TypeSnippets.Patch;
using Kentico.Kontent.Management.Models.Webhooks;
using Kentico.Kontent.Management.Models.Workflow;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kentico.Kontent.Management
{
    /// <summary>
    /// Represents set of Content Management API requests.
    /// </summary>
    public interface IManagementClient
    {
        /// <summary>
        /// Cancels publishing of the language variant.
        /// </summary>
        /// <param name="identifier">The identifier of the language variant identifier of which publishing should be canceled.</param>
        Task CancelPublishingOfLanguageVariant(LanguageVariantIdentifier identifier);

        /// <summary>
        /// Cancels unpublishing of the language variant.
        /// </summary>
        /// <param name="identifier">The identifier of the language variant of which unpublishing should be canceled.</param>
        Task CancelUnpublishingOfLanguageVariant(LanguageVariantIdentifier identifier);

        /// <summary>
        /// Changes workflow step.
        /// </summary>
        /// <param name="identifier">Identifier of the workflow step to be changed.</param>
        Task ChangeLanguageVariantWorkflowStep(WorkflowIdentifier identifier);

        /// <summary>
        /// Creates asset.
        /// </summary>
        /// <param name="asset">Represents the asset that will be created.</param>
        /// <returns>The <see cref="AssetModel"/> instance that represents created asset.</returns>
        Task<AssetModel> CreateAssetAsync(AssetCreateModel asset);

        /// <summary>
        /// Creates the asset folder.
        /// </summary>
        /// <param name="folder">The asset folder that will be created.</param>
        /// <returns>The <see cref="LanguageModel"/> instance that represents created asset folder.</returns>
        Task<AssetFoldersModel> CreateAssetFoldersAsync(AssetFolderCreateModel folder);

        /// <summary>
        /// Creates content item.
        /// </summary>
        /// <param name="contentItem">Represents content item that will be created.</param>
        /// <returns>The <see cref="ContentItemModel"/> instance that represents the created content item.</returns>
        Task<ContentItemModel> CreateContentItemAsync(ContentItemCreateModel contentItem);

        /// <summary>
        /// Creates content type.
        /// </summary>
        /// <param name="contentType">Represents content type that will be created.</param>
        /// <returns>The <see cref="ContentTypeModel"/> instance that represents created content type.</returns>
        Task<ContentTypeModel> CreateContentTypeAsync(ContentTypeCreateModel contentType);

        /// <summary>
        /// Creates content type snippet.
        /// </summary>
        /// <param name="contentTypeSnippet">Represents content type snippet which will be created.</param>
        /// <returns>The <see cref="ContentTypeSnippetModel"/> instance that represents created content type snippet.</returns>
        Task<ContentTypeSnippetModel> CreateContentTypeSnippetAsync(CreateContentSnippetCreateModel contentTypeSnippet);

        /// <summary>
        /// Creates the language.
        /// </summary>
        /// <param name="language">The language to be created.</param>
        /// <returns>The <see cref="LanguageModel"/> instance that represents created language.</returns>
        Task<LanguageModel> CreateLanguageAsync(LanguageCreateModel language);

        /// <summary>
        /// Creates the new version of the language variant.
        /// </summary>
        /// <param name="identifier">The identifier of the language variant for which the new version should be created.</param>
        Task CreateNewVersionOfLanguageVariant(LanguageVariantIdentifier identifier);

        /// <summary>
        /// Creates taxonomy group.
        /// </summary>
        /// <param name="taxonomyGroup">Represents the taxonomy group which will be created.</param>
        /// <returns>The <see cref="TaxonomyGroupModel"/> instance that represents created taxonomy group.</returns>
        Task<TaxonomyGroupModel> CreateTaxonomyGroupAsync(TaxonomyGroupCreateModel taxonomyGroup);

        /// <summary>
        /// Creates the webhook.
        /// </summary>
        /// <param name="webhook">The webhook to be created.</param>
        /// <returns>The <see cref="WebhookModel"/> instance that represents created webhook.</returns>
        Task<WebhookModel> CreateWebhookAsync(WebhookCreateModel webhook);

        /// <summary>
        /// Deletes the given asset.
        /// </summary>
        /// <param name="identifier">The identifier of the asset.</param>
        Task DeleteAssetAsync(Reference identifier);

        /// <summary>
        /// Deletes the given content item.
        /// </summary>
        /// <param name="identifier">The identifier of the content item.</param>
        Task DeleteContentItemAsync(Reference identifier);

        /// <summary>
        /// Deletes the given content type.
        /// </summary>
        /// <param name="identifier">The identifier of the content type.</param>
        Task DeleteContentTypeAsync(Reference identifier);

        /// <summary>
        /// Deletes the given content type snippet.
        /// </summary>
        /// <param name="identifier">The identifier of the content type snippet.</param>
        Task DeleteContentTypeSnippetAsync(Reference identifier);

        /// <summary>
        /// Deletes the given language variant.
        /// </summary>
        /// <param name="identifier">The identifier of the language variant.</param>
        Task DeleteLanguageVariantAsync(LanguageVariantIdentifier identifier);

        /// <summary>
        /// Deletes the given taxonomy group.
        /// </summary>
        /// <param name="identifier">The identifier of the taxonomy group.</param>
        Task DeleteTaxonomyGroupAsync(Reference identifier);

        /// <summary>
        /// Deletes the webhook.
        /// </summary>
        /// <param name="identifier">The identifier of the webhook.</param>
        Task DeleteWebhookAsync(Reference identifier);

        /// <summary>
        /// Disables the webhook.
        /// </summary>
        /// <param name="identifier">The identifier of the webhook.</param>
        Task DisableWebhookAsync(Reference identifier);

        /// <summary>
        /// Enables the webhook.
        /// </summary>
        /// <param name="identifier">The identifier of the webhook.</param>
        Task EnableWebhookAsync(Reference identifier);

        /// <summary>
        /// Returns strongly typed asset.
        /// </summary>
        /// <param name="identifier">The identifier of the asset.</param>
        /// <returns>The <see cref="AssetModel"/> instance that represents requested asset.</returns>
        Task<AssetModel> GetAssetAsync(Reference identifier);

        /// <summary>
        /// Get the Asset Folders
        /// </summary>
        /// <returns>Returns the hierarchy of asset folders beginning with the root level</returns>
        Task<AssetFoldersModel> GetAssetFoldersAsync();

        /// <summary>
        /// Returns strongly typed content item.
        /// </summary>
        /// <param name="identifier">The identifier of the content item.</param>
        /// <returns>The <see cref="ContentItemModel"/> instance that represents requested content item.</returns>
        Task<ContentItemModel> GetContentItemAsync(Reference identifier);

        /// <summary>
        /// Returns strongly typed content type.
        /// </summary>
        /// <param name="identifier">The identifier of the content type.</param>
        /// <returns>The <see cref="ContentTypeModel"/> instance that represents requested content type.</returns>
        Task<ContentTypeModel> GetContentTypeAsync(Reference identifier);

        /// <summary>
        /// Returns content type snippet.
        /// </summary>
        /// <param name="identifier">The identifier of the content type snippet.</param>
        /// <returns>The <see cref="ContentTypeSnippetModel"/> instance that represents requested content type snippet.</returns>
        Task<ContentTypeSnippetModel> GetContentTypeSnippetAsync(Reference identifier);

        /// <summary>
        /// Returns the language.
        /// </summary>
        /// <param name="identifier">The identifier of the language.</param>
        /// <returns>The <see cref="LanguageModel"/> instance that represents requested language.</returns>
        Task<LanguageModel> GetLanguageAsync(Reference identifier);

        /// <summary>
        /// Returns strongly typed language variant.
        /// </summary>
        /// <param name="identifier">The identifier of the language variant.</param>
        /// <returns>The <see cref="LanguageVariantModel"/> instance that represents language variant.</returns>
        Task<LanguageVariantModel> GetLanguageVariantAsync(LanguageVariantIdentifier identifier);

        /// <summary>
        /// Returns strongly typed language variant with strongly typed elements.
        /// </summary>
        /// <typeparam name="T">Type of the content item elements</typeparam>
        /// <param name="identifier">The identifier of the language variant.</param>
        /// <returns>The <see cref="LanguageVariantModel{T}"/> instance that represents language variant.</returns>
        Task<LanguageVariantModel<T>> GetLanguageVariantAsync<T>(LanguageVariantIdentifier identifier) where T : new();

        /// <summary>
        /// Returns project information
        /// </summary>
        /// <returns>The <see cref="Project"/> instance that represents the project infornation.</returns>
        Task<Project> GetProjectInformation();

        /// <summary>
        /// Returns taxonomy group.
        /// </summary>
        /// <param name="identifier">The identifier of the taxonomy group.</param>
        /// <returns>The <see cref="TaxonomyGroupModel"/> instance that represents requested taxonomy group.</returns>
        Task<TaxonomyGroupModel> GetTaxonomyGroupAsync(Reference identifier);

        /// <summary>
        /// Returns the webhook.
        /// </summary>
        /// <param name="identifier">The identifier of the webhook.</param>
        /// <returns>The <see cref="WebhookModel"/> instance that represents requested webhook.</returns>
        Task<WebhookModel> GetWebhookAsync(Reference identifier);

        /// <summary>
        /// Returns strongly typed listing of assets.
        /// The Content management API returns a dynamically paginated listing response limited to up to 100 objects. 
        /// To check if the next page is available use <see cref="IListingResponseModel{T}.HasNextPage"/>,
        /// For getting next page use <see cref="IListingResponseModel{T}.GetNextPage"/>,
        /// </summary>
        /// <returns>The <see cref="IListingResponseModel{AssetModel}"/> instance that represents the listing of assets.</returns>
        Task<IListingResponseModel<AssetModel>> ListAssetsAsync();

        /// <summary>
        /// Returns listing of collection.
        /// </summary>
        /// <returns>The <see cref="CollectionsModel"/> instance that represents the listing of collection.</returns>
        Task<CollectionsModel> ListCollectionsAsync();

        /// <summary>
        /// Returns strongly typed listing of content items. 
        /// The Content management API returns a dynamically paginated listing response limited to up to 100 objects. 
        /// To check if the next page is available use <see cref="IListingResponseModel{T}.HasNextPage"/>,
        /// For getting next page use <see cref="IListingResponseModel{T}.GetNextPage"/>,
        /// </summary>
        /// <returns>The <see cref="IListingResponseModel{ContentItemModel}"/> instance that represents the listing of content items.</returns>
        Task<IListingResponseModel<ContentItemModel>> ListContentItemsAsync();

        /// <summary>
        /// Returns listing of content types.
        /// </summary>
        /// <returns>The <see cref="IListingResponseModel{ContentTypeModel}"/> instance that represents the listing of content types.</returns>
        Task<IListingResponseModel<ContentTypeModel>> ListContentTypesAsync();

        /// <summary>
        /// Returns listing of content type snippets.
        /// </summary>
        /// <returns>The <see cref="IListingResponseModel{ContentTypeSnippetModel}"/> instance that represents the listing of content type snippets.</returns>
        Task<IListingResponseModel<ContentTypeSnippetModel>> ListContentTypeSnippetsAsync();

        /// <summary>
        /// Returns listing of languages.
        /// </summary>
        /// <returns>The <see cref="IListingResponseModel{LanguageModel}"/> instance that represents the listing of languages.</returns>
        Task<IListingResponseModel<LanguageModel>> ListLanguagesAsync();

        /// <summary>
        /// Returns strongly typed listing of language variants for specified collection.
        /// </summary>
        /// <param name="identifier">The identifier of the collection.</param>
        /// <returns>The <see cref="IEnumerable{LanguageVariantModel}"/> instance that represents the listing of language variants.</returns>
        Task<IListingResponseModel<LanguageVariantModel>> ListLanguageVariantsByCollectionAsync(Reference identifier);

        /// <summary>
        /// Returns strongly typed listing of language variants for the specified content item.
        /// </summary>
        /// <param name="identifier">The identifier of the content item.</param>
        /// <returns>The <see cref="IEnumerable{LanguageVariantModel}"/> instance that represents the listing of language variants.</returns>
        Task<IEnumerable<LanguageVariantModel>> ListLanguageVariantsByItemAsync(Reference identifier);

        /// <summary>
        /// Returns strongly typed listing of language variants with strongly typed elements for the specified content item.
        /// </summary>
        /// <typeparam name="T">Type of the content item elements</typeparam>
        /// <param name="identifier">The identifier of the content item.</param>
        /// <returns>A strongly-typed collection with language variants.</returns>
        Task<List<LanguageVariantModel<T>>> ListLanguageVariantsByItemAsync<T>(Reference identifier) where T : new();

        /// <summary>
        /// Returns strongly typed listing of language variants for the specified content type.
        /// </summary>
        /// <param name="identifier">The identifier of the content type.</param>
        /// <returns>The <see cref="IEnumerable{LanguageVariantModel}"/> instance that represents the listing of language variants.</returns>
        Task<IListingResponseModel<LanguageVariantModel>> ListLanguageVariantsByTypeAsync(Reference identifier);

        /// <summary>
        /// Returns strongly typed listing of language variants containing components by type.
        /// </summary>
        /// <param name="identifier">The identifier of the content type.</param>
        /// <returns>The <see cref="IEnumerable{LanguageVariantModel}"/> instance that represents the listing of language variants.</returns>
        Task<IListingResponseModel<LanguageVariantModel>> ListLanguageVariantsOfContentTypeWithComponentsAsync(Reference identifier);

        /// <summary>
        /// Returns listing of taxonomy groups.
        /// </summary>
        /// <returns>The <see cref="IListingResponseModel{TaxonomyGroupModel}"/> instance that represents the listing of taxonomy groups.</returns>
        Task<IListingResponseModel<TaxonomyGroupModel>> ListTaxonomyGroupsAsync();

        /// <summary>
        /// Returns listing of webhooks.
        /// </summary>
        /// <returns>The <see cref="IEnumerable{WebhookModel}"/> instance that represents the listing of webhooks.</returns>
        Task<IEnumerable<WebhookModel>> ListWebhooksAsync();

        /// <summary>
        /// Returns listing of workflow steps.
        /// </summary>
        /// <returns>The <see cref="IEnumerable{WorkflowStepModel}"/> instance that represents the listing of workflow steps.</returns>
        Task<IEnumerable<WorkflowStepModel>> ListWorkflowStepsAsync();

        /// <summary>
        /// Modifies the asset folder.
        /// </summary>
        /// /// <param name="changes">Represents changes that will be applied to the asset folder.</param>
        /// <returns>The <see cref="LanguageModel"/> instance that represents modified asset folder.</returns>
        Task<AssetFoldersModel> ModifyAssetFoldersAsync(IEnumerable<AssetFolderOperationBaseModel> changes);

        /// <summary>
        /// Modifies collection.
        /// </summary>
        /// <param name="changes">Represents changes that will be applied to the collection.</param>
        Task<CollectionsModel> ModifyCollectionAsync(IEnumerable<CollectionOperationBaseModel> changes);

        /// <summary>
        /// Modifies content type.
        /// </summary>
        /// <param name="identifier">The identifier of the content type.</param>
        /// /// <param name="changes">Represents changes that will be applied to the content type.</param>
        Task<ContentTypeModel> ModifyContentTypeAsync(Reference identifier, IEnumerable<ContentTypeOperationBaseModel> changes);

        /// <summary>
        /// Modifies content type snippet.
        /// </summary>
        /// <param name="identifier">The identifier of the content type snippet.</param>
        /// <param name="changes">Represents changes that will be applied to the content type snippet.</param>
        Task<ContentTypeSnippetModel> ModifyContentTypeSnippetAsync(Reference identifier, IEnumerable<ContentTypeSnippetOperationBaseModel> changes);

        /// <summary>
        /// Modifies the language.
        /// </summary>
        /// <param name="identifier">The language to be modified.</param>
        /// <param name="changes">Represents changes that will be applied to the language.</param>
        /// <returns>The <see cref="LanguageModel"/> instance that represents modified language.</returns>
        Task<LanguageModel> ModifyLanguageAsync(Reference identifier, IEnumerable<LanguagePatchModel> changes);

        /// <summary>
        /// Modifies given taxonomy group.
        /// </summary>
        /// <param name="identifier">The identifier of the taxonomy group.</param>
        /// <param name="changes">Represents changes that will be applied to the taxonomy group.</param>
        /// <returns>The <see cref="TaxonomyGroupModel"/> instance that represents the created taxonomy group.</returns>
        Task<TaxonomyGroupModel> ModifyTaxonomyGroupAsync(Reference identifier, IEnumerable<TaxonomyGroupOperationBaseModel> changes);

        /// <summary>
        /// Publishes the language variant.
        /// </summary>
        /// <param name="identifier">Identifier of the language variant to be published.</param>
        Task PublishLanguageVariant(LanguageVariantIdentifier identifier);

        /// <summary>
        /// Schedules publishing of the language variant.
        /// </summary>
        /// <param name="identifier">The identifier of the language variant to be published.</param>
        /// <param name="scheduleModel">The time when the language variant will be published</param>
        Task SchedulePublishingOfLanguageVariant(LanguageVariantIdentifier identifier, ScheduleModel scheduleModel);

        /// <summary>
        /// Schedules unpublishing of the language variant.
        /// </summary>
        /// <param name="identifier">The identifier of the language variant that should be unpublished.</param>
        /// <param name="scheduleModel">The time when the language variant will be unpublished</param>
        Task ScheduleUnpublishingOfLanguageVariant(LanguageVariantIdentifier identifier, ScheduleModel scheduleModel);

        /// <summary>
        /// Unpublishes the language variant.
        /// </summary>
        /// <param name="identifier">Identifier of the language variant to be unpublished.</param>
        Task UnpublishLanguageVariant(LanguageVariantIdentifier identifier);

        /// <summary>
        /// Updates the given asset.
        /// </summary>
        /// <param name="identifier">The identifier of the asset.</param>
        /// <param name="asset">Represents the updated asset.</param>
        /// <returns>The <see cref="AssetModel"/> instance that represents updated asset.</returns>
        Task<AssetModel> UpdateAssetAsync(Reference identifier, AssetUpdateModel asset);

        /// <summary>
        /// Updates the given content item.
        /// </summary>
        /// <param name="identifier">The identifier of the content item.</param>
        /// <param name="contentItem">Represents the updated content item.</param>
        /// <returns>The <see cref="ContentItemModel"/> instance that represents updated content item.</returns>
        Task<ContentItemModel> UpdateContentItemAsync(Reference identifier, ContentItemUpdateModel contentItem);

        /// <summary>
        /// Uploads the given file.
        /// </summary>
        /// <param name="fileContent">Represents the content of the file</param>
        /// <returns>The <see cref="FileReference"/> instance that represents reference to the created file.</returns>
        Task<FileReference> UploadFileAsync(FileContentSource fileContent);

        /// <summary>
        /// Inserts or updates the asset.
        /// </summary>
        /// <param name="externalId">The external identifier of the content item.</param>
        /// <param name="asset">Represents the asset that will be created.</param>
        /// <returns>The <see cref="AssetModel"/> instance that represents inserted or updated asset.</returns>
        Task<AssetModel> UpsertAssetByExternalIdAsync(string externalId, AssetUpsertModel asset);

        /// <summary>
        /// Inserts or updates the given content item.
        /// </summary>
        /// <param name="externalId">The external identifier of the content item.</param>
        /// <param name="contentItem">Represents inserted or updated content item.</param>
        /// <returns>The <see cref="ContentItemModel"/> instance that represents inserted or updated content item.</returns>
        Task<ContentItemModel> UpsertContentItemByExternalIdAsync(string externalId, ContentItemUpsertModel contentItem);

        /// <summary>
        /// Inserts or updates given language variant.
        /// </summary>
        /// <param name="identifier">The identifier of the language variant.</param>
        /// <param name="languageVariantUpsertModel">Represents the inserted or updated language variant.</param>
        /// <returns>The <see cref="LanguageVariantModel"/> instance that represents the inserted or updated language variant.</returns>
        Task<LanguageVariantModel> UpsertLanguageVariantAsync(LanguageVariantIdentifier identifier, LanguageVariantUpsertModel languageVariantUpsertModel);

        /// <summary>
        /// Inserts or updates the given language variant.
        /// </summary>
        /// <typeparam name="T">Type of the content item elements</typeparam>
        /// <param name="identifier">The identifier of the language variant.</param>
        /// <param name="variantElements">Represents inserted or updated strongly typed language variant elements.</param>
        /// <returns>The <see cref="LanguageVariantModel{T}"/> instance that represents inserted or updated language variant.</returns>
        Task<LanguageVariantModel<T>> UpsertLanguageVariantAsync<T>(LanguageVariantIdentifier identifier, T variantElements) where T : new();

        /// <summary>
        /// Validates the project.
        /// </summary>
        /// <returns><see cref="ProjectReportModel"/></returns>
        Task<ProjectReportModel> ValidateProjectAsync();
    }
}