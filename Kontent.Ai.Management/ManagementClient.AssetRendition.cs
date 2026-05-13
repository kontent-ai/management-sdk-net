using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Models.AssetRenditions;
using Kontent.Ai.Management.Models.Shared;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<AssetRenditionModel>> ListAssetRenditionsAsync(Reference assetIdentifier)
    {
        ArgumentNullException.ThrowIfNull(assetIdentifier);

        var assetSegment = assetIdentifier.ToUrlSegment();
        var response = EnsureSuccess(await _managementApi.ListAssetRenditionsInternalAsync(assetSegment));

        return new ListingResponseModel<AssetRenditionModel>(
            (continuationToken, _) => GetNextAssetRenditionsPageAsync(assetSegment, continuationToken),
            response.Pagination?.Token,
            url: string.Empty,
            response.AssetRenditions);
    }

    private async Task<IListingResponse<AssetRenditionModel>> GetNextAssetRenditionsPageAsync(string assetSegment, string continuationToken)
        => EnsureSuccess(await _managementApi.ListAssetRenditionsInternalAsync(assetSegment, continuationToken));

    /// <inheritdoc />
    public async Task<AssetRenditionModel> GetAssetRenditionAsync(AssetRenditionIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return EnsureSuccess(await _managementApi.GetAssetRenditionInternalAsync(identifier.ToUrlSegment()));
    }

    /// <inheritdoc />
    public async Task<AssetRenditionModel> UpdateAssetRenditionAsync(AssetRenditionIdentifier identifier, AssetRenditionUpdateModel updateModel)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(updateModel);

        return EnsureSuccess(await _managementApi.UpdateAssetRenditionInternalAsync(identifier.ToUrlSegment(), updateModel));
    }

    /// <inheritdoc />
    public async Task<AssetRenditionModel> CreateAssetRenditionAsync(Reference assetIdentifier, AssetRenditionCreateModel createModel)
    {
        ArgumentNullException.ThrowIfNull(assetIdentifier);
        ArgumentNullException.ThrowIfNull(createModel);

        return EnsureSuccess(await _managementApi.CreateAssetRenditionInternalAsync(assetIdentifier.ToUrlSegment(), createModel));
    }
}
