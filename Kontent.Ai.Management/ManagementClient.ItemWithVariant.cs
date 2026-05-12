using Kontent.Ai.Management.Models.ItemWithVariant;
using Kontent.Ai.Management.Models.Shared;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<ItemWithVariantFilterResultModel>> FilterItemsWithVariantsAsync(ItemWithVariantFilterRequestModel filterRequest)
    {
        ArgumentNullException.ThrowIfNull(filterRequest);

        var response = EnsureSuccess(await _managementApi.FilterItemsWithVariantsInternalAsync(filterRequest));

        return new ListingResponseModel<ItemWithVariantFilterResultModel>(
            (continuationToken, _) => GetNextFilterItemsWithVariantsPageAsync(continuationToken, filterRequest),
            response.Pagination?.Token,
            url: string.Empty,
            response.Variants);
    }

    /// <inheritdoc />
    public async Task<IListingResponseModel<ContentItemWithVariantModel>> BulkGetItemsWithVariantsAsync(ItemWithVariantBulkGetRequestModel bulkGetRequest)
    {
        ArgumentNullException.ThrowIfNull(bulkGetRequest);

        var response = EnsureSuccess(await _managementApi.BulkGetItemsWithVariantsInternalAsync(bulkGetRequest));

        return new ListingResponseModel<ContentItemWithVariantModel>(
            (continuationToken, _) => GetNextBulkGetItemsWithVariantsPageAsync(continuationToken, bulkGetRequest),
            response.Pagination?.Token,
            url: string.Empty,
            response.Data);
    }

    private async Task<IListingResponse<ItemWithVariantFilterResultModel>> GetNextFilterItemsWithVariantsPageAsync(string continuationToken, ItemWithVariantFilterRequestModel filterRequest)
        => EnsureSuccess(await _managementApi.FilterItemsWithVariantsInternalAsync(filterRequest, continuationToken));

    private async Task<IListingResponse<ContentItemWithVariantModel>> GetNextBulkGetItemsWithVariantsPageAsync(string continuationToken, ItemWithVariantBulkGetRequestModel bulkGetRequest)
        => EnsureSuccess(await _managementApi.BulkGetItemsWithVariantsInternalAsync(bulkGetRequest, continuationToken));
}
