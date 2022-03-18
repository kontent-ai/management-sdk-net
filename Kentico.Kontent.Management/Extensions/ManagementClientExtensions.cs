using System;
using System.Threading.Tasks;
using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.StronglyTyped;

namespace Kentico.Kontent.Management.Extensions;

/// <summary>
/// Extra simplifying methods available for ManagementClient
/// </summary>
public static class ManagementClientExtensions
{
    /// <summary>
    /// Updates the given content item.
    /// </summary>
    /// <param name="client">Content management client instance.</param>
    /// <param name="identifier">Identifies which content item will be updated. </param>
    /// <param name="contentItem">Specifies data for updated content item.</param>
    /// <returns>The <see cref="ContentItemModel"/> instance that represents updated content item.</returns>
    public async static Task<ContentItemModel> UpsertContentItemAsync(this IManagementClient client, Reference identifier, ContentItemModel contentItem)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        if (contentItem == null)
        {
            throw new ArgumentNullException(nameof(contentItem));
        }

        var contentItemUpdateModel = new ContentItemUpsertModel 
        { 
            Name = contentItem.Name,
            Codename = contentItem.Codename,
            Collection = contentItem.Collection,
            ExternalId = contentItem.ExternalId,
            SitemapLocations = contentItem.SitemapLocations,
            Type = contentItem.Type
        };

        return await client.UpsertContentItemAsync(identifier, contentItemUpdateModel);
    }

    /// <summary>
    /// Creates asset.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="fileContent">Represents the content of the file.</param>
    /// <param name="assetCreateModel">Updated values for the asset.</param>
    public async static Task<AssetModel> CreateAssetAsync(this IManagementClient client, FileContentSource fileContent, AssetCreateModel assetCreateModel)
    {
        if (fileContent == null)
        {
            throw new ArgumentNullException(nameof(fileContent));
        }
        if (assetCreateModel == null)
        {
            throw new ArgumentNullException(nameof(assetCreateModel));
        }

        var fileResult = await client.UploadFileAsync(fileContent);

        assetCreateModel.FileReference = fileResult;

        var response = await client.CreateAssetAsync(assetCreateModel);

        return response;
    }

    /// <summary>
    /// Creates asset.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="fileContent">Represents the content of the file.</param>
    /// <param name="assetCreateModel">Updated values for the strongly typed asset.</param>
    public async static Task<AssetModel<T>> CreateAssetAsync<T>(this IManagementClient client, FileContentSource fileContent, AssetCreateModel<T> assetCreateModel) where T: new()
    {
        if (fileContent == null)
        {
            throw new ArgumentNullException(nameof(fileContent));
        }
        if (assetCreateModel == null)
        {
            throw new ArgumentNullException(nameof(assetCreateModel));
        }

        var fileResult = await client.UploadFileAsync(fileContent);

        assetCreateModel.FileReference = fileResult;

        var response = await client.CreateAssetAsync(assetCreateModel);

        return response;
    }

    /// <summary>
    /// Creates or updates the given asset.
    /// </summary>
    /// <param name="client">Content management client instance.</param>
    /// <param name="identifier">The identifier of the asset.</param>
    /// <param name="fileContent">Represents the content of the file.</param>
    /// <param name="upsertModel">Updated values for the asset.</param>
    /// <returns>The <see cref="AssetModel"/> instance that represents created or updated asset.</returns>
    public async static Task<AssetModel> UpsertAssetAsync(this IManagementClient client, Reference identifier, FileContentSource fileContent, AssetUpsertModel upsertModel)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        if (fileContent == null)
        {
            throw new ArgumentNullException(nameof(fileContent));
        }

        if (upsertModel == null)
        {
            throw new ArgumentNullException(nameof(upsertModel));
        }

        var fileResult = await client.UploadFileAsync(fileContent);

        upsertModel.FileReference = fileResult;

        var response = await client.UpsertAssetAsync(identifier, upsertModel);

        return response;
    }

    /// <summary>
    /// Creates or updates the given asset.
    /// </summary>
    /// <param name="client">Content management client instance.</param>
    /// <param name="identifier">The identifier of the asset.</param>
    /// <param name="fileContent">Represents the content of the file.</param>
    /// <param name="upsertModel">Updated values for the asset.</param>
    /// <returns>The <see cref="AssetModel{T}"/> instance that represents created or updated strongly typed asset.</returns>
    public async static Task<AssetModel<T>> UpsertAssetAsync<T>(this IManagementClient client, Reference identifier, FileContentSource fileContent, AssetUpsertModel<T> upsertModel) where T: new()
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        if (fileContent == null)
        {
            throw new ArgumentNullException(nameof(fileContent));
        }

        if (upsertModel == null)
        {
            throw new ArgumentNullException(nameof(upsertModel));
        }

        var fileResult = await client.UploadFileAsync(fileContent);

        upsertModel.FileReference = fileResult;

        var response = await client.UpsertAssetAsync(identifier, upsertModel);

        return response;
    }
}
