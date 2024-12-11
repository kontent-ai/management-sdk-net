using Kontent.Ai.Management.Models.AssetFolders;
using Kontent.Ai.Management.Models.AssetFolders.Patch;
using Kontent.Ai.Management.Models.AssetRenditions;
using Kontent.Ai.Management.Models.Assets;
using Kontent.Ai.Management.Models.Collections;
using Kontent.Ai.Management.Models.Collections.Patch;
using Kontent.Ai.Management.Models.CustomApps;
using Kontent.Ai.Management.Models.CustomApps.Patch;
using Kontent.Ai.Management.Models.Environments;
using Kontent.Ai.Management.Models.Environments.Patch;
using Kontent.Ai.Management.Models.Items;
using Kontent.Ai.Management.Models.Languages;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.LegacyWebhooks;
using Kontent.Ai.Management.Models.PreviewConfiguration;
using Kontent.Ai.Management.Models.EnvironmentReport;
using Kontent.Ai.Management.Models.EnvironmentValidation;
using Kontent.Ai.Management.Models.Publishing;
using Kontent.Ai.Management.Models.Roles;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Spaces;
using Kontent.Ai.Management.Models.Spaces.Patch;
using Kontent.Ai.Management.Models.StronglyTyped;
using Kontent.Ai.Management.Models.Subscription;
using Kontent.Ai.Management.Models.TaxonomyGroups;
using Kontent.Ai.Management.Models.TaxonomyGroups.Patch;
using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management.Models.Types.Patch;
using Kontent.Ai.Management.Models.TypeSnippets;
using Kontent.Ai.Management.Models.TypeSnippets.Patch;
using Kontent.Ai.Management.Models.Users;
using Kontent.Ai.Management.Models.Webhooks;
using Kontent.Ai.Management.Models.WebSpotlight;
using Kontent.Ai.Management.Models.Workflow;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

/// <summary>
/// Represents set of Content Management API requests.
/// </summary>
public interface IManagementClient
{
    /// <summary>
    /// Returns asset.
    /// </summary>
    /// <param name="identifier">The identifier of the asset.</param>
    /// <returns>The <see cref="AssetModel"/> instance that represents requested asset.</returns>
    Task<AssetModel> GetAssetAsync(Reference identifier);

    /// <summary>
    /// Returns asset with strongly typed elements.
    /// </summary>
    /// <param name="identifier">The identifier of the asset.</param>
    /// <returns>The <see cref="AssetModel"/> instance that represents requested asset.</returns>
    Task<AssetModel<T>> GetAssetAsync<T>(Reference identifier) where T : new();

    /// <summary>
    /// Returns listing of assets.
    /// The Content management API returns a dynamically paginated listing response limited to up to 100 objects.
    /// To check if the next page is available use <see cref="IListingResponseModel{T}.HasNextPage"/>.
    /// For getting next page use <see cref="IListingResponseModel{T}.GetNextPage"/>.
    /// </summary>
    /// <returns>The <see cref="IListingResponseModel{AssetModel}"/> instance that represents the listing of assets.</returns>
    Task<IListingResponseModel<AssetModel>> ListAssetsAsync();

    /// <summary>
    /// Returns listing of assets with strongly typed elements.
    /// The Content management API returns a dynamically paginated listing response limited to up to 100 objects.
    /// To check if the next page is available use <see cref="IListingResponseModel{T}.HasNextPage"/>.
    /// For getting next page use <see cref="IListingResponseModel{T}.GetNextPage"/>.
    /// </summary>
    /// <returns>The <see cref="IListingResponseModel{AssetModel}"/> instance that represents the listing of assets.</returns>
    Task<IListingResponseModel<AssetModel<T>>> ListAssetsAsync<T>() where T : new();

    /// <summary>
    /// Creates asset.
    /// </summary>
    /// <param name="asset">Represents the asset that will be created.</param>
    /// <returns>The <see cref="AssetModel"/> instance that represents created asset.</returns>
    Task<AssetModel> CreateAssetAsync(AssetCreateModel asset);

    /// <summary>
    /// Creates asset with strongly typed elements.
    /// </summary>
    /// <param name="asset">Represents the asset that will be created.</param>
    /// <returns>The <see cref="AssetModel"/> instance that represents created asset with strongly typed elements.</returns>
    Task<AssetModel<T>> CreateAssetAsync<T>(AssetCreateModel<T> asset) where T : new();

