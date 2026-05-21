using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.AssetRenditions;
using Kontent.Ai.Management.Models.Shared;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public IAsyncEnumerable<IManagementResult<IReadOnlyList<AssetRenditionModel>>> EnumerateAssetRenditionPagesAsync(Reference assetIdentifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(assetIdentifier);

        var assetSegment = assetIdentifier.ToUrlSegment();
        return PageEnumerator.EnumerateAsync<AssetRenditionsListingResponseServerModel, AssetRenditionModel>(
            (token, ct) => _managementApi.ListAssetRenditionsInternalAsync(assetSegment, token, ct),
            page => page.AssetRenditions,
            page => page.Pagination?.Token,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<AssetRenditionModel>> GetAssetRenditionAsync(AssetRenditionIdentifier identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.GetAssetRenditionInternalAsync(identifier.ToUrlSegment(), cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<AssetRenditionModel>> UpdateAssetRenditionAsync(AssetRenditionIdentifier identifier, AssetRenditionUpdateModel updateModel, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(updateModel);

        var response = await _managementApi.UpdateAssetRenditionInternalAsync(identifier.ToUrlSegment(), updateModel, cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<AssetRenditionModel>> CreateAssetRenditionAsync(Reference assetIdentifier, AssetRenditionCreateModel createModel, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(assetIdentifier);
        ArgumentNullException.ThrowIfNull(createModel);

        var response = await _managementApi.CreateAssetRenditionInternalAsync(assetIdentifier.ToUrlSegment(), createModel, cancellationToken);
        return await response.ToManagementResultAsync();
    }
}
