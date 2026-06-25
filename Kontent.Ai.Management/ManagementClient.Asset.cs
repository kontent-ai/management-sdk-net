using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Assets;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public Task<IManagementResult<IReadOnlyList<AssetModel>>> ListAssetsAsync(CancellationToken cancellationToken = default)
        => PageEnumerator.CollectAsync<AssetListingResponseServerModel, AssetModel>(
            _managementApi.ListAssetsInternalAsync,
            page => page.Assets,
            page => page.Pagination?.Token,
            cancellationToken);

    /// <inheritdoc />
    public IAsyncEnumerable<IManagementResult<IReadOnlyList<AssetModel>>> EnumerateAssetPagesAsync(CancellationToken cancellationToken = default)
        => PageEnumerator.EnumerateAsync<AssetListingResponseServerModel, AssetModel>(
            _managementApi.ListAssetsInternalAsync,
            page => page.Assets,
            page => page.Pagination?.Token,
            cancellationToken);

    /// <inheritdoc />
    public async Task<IManagementResult<AssetModel>> GetAssetAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.GetAssetInternalAsync(identifier.ToUrlSegment(), cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<AssetModel>> UpsertAssetAsync(Reference identifier, AssetUpsertModel asset, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(asset);

        var response = await _managementApi.UpsertAssetInternalAsync(identifier.ToUrlSegment(), asset, cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<AssetModel>> CreateAssetAsync(AssetCreateModel asset, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(asset);

        var response = await _managementApi.CreateAssetInternalAsync(asset, cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult> DeleteAssetAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.DeleteAssetInternalAsync(identifier.ToUrlSegment(), cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<FileReference>> UploadFileAsync(FileContentSource fileContent, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileContent);

        using var content = new FileUploadContent(fileContent);
        var response = await _managementApi.UploadFileInternalAsync(fileContent.FileName, content, cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }
}
