using Kontent.Ai.Management.Models.Assets;
using Kontent.Ai.Management.Models.Items;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Workflow;

namespace Kontent.Ai.Management.Extensions;

/// <summary>
/// Extra simplifying methods available for <see cref="IManagementClient"/>.
/// </summary>
public static class ManagementClientExtensions
{
    /// <summary>
    /// Updates the given content item.
    /// </summary>
    /// <param name="client">Content management client instance.</param>
    /// <param name="identifier">Identifies which content item will be updated.</param>
    /// <param name="contentItem">Specifies data for updated content item.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the updated <see cref="ContentItemModel"/> on success, or the failure detail.</returns>
    public async static Task<IManagementResult<ContentItemModel>> UpsertContentItemAsync(this IManagementClient client, Reference identifier, ContentItemModel contentItem, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(contentItem);

        var contentItemUpdateModel = new ContentItemUpsertModel
        {
            Name = contentItem.Name,
            Codename = contentItem.Codename,
            Collection = contentItem.Collection,
            SitemapLocations = contentItem.SitemapLocations,
            Type = contentItem.Type
        };

        return await client.UpsertContentItemAsync(identifier, contentItemUpdateModel, cancellationToken);
    }

    /// <summary>
    /// Uploads the file and creates an asset that references it.
    /// </summary>
    /// <param name="client">Content management client instance.</param>
    /// <param name="fileContent">Represents the content of the file.</param>
    /// <param name="assetCreateModel">Values for the created asset; its <see cref="AssetCreateModel.FileReference"/> is set from the uploaded file.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the created asset, or the failure detail of the file upload or the create.</returns>
    public async static Task<IManagementResult<AssetModel>> CreateAssetAsync(this IManagementClient client, FileContentSource fileContent, AssetCreateModel assetCreateModel, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileContent);
        ArgumentNullException.ThrowIfNull(assetCreateModel);

        var fileResult = await client.UploadFileAsync(fileContent, cancellationToken);
        if (!fileResult.IsSuccess)
        {
            return ProjectFailure<AssetModel>(fileResult);
        }

        return await client.CreateAssetAsync(assetCreateModel with { FileReference = fileResult.Value }, cancellationToken);
    }

    /// <summary>
    /// Uploads the file and creates or updates the asset that references it.
    /// </summary>
    /// <param name="client">Content management client instance.</param>
    /// <param name="identifier">The identifier of the asset.</param>
    /// <param name="fileContent">Represents the content of the file.</param>
    /// <param name="upsertModel">Values for the upserted asset; its <see cref="AssetUpsertModel.FileReference"/> is set from the uploaded file.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the upserted asset, or the failure detail of the file upload or the upsert.</returns>
    public async static Task<IManagementResult<AssetModel>> UpsertAssetAsync(this IManagementClient client, Reference identifier, FileContentSource fileContent, AssetUpsertModel upsertModel, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(fileContent);
        ArgumentNullException.ThrowIfNull(upsertModel);

        var fileResult = await client.UploadFileAsync(fileContent, cancellationToken);
        if (!fileResult.IsSuccess)
        {
            return ProjectFailure<AssetModel>(fileResult);
        }

        return await client.UpsertAssetAsync(identifier, upsertModel with { FileReference = fileResult.Value }, cancellationToken);
    }

    /// <summary>
    /// Creates a content item and upserts one of its language variants in a single call.
    /// </summary>
    /// <remarks>
    /// On a partial failure — the item is created but the variant upsert fails — the created item is left in place
    /// (no rollback) and the returned failure carries the variant call's detail. Set
    /// <see cref="ContentItemCreateModel.ExternalId"/> so a retry reuses the same item instead of creating a duplicate.
    /// </remarks>
    /// <param name="client">Content management client instance.</param>
    /// <param name="item">The content item to create.</param>
    /// <param name="language">The language of the variant to upsert on the created item.</param>
    /// <param name="variant">The variant data to upsert.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the upserted variant, or the failure detail of the item creation or the variant upsert.</returns>
    public async static Task<IManagementResult<LanguageVariantModel>> CreateContentItemWithVariantAsync(this IManagementClient client, ContentItemCreateModel item, Reference language, LanguageVariantUpsertModel variant, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(language);
        ArgumentNullException.ThrowIfNull(variant);

        var itemResult = await client.CreateContentItemAsync(item, cancellationToken);
        if (!itemResult.IsSuccess)
        {
            return ProjectFailure<LanguageVariantModel>(itemResult);
        }

        var identifier = new LanguageVariantIdentifier(Reference.ById(itemResult.Value.Id), language);
        return await client.UpsertLanguageVariantAsync(identifier, variant, cancellationToken);
    }

    /// <summary>
    /// Creates a content item and upserts one of its language variants from the generated content-type record
    /// <typeparamref name="T"/> in a single call.
    /// </summary>
    /// <remarks>
    /// On a partial failure — the item is created but the variant upsert fails — the created item is left in place
    /// (no rollback) and the returned failure carries the variant call's detail. Set
    /// <see cref="ContentItemCreateModel.ExternalId"/> so a retry reuses the same item instead of creating a duplicate.
    /// </remarks>
    /// <typeparam name="T">The generated content-type record (implements <see cref="IElementsModel"/>).</typeparam>
    /// <param name="client">Content management client instance.</param>
    /// <param name="item">The content item to create.</param>
    /// <param name="language">The language of the variant to upsert on the created item.</param>
    /// <param name="variant">The content-type record carrying the elements to set.</param>
    /// <param name="workflow">Optional workflow step to set on the variant.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A result wrapping the upserted variant — its element values as <typeparamref name="T"/> plus the item,
    /// language, workflow and other variant metadata — or the failure detail of the item creation or the variant upsert.</returns>
    public async static Task<IManagementResult<LanguageVariantModel<T>>> CreateContentItemWithVariantAsync<T>(this IManagementClient client, ContentItemCreateModel item, Reference language, T variant, WorkflowStepIdentifier? workflow = null, CancellationToken cancellationToken = default)
        where T : IElementsModel, new()
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(language);
        ArgumentNullException.ThrowIfNull(variant);

        var itemResult = await client.CreateContentItemAsync(item, cancellationToken);
        if (!itemResult.IsSuccess)
        {
            return ProjectFailure<LanguageVariantModel<T>>(itemResult);
        }

        var identifier = new LanguageVariantIdentifier(Reference.ById(itemResult.Value.Id), language);
        return await client.UpsertLanguageVariantAsync(identifier, variant, workflow, cancellationToken);
    }

    private static ManagementResult<TTo> ProjectFailure<TTo>(IManagementResult source) =>
        ManagementResult<TTo>.Failure(source.Error!, source.StatusCode, source.RequestUrl, source.ResponseHeaders);
}
