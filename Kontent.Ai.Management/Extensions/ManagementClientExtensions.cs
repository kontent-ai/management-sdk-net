using Kontent.Ai.Management.Models.Assets;
using Kontent.Ai.Management.Models.Items;

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
            ExternalId = contentItem.ExternalId,
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
            return ManagementResult<AssetModel>.Failure(fileResult.Error!, fileResult.StatusCode, fileResult.RequestUrl, fileResult.ResponseHeaders);
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
            return ManagementResult<AssetModel>.Failure(fileResult.Error!, fileResult.StatusCode, fileResult.RequestUrl, fileResult.ResponseHeaders);
        }

        return await client.UpsertAssetAsync(identifier, upsertModel with { FileReference = fileResult.Value }, cancellationToken);
    }
}
