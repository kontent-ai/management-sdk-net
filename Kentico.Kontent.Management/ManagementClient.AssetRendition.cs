using Kentico.Kontent.Management.Models.AssetRenditions;
using Kentico.Kontent.Management.Models.Shared;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kentico.Kontent.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<AssetRenditionModel>> ListAssetRenditionsAsync(Reference assetIdentifier)
    {
        if (assetIdentifier == null)
        {
            throw new ArgumentNullException(nameof(assetIdentifier));
        }

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
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildAssetRenditionsUrl(identifier);
        return await _actionInvoker.InvokeReadOnlyMethodAsync<AssetRenditionModel>(endpointUrl, HttpMethod.Get);
    }

    /// <inheritdoc />
    public async Task<AssetRenditionModel> UpdateAssetRenditionAsync(AssetRenditionIdentifier identifier, AssetRenditionUpdateModel updateModel)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }
        if (updateModel == null)
        {
            throw new ArgumentNullException(nameof(updateModel));
        }

        var endpointUrl = _urlBuilder.BuildAssetRenditionsUrl(identifier);
        return await _actionInvoker.InvokeMethodAsync<AssetRenditionUpdateModel, AssetRenditionModel>(endpointUrl, HttpMethod.Put, updateModel);
    }

    /// <inheritdoc />
    public async Task<AssetRenditionModel> CreateAssetRenditionAsync(Reference assetIdentifier, AssetRenditionCreateModel createModel)
    {
        if (assetIdentifier == null)
        {
            throw new ArgumentNullException(nameof(assetIdentifier));
        }

        if (createModel == null)
        {
            throw new ArgumentNullException(nameof(createModel));
        }

        var endpointUrl = _urlBuilder.BuildAssetRenditionsUrl(assetIdentifier);
        return await _actionInvoker.InvokeMethodAsync<AssetRenditionCreateModel, AssetRenditionModel>(endpointUrl, HttpMethod.Post, createModel);
    }
}
