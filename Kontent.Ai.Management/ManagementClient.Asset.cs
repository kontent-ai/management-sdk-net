using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Assets;
using System.Net.Http.Headers;

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

        var stream = fileContent.OpenReadStream();
        try
        {
            if (stream.Length > MAX_FILE_SIZE_MB * 1024 * 1024)
            {
                throw new ArgumentException($"Maximum supported file size is {MAX_FILE_SIZE_MB} MB.", nameof(fileContent));
            }

            var content = new StreamContent(stream);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(fileContent.ContentType);
            content.Headers.ContentLength = stream.Length;

            var response = await _managementApi.UploadFileInternalAsync(fileContent.FileName, content, cancellationToken).ConfigureAwait(false);
            return await response.ToManagementResultAsync().ConfigureAwait(false);
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