    /// <summary>
    /// Updates the given asset.
    /// </summary>
    /// <param name="identifier">The identifier of the asset.</param>
    /// <param name="asset">Represents the updated asset.</param>
    /// <returns>The <see cref="AssetModel"/> instance that represents updated asset.</returns>
    Task<AssetModel> UpsertAssetAsync(Reference identifier, AssetUpsertModel asset);

    /// <summary>
    /// Updates the given asset with strongly typed elements.
    /// </summary>
    /// <param name="identifier">The identifier of the asset.</param>
    /// <param name="asset">Represents the updated asset with strongly typed elements.</param>
    /// <returns>The <see cref="AssetModel"/> instance that represents updated asset with strongly typed elements.</returns>
    Task<AssetModel<T>> UpsertAssetAsync<T>(Reference identifier, AssetUpsertModel<T> asset) where T : new();

    /// <summary>
    /// Deletes the given asset.
    /// </summary>
    /// <param name="identifier">The identifier of the asset.</param>
    Task DeleteAssetAsync(Reference identifier);

    /// <summary>
    /// Uploads the given file.
    /// </summary>
    /// <param name="fileContent">Represents the content of the file</param>
    /// <returns>The <see cref="FileReference"/> instance that represents reference to the created file.</returns>
    Task<FileReference> UploadFileAsync(FileContentSource fileContent);

    /// <summary>
    /// Retrieve a rendition of the specified asset.
    /// </summary>
    /// <param name="identifier">The identifier of the asset rendition.</param>
    /// <returns>The <see cref="AssetRenditionModel"/> instance that represents the asset rendition.</returns>
    Task<AssetRenditionModel> GetAssetRenditionAsync(AssetRenditionIdentifier identifier);

    /// <summary>
    /// Returns a paginated list of all renditions of the specified asset.
    /// The Content management API returns a dynamically paginated listing response limited to up to 100 objects.
    /// To check if the next page is available use <see cref="IListingResponseModel{T}.HasNextPage"/>.
    /// For getting next page use <see cref="IListingResponseModel{T}.GetNextPage"/>.
    /// </summary>
    /// <param name="assetIdentifier">The identifier of the asset.</param>
    /// <returns>The <see cref="IListingResponseModel{AssetRenditionModel}"/> instance that represents the listing of asset renditions.</returns>
    Task<IListingResponseModel<AssetRenditionModel>> ListAssetRenditionsAsync(Reference assetIdentifier);

    /// <summary>
    /// Creates a new rendition of the specified asset.
    /// </summary>
    /// <param name="assetIdentifier">The identifier of the asset.</param>
    /// <param name="createModel">Represents the asset rendition that will be created.</param>
    /// <returns>The <see cref="AssetRenditionModel"/> instance that represents the created asset rendition.</returns>
    Task<AssetRenditionModel> CreateAssetRenditionAsync(Reference assetIdentifier, AssetRenditionCreateModel createModel);

