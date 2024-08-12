using Kontent.Ai.Management.Models.AssetRenditions;
using Kontent.Ai.Management.Models.Shared;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<AssetRenditionModel>> ListAssetRenditionsAsync(Reference assetIdentifier)
    {
        ArgumentNullException.ThrowIfNull(assetIdentifier);

        var endpointUrl = _urlBuilder.BuildAssetRenditionsUrl(assetIdentifier);
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<AssetRenditionsListingResponseServerModel>(endpointUrl, HttpMethod.Get);

        return new ListingResponseModel<AssetRenditionModel>(
            GetNextListingPageAsync<AssetRenditionsListingResponseServerModel, AssetRenditionModel>,
            response.Pagination?.Token,
            endpointUrl,
            response.AssetRenditions);
    }

    /// <inheritdoc />
    public async Task<AssetRenditionModel> GetAssetRenditionAsync(AssetRenditionIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildAssetRenditionsUrl(identifier);
        return await _actionInvoker.InvokeReadOnlyMethodAsync<AssetRenditionModel>(endpointUrl, HttpMethod.Get);
    }

    /// <inheritdoc />
    public async Task<AssetRenditionModel> UpdateAssetRenditionAsync(AssetRenditionIdentifier identifier, AssetRenditionUpdateModel updateModel)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(updateModel);

        var endpointUrl = _urlBuilder.BuildAssetRenditionsUrl(identifier);
        return await _actionInvoker.InvokeMethodAsync<AssetRenditionUpdateModel, AssetRenditionModel>(endpointUrl, HttpMethod.Put, updateModel);
    }

    /// <inheritdoc />
    public async Task<AssetRenditionModel> CreateAssetRenditionAsync(Reference assetIdentifier, AssetRenditionCreateModel createModel)
    {
        ArgumentNullException.ThrowIfNull(assetIdentifier);

        ArgumentNullException.ThrowIfNull(createModel);

        var endpointUrl = _urlBuilder.BuildAssetRenditionsUrl(assetIdentifier);
        return await _actionInvoker.InvokeMethodAsync<AssetRenditionCreateModel, AssetRenditionModel>(endpointUrl, HttpMethod.Post, createModel);
    }
}
