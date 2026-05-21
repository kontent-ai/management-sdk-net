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
using Kontent.Ai.Management.Models.ItemWithVariant;
using Kontent.Ai.Management.Models.Languages;
using Kontent.Ai.Management.Models.LanguageVariants;
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
using System.Threading;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

/// <summary>
/// Represents set of Content Management API requests. The concrete <see cref="ManagementClient"/> implements
/// <see cref="IDisposable"/> / <see cref="IAsyncDisposable"/> for non-DI lifetime management; DI consumers
/// receive an <see cref="IManagementClient"/> reference and rely on the host container to dispose the underlying
/// instance.
/// </summary>
public interface IManagementClient
{
    /// <summary>
    /// Returns the asset.
    /// </summary>
    /// <param name="identifier">The identifier of the asset.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the requested <see cref="AssetModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<AssetModel>> GetAssetAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Enumerates the assets, one continuation-token page at a time.
    /// </summary>
    /// <remarks>Use the <see cref="Extensions.ListingExtensions.Items{T}"/> extension to flatten the pages into a flat item stream.</remarks>
    /// <param name="cancellationToken">Token to cancel the enumeration.</param>
    /// <returns>An async stream of pages; each yields one page's assets on success, or that page's failure detail.</returns>
    IAsyncEnumerable<IManagementResult<IReadOnlyList<AssetModel>>> EnumerateAssetPagesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates an asset.
    /// </summary>
    /// <param name="asset">Represents the asset that will be created.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the created <see cref="AssetModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<AssetModel>> CreateAssetAsync(AssetCreateModel asset, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the given asset.
    /// </summary>
    /// <param name="identifier">The identifier of the asset.</param>
    /// <param name="asset">Represents the updated asset.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the updated <see cref="AssetModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<AssetModel>> UpsertAssetAsync(Reference identifier, AssetUpsertModel asset, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the given asset.
    /// </summary>
    /// <param name="identifier">The identifier of the asset.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> DeleteAssetAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads the given file.
    /// </summary>
    /// <param name="fileContent">Represents the content of the file.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the <see cref="FileReference"/> to the uploaded file on success, or the failure detail.</returns>
    Task<IManagementResult<FileReference>> UploadFileAsync(FileContentSource fileContent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve a rendition of the specified asset.
    /// </summary>
    /// <param name="identifier">The identifier of the asset rendition.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the requested <see cref="AssetRenditionModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<AssetRenditionModel>> GetAssetRenditionAsync(AssetRenditionIdentifier identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Enumerates the renditions of the specified asset, one continuation-token page at a time.
    /// </summary>
    /// <remarks>Use the <see cref="Extensions.ListingExtensions.Items{T}"/> extension to flatten the pages into a flat item stream.</remarks>
    /// <param name="assetIdentifier">The identifier of the asset.</param>
    /// <param name="cancellationToken">Token to cancel the enumeration.</param>
    /// <returns>An async stream of pages; each yields one page's renditions on success, or that page's failure detail.</returns>
    IAsyncEnumerable<IManagementResult<IReadOnlyList<AssetRenditionModel>>> EnumerateAssetRenditionPagesAsync(Reference assetIdentifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new rendition of the specified asset.
    /// </summary>
    /// <param name="assetIdentifier">The identifier of the asset.</param>
    /// <param name="createModel">Represents the asset rendition that will be created.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the created <see cref="AssetRenditionModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<AssetRenditionModel>> CreateAssetRenditionAsync(Reference assetIdentifier, AssetRenditionCreateModel createModel, CancellationToken cancellationToken = default);

    /// <summary>
    /// Modify a rendition of the asset.
    /// </summary>
    /// <param name="identifier">The identifier of the asset rendition.</param>
    /// <param name="updateModel">Represents the updated asset rendition.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the updated <see cref="AssetRenditionModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<AssetRenditionModel>> UpdateAssetRenditionAsync(AssetRenditionIdentifier identifier, AssetRenditionUpdateModel updateModel, CancellationToken cancellationToken = default);
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
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the created <see cref="AssetFoldersModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<AssetFoldersModel>> CreateAssetFoldersAsync(AssetFolderCreateModel folder, CancellationToken cancellationToken = default);

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
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the created <see cref="LanguageModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<LanguageModel>> CreateLanguageAsync(LanguageCreateModel language, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates the new version of the language variant.
    /// </summary>
    /// <param name="identifier">The identifier of the language variant for which the new version should be created.</param>
    Task CreateNewVersionOfLanguageVariantAsync(LanguageVariantIdentifier identifier);

    /// <summary>
    /// Creates taxonomy group.
    /// </summary>
    /// <param name="taxonomyGroup">Represents the taxonomy group which will be created.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the created <see cref="TaxonomyGroupModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<TaxonomyGroupModel>> CreateTaxonomyGroupAsync(TaxonomyGroupCreateModel taxonomyGroup, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates the webhook.
    /// </summary>
    /// <param name="webhook">The webhook to be created.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the created <see cref="WebhookModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<WebhookModel>> CreateWebhookAsync(WebhookCreateModel webhook, CancellationToken cancellationToken = default);

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
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> DeleteTaxonomyGroupAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the webhook.
    /// </summary>
    /// <param name="identifier">The identifier of the webhook.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> DeleteWebhookAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Disables the webhook.
    /// </summary>
    /// <param name="identifier">The identifier of the webhook.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> DisableWebhookAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Enables the webhook.
    /// </summary>
    /// <param name="identifier">The identifier of the webhook.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> EnableWebhookAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the Asset Folders
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the asset-folder hierarchy on success, or the failure detail.</returns>
    Task<IManagementResult<AssetFoldersModel>> GetAssetFoldersAsync(CancellationToken cancellationToken = default);

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
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the requested <see cref="LanguageModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<LanguageModel>> GetLanguageAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns strongly typed language variant.
    /// </summary>
    /// <param name="identifier">The identifier of the language variant.</param>
    /// <returns>The <see cref="LanguageVariantModel"/> instance that represents language variant.</returns>
    Task<LanguageVariantModel> GetLanguageVariantAsync(LanguageVariantIdentifier identifier);

    /// <summary>
    /// Retrieves a language variant and projects its elements onto the generated content-type record
    /// <typeparamref name="T"/>. Failures (HTTP 4xx/5xx) are surfaced through the returned result rather than thrown;
    /// network-level and serialization failures still propagate as exceptions.
    /// </summary>
    /// <typeparam name="T">The generated content-type record (implements <see cref="IContentItem"/>).</typeparam>
    /// <param name="identifier">The identifier of the language variant.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the populated <typeparamref name="T"/> on success, or the API errors on failure.</returns>
    Task<IManagementResult<T>> GetLanguageVariantAsync<T>(LanguageVariantIdentifier identifier, CancellationToken cancellationToken = default)
        where T : IContentItem, new();

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
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the <see cref="Environment"/> on success, or the failure detail.</returns>
    Task<IManagementResult<Models.EnvironmentReport.Environment>> GetEnvironmentInformationAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns taxonomy group.
    /// </summary>
    /// <param name="identifier">The identifier of the taxonomy group.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the requested <see cref="TaxonomyGroupModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<TaxonomyGroupModel>> GetTaxonomyGroupAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the webhook.
    /// </summary>
    /// <param name="identifier">The identifier of the webhook.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the requested <see cref="WebhookModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<WebhookModel>> GetWebhookAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns listing of collection.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the <see cref="CollectionsModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<CollectionsModel>> ListCollectionsAsync(CancellationToken cancellationToken = default);

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
    /// Enumerates the languages, one continuation-token page at a time.
    /// </summary>
    /// <remarks>Use the <see cref="Extensions.ListingExtensions.Items{T}"/> extension to flatten the pages into a flat item stream.</remarks>
    /// <param name="cancellationToken">Token to cancel the enumeration.</param>
    /// <returns>An async stream of pages; each yields one page's languages on success, or that page's failure detail.</returns>
    IAsyncEnumerable<IManagementResult<IReadOnlyList<LanguageModel>>> EnumerateLanguagePagesAsync(CancellationToken cancellationToken = default);

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
    /// Enumerates the taxonomy groups, one continuation-token page at a time.
    /// </summary>
    /// <remarks>Use the <see cref="Extensions.ListingExtensions.Items{T}"/> extension to flatten the pages into a flat item stream.</remarks>
    /// <param name="cancellationToken">Token to cancel the enumeration.</param>
    /// <returns>An async stream of pages; each yields one page's taxonomy groups on success, or that page's failure detail.</returns>
    IAsyncEnumerable<IManagementResult<IReadOnlyList<TaxonomyGroupModel>>> EnumerateTaxonomyGroupPagesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns listing of webhooks.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the listing of <see cref="WebhookModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<IEnumerable<WebhookModel>>> ListWebhooksAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns listing of workflows.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the listing of <see cref="WorkflowModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<IEnumerable<WorkflowModel>>> ListWorkflowsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the given workflow.
    /// </summary>
    /// <param name="identifier">The identifier of the workflow.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> DeleteWorkflowAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new workflow.
    /// </summary>
    /// <param name="workflow">Represents workflow that will be created.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the created <see cref="WorkflowModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<WorkflowModel>> CreateWorkflowAsync(WorkflowUpsertModel workflow, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update the specified workflow.
    /// </summary>
    /// <param name="identifier">The identifier of the workflow to update.</param>
    /// <param name="workflow">Represents the changed workflow to update.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the updated <see cref="WorkflowModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<WorkflowModel>> UpdateWorkflowAsync(Reference identifier, WorkflowUpsertModel workflow, CancellationToken cancellationToken = default);

    /// <summary>
    /// Modifies the asset folder.
    /// </summary>
    /// <param name="changes">Represents changes that will be applied to the asset folder.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the modified <see cref="AssetFoldersModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<AssetFoldersModel>> ModifyAssetFoldersAsync(IEnumerable<AssetFolderOperationBaseModel> changes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Modifies collection.
    /// </summary>
    /// <param name="changes">Represents changes that will be applied to the collection.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the modified <see cref="CollectionsModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<CollectionsModel>> ModifyCollectionAsync(IEnumerable<CollectionOperationBaseModel> changes, CancellationToken cancellationToken = default);

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
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the modified <see cref="LanguageModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<LanguageModel>> ModifyLanguageAsync(Reference identifier, IEnumerable<LanguagePatchModel> changes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Modifies given taxonomy group.
    /// </summary>
    /// <param name="identifier">The identifier of the taxonomy group.</param>
    /// <param name="changes">Represents changes that will be applied to the taxonomy group.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the modified <see cref="TaxonomyGroupModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<TaxonomyGroupModel>> ModifyTaxonomyGroupAsync(Reference identifier, IEnumerable<TaxonomyGroupOperationBaseModel> changes, CancellationToken cancellationToken = default);

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
    /// Schedules publish and unpublish of language variant.
    /// </summary>
    /// <param name="identifier">The identifier of the language variant that should be scheduled.</param>
    /// <param name="schedule">The interval in which the variant should be published</param>
    Task SchedulePublishingAndUnpublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier, SchedulePublishAndUnpublishModel schedule);

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
    /// Inserts or updates a language variant from the generated content-type record <typeparamref name="T"/>.
    /// The record is validated locally first (<see cref="Validation.ContentItemValidator"/>); a validation failure
    /// short-circuits with no HTTP call. <c>null</c> properties are omitted from the payload (partial update).
    /// Failures (validation, HTTP 4xx/5xx) are surfaced through the returned result rather than thrown;
    /// network-level and serialization failures still propagate as exceptions.
    /// </summary>
    /// <typeparam name="T">The generated content-type record (implements <see cref="IContentItem"/>).</typeparam>
    /// <param name="identifier">The identifier of the language variant.</param>
    /// <param name="variant">The content-type record carrying the elements to set.</param>
    /// <param name="workflow">Optional workflow step to set on the variant.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the upserted variant projected onto <typeparamref name="T"/>, or the errors on failure.</returns>
    Task<IManagementResult<T>> UpsertLanguageVariantAsync<T>(
        LanguageVariantIdentifier identifier,
        T variant,
        WorkflowStepIdentifier? workflow = null,
        CancellationToken cancellationToken = default)
        where T : IContentItem, new();

    /// <summary>
    /// Validates the environment.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the <see cref="EnvironmentReportModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<EnvironmentReportModel>> ValidateEnvironmentAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Initiates asynchronous environment validation.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the <see cref="AsyncValidationTaskModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<AsyncValidationTaskModel>> InitiateEnvironmentAsyncValidationTaskAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets async validation task.
    /// </summary>
    /// <param name="taskId">The identifier of the validation task.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the <see cref="AsyncValidationTaskModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<AsyncValidationTaskModel>> GetAsyncValidationTaskAsync(Guid taskId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Enumerates the issues of an async validation task, one continuation-token page at a time.
    /// </summary>
    /// <remarks>Use the <see cref="Extensions.ListingExtensions.Items{T}"/> extension to flatten the pages into a flat item stream.</remarks>
    /// <param name="taskId">The identifier of the validation task.</param>
    /// <param name="cancellationToken">Token to cancel the enumeration.</param>
    /// <returns>An async stream of pages; each yields one page's issues on success, or that page's failure detail.</returns>
    IAsyncEnumerable<IManagementResult<IReadOnlyList<AsyncValidationTaskIssueModel>>> EnumerateAsyncValidationTaskIssuePagesAsync(Guid taskId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all roles in an environment.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the <see cref="EnvironmentRolesModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<EnvironmentRolesModel>> ListEnvironmentRolesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns environment role.
    /// </summary>
    /// <param name="identifier">The identifier of the environment role.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the requested <see cref="EnvironmentRoleModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<EnvironmentRoleModel>> GetEnvironmentRoleAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Invites a new user to an environment.
    /// </summary>
    /// <param name="invitation">Represents an user that is to be invited.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the newly invited <see cref="UserModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<UserModel>> InviteUserIntoEnvironmentAsync(UserInviteModel invitation, CancellationToken cancellationToken = default);

    /// <summary>
    /// Modifies user's roles.
    /// </summary>
    /// <param name="identifier">The identifier of the environment user.</param>
    /// <param name="user">Represents an user that is to be modified.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the modified <see cref="UserModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<UserModel>> ModifyUsersRolesAsync(UserIdentifier identifier, UserModel user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Enumerates the projects under your subscription, one continuation-token page at a time.
    /// </summary>
    /// <remarks>Use the <see cref="Extensions.ListingExtensions.Items{T}"/> extension to flatten the pages into a flat item stream.</remarks>
    /// <param name="cancellationToken">Token to cancel the enumeration.</param>
    /// <returns>An async stream of pages; each yields one page's projects on success, or that page's failure detail.</returns>
    IAsyncEnumerable<IManagementResult<IReadOnlyList<SubscriptionProjectModel>>> EnumerateSubscriptionProjectPagesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Enumerates the users under your subscription — including their assignment to projects, environments,
    /// collections, roles, and languages — one continuation-token page at a time.
    /// </summary>
    /// <remarks>Use the <see cref="Extensions.ListingExtensions.Items{T}"/> extension to flatten the pages into a flat item stream.</remarks>
    /// <param name="cancellationToken">Token to cancel the enumeration.</param>
    /// <returns>An async stream of pages; each yields one page's users on success, or that page's failure detail.</returns>
    IAsyncEnumerable<IManagementResult<IReadOnlyList<SubscriptionUserModel>>> EnumerateSubscriptionUserPagesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve a user metadata from under the specified subscription.
    /// The metadata include information about the user's access to projects and environments,
    /// and content in specific collections, roles, and languages.
    /// </summary>
    /// <param name="identifier">The identifier of the subscription user.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the <see cref="SubscriptionUserModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<SubscriptionUserModel>> GetSubscriptionUserAsync(UserIdentifier identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Activates the specified user in all projects under the specified subscription.
    /// </summary>
    /// <param name="identifier">The identifier of the subscription user.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> ActivateSubscriptionUserAsync(UserIdentifier identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deactivates the specified user in all projects under the specified subscription.
    /// </summary>
    /// <param name="identifier">The identifier of the subscription user.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> DeactivateSubscriptionUserAsync(UserIdentifier identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clones environment.
    /// </summary>
    /// <param name="cloneEnvironmentModel">Cloning settings.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the <see cref="EnvironmentClonedModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<EnvironmentClonedModel>> CloneEnvironmentAsync(EnvironmentCloneModel cloneEnvironmentModel, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the state of the environment cloning.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the <see cref="EnvironmentCloningStateModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<EnvironmentCloningStateModel>> GetEnvironmentCloningStateAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes current environment.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> DeleteEnvironmentAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks current environment as production.
    /// </summary>
    /// <param name="markAsProductionModel">Represents settings that will be used to mark the environment as production.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> MarkEnvironmentAsProductionAsync(MarkAsProductionModel markAsProductionModel, CancellationToken cancellationToken = default);

    /// <summary>
    /// Modifies current environment.
    /// </summary>
    /// <param name="changes">Represents changes that will be applied to the environment.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the modified <see cref="EnvironmentModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<EnvironmentModel>> ModifyEnvironmentAsync(IEnumerable<EnvironmentOperationBaseModel> changes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates the space.
    /// </summary>
    /// <param name="space">The space to be created.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the created <see cref="SpaceModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<SpaceModel>> CreateSpaceAsync(SpaceCreateModel space, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the space.
    /// </summary>
    /// <param name="identifier">The identifier of the space.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the requested <see cref="SpaceModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<SpaceModel>> GetSpaceAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all spaces.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the listing of <see cref="SpaceModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<IEnumerable<SpaceModel>>> ListSpacesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Modifies the space.
    /// </summary>
    /// <param name="identifier">The identifier of the space.</param>
    /// <param name="changes">The changes that will be applied to the space.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the modified <see cref="SpaceModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<SpaceModel>> ModifySpaceAsync(Reference identifier, IEnumerable<SpaceOperationReplaceModel> changes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the space.
    /// </summary>
    /// <param name="identifier">The identifier of the space.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> DeleteSpaceAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the preview configuration.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the <see cref="PreviewConfigurationModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<PreviewConfigurationModel>> GetPreviewConfigurationAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Modify the preview configuration.
    /// </summary>
    /// <param name="previewConfiguration">Represents configuration that will be used for project.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the <see cref="PreviewConfigurationModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<PreviewConfigurationModel>> ModifyPreviewConfigurationAsync(PreviewConfigurationModel previewConfiguration, CancellationToken cancellationToken = default);

    /// <summary>
    /// Activates the web spotlight, allowing you to specify an existing Root Type ID.
    /// </summary>
    /// <param name="webSpotlightActivateModel">Represents configuration that will be used for web spotlight activation.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the <see cref="WebSpotlightModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<WebSpotlightModel>> ActivateWebSpotlightAsync(WebSpotlightActivateModel webSpotlightActivateModel, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deactivates the web spotlight.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the <see cref="WebSpotlightModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<WebSpotlightModel>> DeactivateWebSpotlightAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the web spotlight status.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the <see cref="WebSpotlightModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<WebSpotlightModel>> GetWebSpotlightStatusAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Enumerates the custom apps, one continuation-token page at a time.
    /// </summary>
    /// <remarks>Use the <see cref="Extensions.ListingExtensions.Items{T}"/> extension to flatten the pages into a flat item stream.</remarks>
    /// <param name="cancellationToken">Token to cancel the enumeration.</param>
    /// <returns>An async stream of pages; each yields one page's custom apps on success, or that page's failure detail.</returns>
    IAsyncEnumerable<IManagementResult<IReadOnlyList<CustomAppModel>>> EnumerateCustomAppPagesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the custom app.
    /// </summary>
    /// <param name="identifier">The identifier of the custom app.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the requested <see cref="CustomAppModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<CustomAppModel>> GetCustomAppAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates the custom apps.
    /// </summary>
    /// <param name="customApp">Represents the custom app that will be created.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the created <see cref="CustomAppModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<CustomAppModel>> CreateCustomAppAsync(CustomAppCreateModel customApp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the custom apps.
    /// </summary>
    /// <param name="identifier">The identifier of the custom app.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> DeleteCustomAppAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Modifies the custom apps.
    /// </summary>
    /// <param name="identifier">The identifier of the custom app.</param>
    /// <param name="changes">Represents changes that will be applied to the custom app.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the modified <see cref="CustomAppModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<CustomAppModel>> ModifyCustomAppAsync(Reference identifier, IEnumerable<CustomAppOperationBaseModel> changes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns listing of filtered item variant references.
    /// The Content management API returns a dynamically paginated listing response limited to up to 50 objects.
    /// To check if the next page is available use <see cref="IListingResponseModel{T}.HasNextPage"/>.
    /// For getting next page use <see cref="IListingResponseModel{T}.GetNextPage"/>.
    /// </summary>
    /// <param name="filterRequest">The filter request containing filters and ordering options.</param>
    /// <returns>The <see cref="IListingResponseModel{ItemWithVariantFilterResultModel}"/> instance that represents the listing of filtered variant references.</returns>
    Task<IListingResponseModel<ItemWithVariantFilterResultModel>> FilterItemsWithVariantsAsync(ItemWithVariantFilterRequestModel filterRequest);

    /// <summary>
    /// Returns listing of content items with their language variants.
    /// The Content management API returns a dynamically paginated listing response limited to up to 50 objects.
    /// To check if the next page is available use <see cref="IListingResponseModel{T}.HasNextPage"/>.
    /// For getting next page use <see cref="IListingResponseModel{T}.GetNextPage"/>.
    /// </summary>
    /// <param name="bulkGetRequest">The bulk-get request containing variant identifiers.</param>
    /// <returns>The <see cref="IListingResponseModel{ContentItemWithVariantModel}"/> instance that represents the listing of content items with variants.</returns>
    Task<IListingResponseModel<ContentItemWithVariantModel>> BulkGetItemsWithVariantsAsync(ItemWithVariantBulkGetRequestModel bulkGetRequest);

}