    /// <summary>
    /// Modify a rendition of the asset.
    /// </summary>
    /// <param name="identifier">The identifier of the asset rendition.</param>
    /// <param name="updateModel">Represents the updated asset rendition.</param>
    /// <returns>The <see cref="AssetRenditionModel"/> instance that represents the updated asset rendition.</returns>
    Task<AssetRenditionModel> UpdateAssetRenditionAsync(AssetRenditionIdentifier identifier, AssetRenditionUpdateModel updateModel);
    /// <summary>
    /// Cancels publishing of the language variant.
    /// </summary>
    /// <param name="identifier">The identifier of the language variant identifier of which publishing should be canceled.</param>
    Task CancelPublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier);

    /// <summary>
    /// Cancels unpublishing of the language variant.
    /// </summary>
    /// <param name="identifier">The identifier of the language variant of which unpublishing should be canceled.</param>
    Task CancelUnpublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier);

    /// <summary>
    /// Changes workflow.
    /// </summary>
    /// <param name="identifier">Identifier of the language variant to be changed.</param>
    /// <param name="changeModel">Change language variant workflow model.</param>
    Task ChangeLanguageVariantWorkflowAsync(LanguageVariantIdentifier identifier, ChangeLanguageVariantWorkflowModel changeModel);

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
    Task<ContentTypeSnippetModel> CreateContentTypeSnippetAsync(ContentTypeSnippetCreateModel contentTypeSnippet);

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
    Task CreateNewVersionOfLanguageVariantAsync(LanguageVariantIdentifier identifier);

    /// <summary>
    /// Creates taxonomy group.
    /// </summary>
    /// <param name="taxonomyGroup">Represents the taxonomy group which will be created.</param>
    /// <returns>The <see cref="TaxonomyGroupModel"/> instance that represents created taxonomy group.</returns>
    Task<TaxonomyGroupModel> CreateTaxonomyGroupAsync(TaxonomyGroupCreateModel taxonomyGroup);

    /// <summary>
    /// Creates the legacy webhook.
    /// </summary>
    /// <param name="webhook">The legacy webhook to be created.</param>
    /// <returns>The <see cref="LegacyWebhookModel"/> instance that represents created legacy webhook.</returns>
    Task<LegacyWebhookModel> CreateLegacyWebhookAsync(LegacyWebhookCreateModel webhook);

    /// <summary>
    /// Creates the webhook.
    /// </summary>
    /// <param name="webhook">The webhook to be created.</param>
    /// <returns>The <see cref="WebhookModel"/> instance that represents created webhook.</returns>
    Task<WebhookModel> CreateWebhookAsync(WebhookCreateModel webhook);
    
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
    /// Deletes the legacy webhook.
    /// </summary>
    /// <param name="identifier">The identifier of the legacy webhook.</param>
    Task DeleteLegacyWebhookAsync(Reference identifier);

    /// <summary>
    /// Deletes the webhook.
    /// </summary>
    /// <param name="identifier">The identifier of the webhook.</param>
    Task DeleteWebhookAsync(Reference identifier);

    /// <summary>
    /// Disables the legacy webhook.
    /// </summary>
    /// <param name="identifier">The identifier of the legacy webhook.</param>
    Task DisableLegacyWebhookAsync(Reference identifier);

    /// <summary>
    /// Disables the webhook.
    /// </summary>
    /// <param name="identifier">The identifier of the webhook.</param>
    Task DisableWebhookAsync(Reference identifier);

    /// <summary>
    /// Enables the legacy webhook.
    /// </summary>
    /// <param name="identifier">The identifier of the legacy webhook.</param>
    Task EnableLegacyWebhookAsync(Reference identifier);

    /// <summary>
    /// Enables the webhook.
    /// </summary>
    /// <param name="identifier">The identifier of the webhook.</param>
    Task EnableWebhookAsync(Reference identifier);
    
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
    /// Returns strongly typed currently published language variant.
    /// </summary>
    /// <param name="identifier">The identifier of the language variant.</param>
    /// <returns>The <see cref="LanguageVariantModel"/> instance that represents language variant.</returns>
    Task<LanguageVariantModel> GetPublishedLanguageVariantAsync(LanguageVariantIdentifier identifier);

    /// <summary>
    /// Returns strongly typed currently published language variant with strongly typed elements.
    /// </summary>
    /// <typeparam name="T">Type of the content item elements</typeparam>
    /// <param name="identifier">The identifier of the language variant.</param>
    /// <returns>The <see cref="LanguageVariantModel{T}"/> instance that represents language variant.</returns>
    Task<LanguageVariantModel<T>> GetPublishedLanguageVariantAsync<T>(LanguageVariantIdentifier identifier) where T : new();

    /// <summary>
    /// Returns environment information
    /// </summary>
    /// <returns>The <see cref="Environment"/> instance that represents the environment information.</returns>
    Task<Models.EnvironmentReport.Environment> GetEnvironmentInformationAsync();

    /// <summary>
    /// Returns taxonomy group.
    /// </summary>
    /// <param name="identifier">The identifier of the taxonomy group.</param>
    /// <returns>The <see cref="TaxonomyGroupModel"/> instance that represents requested taxonomy group.</returns>
    Task<TaxonomyGroupModel> GetTaxonomyGroupAsync(Reference identifier);

    /// <summary>
    /// Returns the legacy webhook.
    /// </summary>
    /// <param name="identifier">The identifier of the legacy webhook.</param>
    /// <returns>The <see cref="LegacyWebhookModel"/> instance that represents requested legacy webhook.</returns>
    Task<LegacyWebhookModel> GetLegacyWebhookAsync(Reference identifier);
    
    /// <summary>
    /// Returns the webhook.
    /// </summary>
    /// <param name="identifier">The identifier of the webhook.</param>
    /// <returns>The <see cref="WebhookModel"/> instance that represents requested webhook.</returns>
    Task<WebhookModel> GetWebhookAsync(Reference identifier);

    /// <summary>
    /// Returns listing of collection.
    /// </summary>
    /// <returns>The <see cref="CollectionsModel"/> instance that represents the listing of collection.</returns>
    Task<CollectionsModel> ListCollectionsAsync();

    /// <summary>
    /// Returns strongly typed listing of content items. 
    /// The Content management API returns a dynamically paginated listing response limited to up to 100 objects.
    /// To check if the next page is available use <see cref="IListingResponseModel{T}.HasNextPage"/>.
    /// For getting next page use <see cref="IListingResponseModel{T}.GetNextPage"/>.
    /// </summary>
    /// <returns>The <see cref="IListingResponseModel{ContentItemModel}"/> instance that represents the listing of content items.</returns>
    Task<IListingResponseModel<ContentItemModel>> ListContentItemsAsync();

    /// <summary>
    /// Returns listing of content types.
    /// The Content management API returns a dynamically paginated listing response limited to up to 100 objects.
    /// To check if the next page is available use <see cref="IListingResponseModel{T}.HasNextPage"/>.
    /// For getting next page use <see cref="IListingResponseModel{T}.GetNextPage"/>.
    /// </summary>
    /// <returns>The <see cref="IListingResponseModel{ContentTypeModel}"/> instance that represents the listing of content types.</returns>
    Task<IListingResponseModel<ContentTypeModel>> ListContentTypesAsync();

    /// <summary>
    /// Returns listing of content type snippets.
    /// The Content management API returns a dynamically paginated listing response limited to up to 100 objects.
    /// To check if the next page is available use <see cref="IListingResponseModel{T}.HasNextPage"/>.
    /// For getting next page use <see cref="IListingResponseModel{T}.GetNextPage"/>.
    /// </summary>
    /// <returns>The <see cref="IListingResponseModel{ContentTypeSnippetModel}"/> instance that represents the listing of content type snippets.</returns>
    Task<IListingResponseModel<ContentTypeSnippetModel>> ListContentTypeSnippetsAsync();

    /// <summary>
    /// Returns listing of languages.
    /// The Content management API returns a dynamically paginated listing response limited to up to 100 objects.
    /// To check if the next page is available use <see cref="IListingResponseModel{T}.HasNextPage"/>.
    /// For getting next page use <see cref="IListingResponseModel{T}.GetNextPage"/>.
    /// </summary>
    /// <returns>The <see cref="IListingResponseModel{LanguageModel}"/> instance that represents the listing of languages.</returns>
    Task<IListingResponseModel<LanguageModel>> ListLanguagesAsync();

    /// <summary>
    /// Returns strongly typed listing of language variants for specified collection.
    /// The Content management API returns a dynamically paginated listing response limited to up to 100 objects.
    /// To check if the next page is available use <see cref="IListingResponseModel{T}.HasNextPage"/>.
    /// For getting next page use <see cref="IListingResponseModel{T}.GetNextPage"/>.
    /// </summary>
    /// <param name="identifier">The identifier of the collection.</param>
    /// <returns>The <see cref="IEnumerable{LanguageVariantModel}"/> instance that represents the listing of language variants.</returns>
    Task<IListingResponseModel<LanguageVariantModel>> ListLanguageVariantsByCollectionAsync(Reference identifier);

    /// <summary>
    /// Returns strongly typed listing of language variants for specified space.
    /// The Content management API returns a dynamically paginated listing response limited to up to 100 objects.
    /// To check if the next page is available use <see cref="IListingResponseModel{T}.HasNextPage"/>.
    /// For getting next page use <see cref="IListingResponseModel{T}.GetNextPage"/>.
    /// </summary>
    /// <param name="identifier">The identifier of the collection.</param>
    /// <returns>The <see cref="IEnumerable{LanguageVariantModel}"/> instance that represents the listing of language variants.</returns>
    Task<IListingResponseModel<LanguageVariantModel>> ListLanguageVariantsBySpaceAsync(Reference identifier);

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
    /// The Content management API returns a dynamically paginated listing response limited to up to 100 objects.
    /// To check if the next page is available use <see cref="IListingResponseModel{T}.HasNextPage"/>.
    /// For getting next page use <see cref="IListingResponseModel{T}.GetNextPage"/>.
    /// </summary>
    /// <param name="identifier">The identifier of the content type.</param>
    /// <returns>The <see cref="IEnumerable{LanguageVariantModel}"/> instance that represents the listing of language variants.</returns>
    Task<IListingResponseModel<LanguageVariantModel>> ListLanguageVariantsByTypeAsync(Reference identifier);

    /// <summary>
    /// Returns strongly typed listing of language variants with strongly typed elements for the specified content type.
    /// The Content management API returns a dynamically paginated listing response limited to up to 100 objects.
    /// To check if the next page is available use <see cref="IListingResponseModel{T}.HasNextPage"/>.
    /// For getting next page use <see cref="IListingResponseModel{T}.GetNextPage"/>.
    /// </summary>
    /// <typeparam name="T">Type of the content item elements</typeparam>
    /// <param name="identifier">The identifier of the content type.</param>
    /// <returns>The <see cref="IEnumerable{LanguageVariantModel}"/> instance that represents the listing of language variants.</returns>
    Task<IListingResponseModel<LanguageVariantModel<T>>> ListLanguageVariantsByTypeAsync<T>(Reference identifier) where T : new();

    /// <summary>
    /// Returns strongly typed listing of language variants containing components by type.
    /// The Content management API returns a dynamically paginated listing response limited to up to 100 objects.
    /// To check if the next page is available use <see cref="IListingResponseModel{T}.HasNextPage"/>.
    /// For getting next page use <see cref="IListingResponseModel{T}.GetNextPage"/>.
    /// </summary>
    /// <param name="identifier">The identifier of the content type.</param>
    /// <returns>The <see cref="IEnumerable{LanguageVariantModel}"/> instance that represents the listing of language variants.</returns>
    Task<IListingResponseModel<LanguageVariantModel>> ListLanguageVariantsOfContentTypeWithComponentsAsync(Reference identifier);

    /// <summary>
    /// Returns listing of taxonomy groups.
    /// The Content management API returns a dynamically paginated listing response limited to up to 100 objects.
    /// To check if the next page is available use <see cref="IListingResponseModel{T}.HasNextPage"/>.
    /// For getting next page use <see cref="IListingResponseModel{T}.GetNextPage"/>.
    /// </summary>
    /// <returns>The <see cref="IListingResponseModel{TaxonomyGroupModel}"/> instance that represents the listing of taxonomy groups.</returns>
    Task<IListingResponseModel<TaxonomyGroupModel>> ListTaxonomyGroupsAsync();

    /// <summary>
    /// Returns listing of legacy webhooks.
    /// </summary>
    /// <returns>The <see cref="IEnumerable{LegacyWebhookModel}"/> instance that represents the listing of legacy webhooks.</returns>
    Task<IEnumerable<LegacyWebhookModel>> ListLegacyWebhooksAsync();
    
    /// <summary>
    /// Returns listing of webhooks.
    /// </summary>
    /// <returns>The <see cref="IEnumerable{WebhookModel}"/> instance that represents the listing of webhooks.</returns>
    Task<IEnumerable<WebhookModel>> ListWebhooksAsync();

    /// <summary>
    /// Returns listing of workflows.
    /// </summary>
    /// <returns>The <see cref="IEnumerable{WorkflowResponseModel}"/> instance that represents the listing of workflow steps.</returns>
    Task<IEnumerable<WorkflowModel>> ListWorkflowsAsync();

    /// <summary>
    /// Deletes the given workflow.
    /// </summary>
    /// <param name="identifier">The identifier of the workflow.</param>
    Task DeleteWorkflowAsync(Reference identifier);

    /// <summary>
    /// Creates a new workflow.
    /// </summary>
    /// <param name="workflow">Represents workflow that will be created.</param>
    /// <returns>The <see cref="WorkflowModel"/> instance that represents the created workflow.</returns>
    Task<WorkflowModel> CreateWorkflowAsync(WorkflowUpsertModel workflow);

    /// <summary>
    /// Update the specified workflow.
    /// </summary>
    /// <param name="identifier">The identifier of the workflow to update.</param>
    /// <param name="workflow">Represents the changed workflow to update.</param>
    /// <returns>The <see cref="WorkflowModel"/> instance that represents the updated workflow.</returns>
    Task<WorkflowModel> UpdateWorkflowAsync(Reference identifier, WorkflowUpsertModel workflow);

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
    Task PublishLanguageVariantAsync(LanguageVariantIdentifier identifier);

    /// <summary>
    /// Schedules publishing of the language variant.
    /// </summary>
    /// <param name="identifier">The identifier of the language variant to be published.</param>
    /// <param name="scheduleModel">The time when the language variant will be published</param>
    Task SchedulePublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier, ScheduleModel scheduleModel);

    /// <summary>
    /// Schedules unpublishing of the language variant.
    /// </summary>
    /// <param name="identifier">The identifier of the language variant that should be unpublished.</param>
    /// <param name="scheduleModel">The time when the language variant will be unpublished</param>
    Task ScheduleUnpublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier, ScheduleModel scheduleModel);

    /// <summary>
    /// Unpublishes the language variant.
    /// </summary>
    /// <param name="identifier">Identifier of the language variant to be unpublished.</param>
    Task UnpublishLanguageVariantAsync(LanguageVariantIdentifier identifier);

    /// <summary>
    /// Updates the given content item.
    /// </summary>
    /// <param name="identifier">The identifier of the content item.</param>
    /// <param name="contentItem">Represents the updated content item.</param>
    /// <returns>The <see cref="ContentItemModel"/> instance that represents updated content item.</returns>
    Task<ContentItemModel> UpsertContentItemAsync(Reference identifier, ContentItemUpsertModel contentItem);

    /// <summary>
    /// Inserts or updates given language variant.
    /// </summary>
    /// <param name="identifier">The identifier of the language variant.</param>
    /// <param name="languageVariantUpsertModel">Represents the inserted or updated language variant.</param>
    /// <returns>The <see cref="LanguageVariantModel"/> instance that represents the inserted or updated language variant.</returns>
    Task<LanguageVariantModel> UpsertLanguageVariantAsync(LanguageVariantIdentifier identifier, LanguageVariantUpsertModel languageVariantUpsertModel);

    /// <summary>
    /// Creates or updates the given content item variant.
    /// </summary>
    /// <param name="identifier">The identifier of the language variant.</param>
    /// <param name="languageVariant">Represents the language variant which data will be used to create <see cref="LanguageVariantUpsertModel"/>.</param>
    /// <returns>The <see cref="LanguageVariantModel"/> instance that represents created or updated content item variant.</returns>
    Task<LanguageVariantModel> UpsertLanguageVariantAsync(LanguageVariantIdentifier identifier, LanguageVariantModel languageVariant);

    /// <summary>
    /// Inserts or updates the given language variant.
    /// </summary>
    /// <typeparam name="T">Type of the content item elements</typeparam>
    /// <param name="identifier">The identifier of the language variant.</param>
    /// <param name="variantElements">Represents inserted or updated strongly typed language variant elements.</param>
    /// <param name="workflow">Workflow step definition to set the inserted or updated language variant.</param>
    /// <returns>The <see cref="LanguageVariantModel{T}"/> instance that represents inserted or updated language variant.</returns>
    Task<LanguageVariantModel<T>> UpsertLanguageVariantAsync<T>(LanguageVariantIdentifier identifier, T variantElements, WorkflowStepIdentifier workflow = null) where T : new();

    /// <summary>
    /// Validates the environment.
    /// </summary>
    /// <returns><see cref="EnvironmentReportModel"/></returns>
    Task<EnvironmentReportModel> ValidateEnvironmentAsync();

    /// <summary>
    /// Initiates asynchronous environment validation.
    /// </summary>
    /// <returns><see cref="AsyncValidationTaskModel"/></returns>
    Task<AsyncValidationTaskModel> InitiateEnvironmentAsyncValidationTaskAsync();

    /// <summary>
    /// Gets async validation task.
    /// </summary>
    /// <returns><see cref="AsyncValidationTaskModel"/></returns>
    Task<AsyncValidationTaskModel> GetAsyncValidationTaskAsync(Guid taskId);

    /// <summary>
    /// Lists async validation task issues.
    /// </summary>
    /// <returns><see cref="IListingResponseModel{AsyncValidationTaskIssue}"/></returns>
    Task<IListingResponseModel<AsyncValidationTaskIssueModel>> ListAsyncValidationTaskIssuesAsync(Guid taskId);

    /// <summary>
    /// Lists all roles in an environment.
    /// </summary>
    /// <returns>The <see cref="EnvironmentRolesModel"/> instance that represents the listing of roles in an environment.</returns>
    Task<EnvironmentRolesModel> ListEnvironmentRolesAsync();

    /// <summary>
    /// Returns environment role.
    /// </summary>
    /// <param name="identifier">The identifier of the environment role.</param>
    /// <returns>The <see cref="EnvironmentRoleModel"/> instance that represents requested environment role.</returns>
    Task<EnvironmentRoleModel> GetEnvironmentRoleAsync(Reference identifier);

    /// <summary>
    /// Invites a new user to an environment.
    /// </summary>
    /// <param name="invitation">Represents an user that is to be invited.</param>
    /// <returns>Returns the newly invited user.</returns>
    Task<UserModel> InviteUserIntoEnvironmentAsync(UserInviteModel invitation);

    /// <summary>
    /// Modifies user's roles.
    /// </summary>
    /// <param name="identifier">The identifier of the environment user.</param>
    /// <param name="user">Represents an user that is to be modified.</param>
    /// <returns>Returns the modified user.</returns>
    Task<UserModel> ModifyUsersRolesAsync(UserIdentifier identifier, UserModel user);

    /// <summary>
    /// Returns strongly typed listing of projects.
    /// The Content management API returns a dynamically paginated listing response limited to up to 100 objects.
    /// To check if the next page is available use <see cref="IListingResponseModel{T}.HasNextPage"/>.
    /// For getting next page use <see cref="IListingResponseModel{T}.GetNextPage"/>.
    /// </summary>
    /// <returns>The <see cref="IListingResponseModel{SubscriptionProjectModel}"/> instance that represents the listing of projects.</returns>
    Task<IListingResponseModel<SubscriptionProjectModel>> ListSubscriptionProjectsAsync();

    /// <summary>
    /// Returns strongly typed listing of users under your subscription including
    /// their assignment to projects, environments, collections, roles, and languages.
    /// The Content management API returns a dynamically paginated listing response limited to up to 100 objects.
    /// To check if the next page is available use <see cref="IListingResponseModel{T}.HasNextPage"/>.
    /// For getting next page use <see cref="IListingResponseModel{T}.GetNextPage"/>.
    /// </summary>
    /// <returns>The <see cref="IListingResponseModel{SubscriptionUserModel}"/> instance that represents the listing of subscription users.</returns>
    Task<IListingResponseModel<SubscriptionUserModel>> ListSubscriptionUsersAsync();

    /// <summary>
    /// Retrieve a user metadata from under the specified subscription.
    /// The metadata include information about the user's access to projects and environments,
    /// and content in specific collections, roles, and languages.
    /// </summary>
    /// <param name="identifier">The identifier of the subscription user.</param>
    /// <returns>The <see cref="SubscriptionProjectModel"/> instance that represents the project.</returns>
    Task<SubscriptionUserModel> GetSubscriptionUserAsync(UserIdentifier identifier);

    /// <summary>
    /// Activates the specified user in all projects under the specified subscription.
    /// </summary>
    /// <param name="identifier">The identifier of the subscription user.</param>
    /// <returns></returns>
    Task ActivateSubscriptionUserAsync(UserIdentifier identifier);

    /// <summary>
    /// Deactivates the specified user in all projects under the specified subscription.
    /// </summary>
    /// <param name="identifier">The identifier of the subscription user.</param>
    /// <returns></returns>
    Task DeactivateSubscriptionUserAsync(UserIdentifier identifier);

    /// <summary>
    /// Clones environment.
    /// </summary>
    /// <param name="cloneEnvironmentModel">Cloning settings.</param>
    /// <returns>The <see cref="EnvironmentClonedModel"/> instance that represents the result of the cloning.</returns>
    Task<EnvironmentClonedModel> CloneEnvironmentAsync(EnvironmentCloneModel cloneEnvironmentModel);

    /// <summary>
    /// Returns the state of the environment cloning.
    /// </summary>
    /// <returns>The <see cref="EnvironmentCloningStateModel"/> instance that represents the current state of the cloning.</returns>
    Task<EnvironmentCloningStateModel> GetEnvironmentCloningStateAsync();

    /// <summary>
    /// Deletes current environment.
    /// </summary>
    Task DeleteEnvironmentAsync();

    /// <summary>
    /// Marks current environment as production.
    /// <param name="markAsProductionModel">Represents settings that will be used to mark the environment as production.</param>
    /// </summary>
    Task MarkEnvironmentAsProductionAsync(MarkAsProductionModel markAsProductionModel);

    /// <summary>
    /// Modifies current environment.
    /// </summary>
    /// <param name="changes">Represents changes that will be applied to the environment.</param>
    /// <returns>The <see cref="EnvironmentModel"/> instance that represents the modified environment.</returns>
    Task<EnvironmentModel> ModifyEnvironmentAsync(IEnumerable<EnvironmentOperationBaseModel> changes);

    /// <summary>
    /// Creates the space.
    /// </summary>
    /// <param name="space">The space to be created.</param>
    /// <returns>The <see cref="SpaceModel"/> instance that represents the created space.</returns>
    Task<SpaceModel> CreateSpaceAsync(SpaceCreateModel space);

    /// <summary>
    /// Returns the space.
    /// </summary>
    /// <returns>The <see cref="SpaceModel"/> instance that represents the space.</returns>
    Task<SpaceModel> GetSpaceAsync(Reference identifier);

    /// <summary>
    /// Returns all spaces.
    /// </summary>
    /// <returns>The <see cref="IEnumerable{SpaceModel}"/> instance that represents the listing of spaces.</returns>
    Task<IEnumerable<SpaceModel>> ListSpacesAsync();

    /// <summary>
    /// Modifies the space.
    /// </summary>
    /// <param name="identifier">The identifier of the space.</param>
    /// <param name="changes">The changes that will be applied to the space.</param>
    /// <returns>The <see cref="SpaceModel"/> instance that represents the modified space.</returns>
    Task<SpaceModel> ModifySpaceAsync(Reference identifier, IEnumerable<SpaceOperationReplaceModel> changes);

    /// <summary>
    /// Deletes the space.
    /// </summary>
    /// <param name="identifier">The identifier of the space.</param>
    Task DeleteSpaceAsync(Reference identifier);

    /// <summary>
    /// Returns the preview configuration.
    /// </summary>
    /// <returns>The <see cref="PreviewConfigurationModel"/> instance that represents the preview configuration.</returns>
    Task<PreviewConfigurationModel> GetPreviewConfigurationAsync();

    /// <summary>
    /// Modify the preview configuration.
    /// </summary>
    /// <param name="previewConfiguration">Represents configuration that will be used for project.</param>
    /// <returns>The <see cref="PreviewConfigurationModel"/> instance that represents the preview configuration.</returns>
    Task<PreviewConfigurationModel> ModifyPreviewConfigurationAsync(PreviewConfigurationModel previewConfiguration);

    /// <summary>
    /// Activates the web spotlight, allowing you to specify an existing Root Type ID.
    /// </summary>
    /// <param name="webSpotlightActivateModel">Represents configuration that will be used for web spotlight activation.</param>
    /// <returns>A <see cref="WebSpotlightModel"/> instance representing the web spotlight status.</returns>
    Task<WebSpotlightModel> ActivateWebSpotlightAsync(WebSpotlightActivateModel webSpotlightActivateModel);

    /// <summary>
    /// Deactivates the web spotlight.
    /// </summary>
    /// <returns>A <see cref="WebSpotlightModel"/> instance representing the web spotlight status.</returns>
    Task<WebSpotlightModel> DeactivateWebSpotlightAsync();

    /// <summary>
    /// Returns the web spotlight status.
    /// </summary>
    /// <returns>A <see cref="WebSpotlightModel"/> instance representing the web spotlight status.</returns>
    Task<WebSpotlightModel> GetWebSpotlightStatusAsync();

    /// <summary>
    /// Returns list of custom apps.
    /// </summary>
    /// <returns>The <see cref="IListingResponseModel{CustomAppModel}"/> instance that represents the listing of custom apps.</returns>
    Task<IListingResponseModel<CustomAppModel>> ListCustomAppsAsync();

    /// <summary>
    /// Returns the custom app.
    /// </summary>
    /// <returns>The <see cref="CustomAppModel"/> instance that represents the custom app.</returns>
    Task<CustomAppModel> GetCustomAppAsync(Reference identifier);

    /// <summary>
    /// Creates the custom apps.
    /// </summary>
    /// <returns>The <see cref="CustomAppModel"/> instance that represents the custom app.</returns>
    Task<CustomAppModel> CreateCustomAppAsync(CustomAppCreateModel customApp);

    /// <summary>
    /// Deletes the custom apps.
    /// </summary>
    Task DeleteCustomAppAsync(Reference identifier);
    
    /// <summary>
    /// Modifies the custom apps.
    /// </summary>
    /// <returns>The <see cref="CustomAppModel"/> instance that represents the custom app.</returns>
    Task<CustomAppModel> ModifyCustomAppAsync(Reference identifier, IEnumerable<CustomAppOperationBaseModel> changes);
}
