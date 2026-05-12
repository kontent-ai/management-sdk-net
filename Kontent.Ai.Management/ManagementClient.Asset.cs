using System.Net.Http.Headers;
using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Models.Assets;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.StronglyTyped;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<AssetModel>> ListAssetsAsync()
    {
        var response = EnsureSuccess(await _managementApi.ListAssetsInternalAsync());

        return new ListingResponseModel<AssetModel>(
            (continuationToken, _) => GetNextAssetsPageAsync(continuationToken),
            response.Pagination?.Token,
            url: string.Empty,
            response.Assets);
    }

    /// <inheritdoc />
    public async Task<IListingResponseModel<AssetModel<T>>> ListAssetsAsync<T>() where T : new()
    {
        var response = EnsureSuccess(await _managementApi.ListAssetsInternalAsync());

        return new ListingResponseMappedModel<AssetModel, AssetModel<T>>(
            (continuationToken, _) => GetNextAssetsPageAsync(continuationToken),
            response.Pagination?.Token,
            url: string.Empty,
            response.Assets,
            _modelProvider.GetAssetModel<T>);
    }

    private async Task<IListingResponse<AssetModel>> GetNextAssetsPageAsync(string continuationToken)
        => EnsureSuccess(await _managementApi.ListAssetsInternalAsync(continuationToken));

    /// <inheritdoc />
    public async Task<AssetModel> GetAssetAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return EnsureSuccess(await _managementApi.GetAssetInternalAsync(identifier.ToUrlSegment()));
    }

    /// <inheritdoc />
    public async Task<AssetModel<T>> GetAssetAsync<T>(Reference identifier) where T : new()
        => _modelProvider.GetAssetModel<T>(await GetAssetAsync(identifier));

    /// <inheritdoc />
    public async Task<AssetModel> UpsertAssetAsync(Reference identifier, AssetUpsertModel asset)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(asset);

        return EnsureSuccess(await _managementApi.UpsertAssetInternalAsync(identifier.ToUrlSegment(), asset));
    }

    /// <inheritdoc />
    public async Task<AssetModel<T>> UpsertAssetAsync<T>(Reference identifier, AssetUpsertModel<T> asset) where T : new()
    {
        ArgumentNullException.ThrowIfNull(asset);

        return _modelProvider.GetAssetModel<T>(await UpsertAssetAsync(identifier, _modelProvider.GetAssetUpsertModel(asset)));
    }

    /// <inheritdoc />
    public async Task<AssetModel> CreateAssetAsync(AssetCreateModel asset)
    {
        ArgumentNullException.ThrowIfNull(asset);

        return EnsureSuccess(await _managementApi.CreateAssetInternalAsync(asset));
    }

    /// <inheritdoc />
    public async Task<AssetModel<T>> CreateAssetAsync<T>(AssetCreateModel<T> asset) where T : new()
    {
        ArgumentNullException.ThrowIfNull(asset);

        return _modelProvider.GetAssetModel<T>(await CreateAssetAsync(_modelProvider.GetAssetCreateModel(asset)));
    }

    /// <inheritdoc />
    public async Task DeleteAssetAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.DeleteAssetInternalAsync(identifier.ToUrlSegment()));
    }

    /// <inheritdoc />
    public async Task<FileReference> UploadFileAsync(FileContentSource fileContent)
    {
        ArgumentNullException.ThrowIfNull(fileContent);

        var stream = fileContent.OpenReadStream();
        try
        {
            if (stream.Length > MAX_FILE_SIZE_MB * 1024 * 1024)
            {
                throw new ArgumentException($"Maximum supported file size is {MAX_FILE_SIZE_MB} MB.", nameof(stream));
            }

            var content = new StreamContent(stream);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(fileContent.ContentType);
            content.Headers.ContentLength = stream.Length;

            return EnsureSuccess(await _managementApi.UploadFileInternalAsync(fileContent.FileName, content));
        }
        finally
        {
            // Dispose the stream only in case a new stream was created.
            if (fileContent.CreatesNewStream)
            {
                stream.Dispose();
            }
        }
    }
}
