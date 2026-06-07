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
using Kontent.Ai.Management.Models.Spaces;
using Kontent.Ai.Management.Models.Spaces.Patch;
using Kontent.Ai.Management.Models.Subscription;
using Kontent.Ai.Management.Models.TaxonomyGroups;
using Kontent.Ai.Management.Models.TaxonomyGroups.Patch;
using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management.Models.Types.Patch;
using Kontent.Ai.Management.Models.TypeSnippets;
using Kontent.Ai.Management.Models.TypeSnippets.Patch;
using Kontent.Ai.Management.Models.Users;
using Kontent.Ai.Management.Models.Webhooks;
using Kontent.Ai.Management.Models.Workflow;

namespace Kontent.Ai.Management;

/// <summary>
/// Represents set of Content Management API requests. The client owns its underlying HTTP resources when built
/// standalone (via the <see cref="ManagementClient"/> constructor or <see cref="Configuration.ManagementClientBuilder"/>),
/// so it is <see cref="IDisposable"/> / <see cref="IAsyncDisposable"/> — dispose it (or <c>await using</c> it) to
/// release them. For DI-managed instances disposal is a no-op; the host container owns the lifetime.
/// </summary>
public interface IManagementClient : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Returns the asset.
    /// </summary>
    /// <param name="identifier">The identifier of the asset.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the requested <see cref="AssetModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<AssetModel>> GetAssetAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all assets.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping all assets on success, or the first failed page's detail.</returns>
    Task<IManagementResult<IReadOnlyList<AssetModel>>> ListAssetsAsync(CancellationToken cancellationToken = default);

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
    /// Lists all renditions of the specified asset.
    /// </summary>
    /// <param name="assetIdentifier">The identifier of the asset.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping all renditions on success, or the first failed page's detail.</returns>
    Task<IManagementResult<IReadOnlyList<AssetRenditionModel>>> ListAssetRenditionsAsync(Reference assetIdentifier, CancellationToken cancellationToken = default);

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
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> CancelPublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels unpublishing of the language variant.
    /// </summary>
    /// <param name="identifier">The identifier of the language variant of which unpublishing should be canceled.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> CancelUnpublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Changes workflow.
    /// </summary>
    /// <param name="identifier">Identifier of the language variant to be changed.</param>
    /// <param name="changeModel">Change language variant workflow model.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> ChangeLanguageVariantWorkflowAsync(LanguageVariantIdentifier identifier, ChangeLanguageVariantWorkflowModel changeModel, CancellationToken cancellationToken = default);

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
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the created <see cref="ContentItemModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<ContentItemModel>> CreateContentItemAsync(ContentItemCreateModel contentItem, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates content type.
    /// </summary>
    /// <param name="contentType">Represents content type that will be created.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the created <see cref="ContentTypeModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<ContentTypeModel>> CreateContentTypeAsync(ContentTypeCreateModel contentType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates content type snippet.
    /// </summary>
    /// <param name="contentTypeSnippet">Represents content type snippet which will be created.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the created <see cref="ContentTypeSnippetModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<ContentTypeSnippetModel>> CreateContentTypeSnippetAsync(ContentTypeSnippetCreateModel contentTypeSnippet, CancellationToken cancellationToken = default);

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
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> CreateNewVersionOfLanguageVariantAsync(LanguageVariantIdentifier identifier, CancellationToken cancellationToken = default);

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
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> DeleteContentItemAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the given content type.
    /// </summary>
    /// <param name="identifier">The identifier of the content type.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> DeleteContentTypeAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the given content type snippet.
    /// </summary>
    /// <param name="identifier">The identifier of the content type snippet.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> DeleteContentTypeSnippetAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the given language variant.
    /// </summary>
    /// <param name="identifier">The identifier of the language variant.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> DeleteLanguageVariantAsync(LanguageVariantIdentifier identifier, CancellationToken cancellationToken = default);

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
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the requested <see cref="ContentItemModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<ContentItemModel>> GetContentItemAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns content type.
    /// </summary>
    /// <param name="identifier">The identifier of the content type.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the requested <see cref="ContentTypeModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<ContentTypeModel>> GetContentTypeAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns content type snippet.
    /// </summary>
    /// <param name="identifier">The identifier of the content type snippet.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the requested <see cref="ContentTypeSnippetModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<ContentTypeSnippetModel>> GetContentTypeSnippetAsync(Reference identifier, CancellationToken cancellationToken = default);

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
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the requested <see cref="LanguageVariantModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<LanguageVariantModel>> GetLanguageVariantAsync(LanguageVariantIdentifier identifier, CancellationToken cancellationToken = default);

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
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the requested <see cref="LanguageVariantModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<LanguageVariantModel>> GetPublishedLanguageVariantAsync(LanguageVariantIdentifier identifier, CancellationToken cancellationToken = default);

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

    /// <summary>Lists all environment's content items.</summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping all content items on success, or the first failed page's detail.</returns>
    Task<IManagementResult<IReadOnlyList<ContentItemModel>>> ListContentItemsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all environment's content types.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping all content types on success, or the first failed page's detail.</returns>
    Task<IManagementResult<IReadOnlyList<ContentTypeModel>>> ListContentTypesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all environment's content type snippets.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping all content type snippets on success, or the first failed page's detail.</returns>
    Task<IManagementResult<IReadOnlyList<ContentTypeSnippetModel>>> ListContentTypeSnippetsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all languages.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping all languages on success, or the first failed page's detail.</returns>
    Task<IManagementResult<IReadOnlyList<LanguageModel>>> ListLanguagesAsync(CancellationToken cancellationToken = default);

    /// <summary>Lists all language variants for the specified collection.</summary>
    /// <param name="identifier">The identifier of the collection.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping all language variants on success, or the first failed page's detail.</returns>
    Task<IManagementResult<IReadOnlyList<LanguageVariantModel>>> ListLanguageVariantsByCollectionAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>Lists all language variants for the specified space.</summary>
    /// <param name="identifier">The identifier of the space.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping all language variants on success, or the first failed page's detail.</returns>
    Task<IManagementResult<IReadOnlyList<LanguageVariantModel>>> ListLanguageVariantsBySpaceAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns strongly typed listing of language variants for the specified content item.
    /// </summary>
    /// <param name="identifier">The identifier of the content item.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the listing of <see cref="LanguageVariantModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<IReadOnlyList<LanguageVariantModel>>> ListLanguageVariantsByItemAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>Lists all language variants for the specified content type.</summary>
    /// <param name="identifier">The identifier of the content type.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping all language variants on success, or the first failed page's detail.</returns>
    Task<IManagementResult<IReadOnlyList<LanguageVariantModel>>> ListLanguageVariantsByTypeAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>Lists all language variants containing components for the specified content type.</summary>
    /// <param name="identifier">The identifier of the content type.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping all language variants on success, or the first failed page's detail.</returns>
    Task<IManagementResult<IReadOnlyList<LanguageVariantModel>>> ListLanguageVariantsOfContentTypeWithComponentsAsync(Reference identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all taxonomy groups.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping all taxonomy groups on success, or the first failed page's detail.</returns>
    Task<IManagementResult<IReadOnlyList<TaxonomyGroupModel>>> ListTaxonomyGroupsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns listing of webhooks.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the listing of <see cref="WebhookModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<IReadOnlyList<WebhookModel>>> ListWebhooksAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns listing of workflows.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the listing of <see cref="WorkflowModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<IReadOnlyList<WorkflowModel>>> ListWorkflowsAsync(CancellationToken cancellationToken = default);

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
    /// <param name="changes">Represents changes that will be applied to the content type.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the modified <see cref="ContentTypeModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<ContentTypeModel>> ModifyContentTypeAsync(Reference identifier, IEnumerable<ContentTypeOperationBaseModel> changes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Modifies content type snippet.
    /// </summary>
    /// <param name="identifier">The identifier of the content type snippet.</param>
    /// <param name="changes">Represents changes that will be applied to the content type snippet.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the modified <see cref="ContentTypeSnippetModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<ContentTypeSnippetModel>> ModifyContentTypeSnippetAsync(Reference identifier, IEnumerable<ContentTypeSnippetOperationBaseModel> changes, CancellationToken cancellationToken = default);

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
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> PublishLanguageVariantAsync(LanguageVariantIdentifier identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Schedules publishing of the language variant.
    /// </summary>
    /// <param name="identifier">The identifier of the language variant to be published.</param>
    /// <param name="scheduleModel">The time when the language variant will be published</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> SchedulePublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier, ScheduleModel scheduleModel, CancellationToken cancellationToken = default);

    /// <summary>
    /// Schedules unpublishing of the language variant.
    /// </summary>
    /// <param name="identifier">The identifier of the language variant that should be unpublished.</param>
    /// <param name="scheduleModel">The time when the language variant will be unpublished</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> ScheduleUnpublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier, ScheduleModel scheduleModel, CancellationToken cancellationToken = default);

    /// <summary>
    /// Schedules publish and unpublish of language variant.
    /// </summary>
    /// <param name="identifier">The identifier of the language variant that should be scheduled.</param>
    /// <param name="schedule">The interval in which the variant should be published</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> SchedulePublishingAndUnpublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier, SchedulePublishAndUnpublishModel schedule, CancellationToken cancellationToken = default);

    /// <summary>
    /// Unpublishes the language variant.
    /// </summary>
    /// <param name="identifier">Identifier of the language variant to be unpublished.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result indicating success, or the failure detail.</returns>
    Task<IManagementResult> UnpublishLanguageVariantAsync(LanguageVariantIdentifier identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the given content item.
    /// </summary>
    /// <param name="identifier">The identifier of the content item.</param>
    /// <param name="contentItem">Represents the updated content item.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the updated <see cref="ContentItemModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<ContentItemModel>> UpsertContentItemAsync(Reference identifier, ContentItemUpsertModel contentItem, CancellationToken cancellationToken = default);

    /// <summary>
    /// Inserts or updates given language variant.
    /// </summary>
    /// <param name="identifier">The identifier of the language variant.</param>
    /// <param name="languageVariantUpsertModel">Represents the inserted or updated language variant.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the inserted or updated <see cref="LanguageVariantModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<LanguageVariantModel>> UpsertLanguageVariantAsync(LanguageVariantIdentifier identifier, LanguageVariantUpsertModel languageVariantUpsertModel, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates or updates the given content item variant.
    /// </summary>
    /// <param name="identifier">The identifier of the language variant.</param>
    /// <param name="languageVariant">Represents the language variant which data will be used to create <see cref="LanguageVariantUpsertModel"/>.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the created or updated <see cref="LanguageVariantModel"/> on success, or the failure detail.</returns>
    Task<IManagementResult<LanguageVariantModel>> UpsertLanguageVariantAsync(LanguageVariantIdentifier identifier, LanguageVariantModel languageVariant, CancellationToken cancellationToken = default);

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
    /// Lists all issues of an async validation task.
    /// </summary>
    /// <param name="taskId">The identifier of the validation task.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping all issues on success, or the first failed page's detail.</returns>
    Task<IManagementResult<IReadOnlyList<AsyncValidationTaskIssueModel>>> ListAsyncValidationTaskIssuesAsync(Guid taskId, CancellationToken cancellationToken = default);

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
    /// Lists all projects under your subscription.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping all projects on success, or the first failed page's detail.</returns>
    Task<IManagementResult<IReadOnlyList<SubscriptionProjectModel>>> ListSubscriptionProjectsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all users under your subscription — including their assignment to projects, environments,
    /// collections, roles, and languages — one continuation-token page at a time.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping all users on success, or the first failed page's detail.</returns>
    Task<IManagementResult<IReadOnlyList<SubscriptionUserModel>>> ListSubscriptionUsersAsync(CancellationToken cancellationToken = default);

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
    Task<IManagementResult<IReadOnlyList<SpaceModel>>> ListSpacesAsync(CancellationToken cancellationToken = default);

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
    /// Lists all custom apps.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping all custom apps on success, or the first failed page's detail.</returns>
    Task<IManagementResult<IReadOnlyList<CustomAppModel>>> ListCustomAppsAsync(CancellationToken cancellationToken = default);

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

    /// <summary>Lists all filtered item variant references.</summary>
    /// <param name="filterRequest">The filter request containing filters and ordering options.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping all filtered variant references on success, or the first failed page's detail.</returns>
    Task<IManagementResult<IReadOnlyList<ItemWithVariantFilterResultModel>>> ListItemsWithVariantsByFilterAsync(ItemWithVariantFilterRequestModel filterRequest, CancellationToken cancellationToken = default);

    /// <summary>Lists all content items with their language variants.</summary>
    /// <param name="bulkGetRequest">The bulk-get request containing variant identifiers.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping all content items with variants on success, or the first failed page's detail.</returns>
    Task<IManagementResult<IReadOnlyList<ContentItemWithVariantModel>>> ListItemsWithVariantsByBulkGetAsync(ItemWithVariantBulkGetRequestModel bulkGetRequest, CancellationToken cancellationToken = default);

}
