using Kontent.Ai.Management.Models.Assets;
using Kontent.Ai.Management.Models.Assets.Patch;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.StronglyTyped;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
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
    public async Task<AssetFoldersModel> GetAssetFoldersAsync()
    {
        var endpointUrl = _urlBuilder.BuildAssetFoldersUrl();
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<AssetFoldersModel>(endpointUrl, HttpMethod.Get);

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
}
