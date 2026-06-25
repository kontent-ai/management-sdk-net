using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.AssetRenditions;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public Task<IManagementResult<IReadOnlyList<AssetRenditionModel>>> ListAssetRenditionsAsync(Reference assetIdentifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(assetIdentifier);

        var assetSegment = assetIdentifier.ToUrlSegment();
        return PageEnumerator.CollectAsync<AssetRenditionsListingResponseServerModel, AssetRenditionModel>(
            (token, ct) => _managementApi.ListAssetRenditionsInternalAsync(assetSegment, token, ct),
            page => page.AssetRenditions,
            page => page.Pagination?.Token,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<IManagementResult<AssetRenditionModel>> GetAssetRenditionAsync(AssetRenditionIdentifier identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return _managementApi.GetAssetRenditionInternalAsync(identifier.ToUrlSegment(), cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public Task<IManagementResult<AssetRenditionModel>> UpdateAssetRenditionAsync(AssetRenditionIdentifier identifier, AssetRenditionUpdateModel updateModel, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(updateModel);

        return _managementApi.UpdateAssetRenditionInternalAsync(identifier.ToUrlSegment(), updateModel, cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public Task<IManagementResult<AssetRenditionModel>> CreateAssetRenditionAsync(Reference assetIdentifier, AssetRenditionCreateModel createModel, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(assetIdentifier);
        ArgumentNullException.ThrowIfNull(createModel);

        return _managementApi.CreateAssetRenditionInternalAsync(assetIdentifier.ToUrlSegment(), createModel, cancellationToken).ToManagementResultAsync();
    }
}
