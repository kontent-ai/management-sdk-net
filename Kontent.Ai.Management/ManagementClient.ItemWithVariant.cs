using Kontent.Ai.Management.Models.ItemWithVariant;
using Kontent.Ai.Management.Models.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

public sealed partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<ItemWithVariantFilterResultModel>> FilterItemsWithVariantsAsync(ItemWithVariantFilterRequestModel filterRequest)
    {
        ArgumentNullException.ThrowIfNull(filterRequest);

        var endpointUrl = _urlBuilder.BuildItemsWithVariantFilterUrl();
        var response = await _actionInvoker.InvokeMethodAsync<ItemWithVariantFilterRequestModel, ItemWithVariantFilterListingResponseServerModel>(endpointUrl, HttpMethod.Post, filterRequest);

        return new ListingResponseModel<ItemWithVariantFilterResultModel>(
            (continuationToken, url) => GetNextFilterItemsWithVariantsPageAsync(continuationToken, url, filterRequest),
            response.Pagination?.Token,
            endpointUrl,
            response.Variants);
    }

    /// <inheritdoc />
    public async Task<IListingResponseModel<ContentItemWithVariantModel>> BulkGetItemsWithVariantsAsync(ItemWithVariantBulkGetRequestModel bulkGetRequest)
    {
        ArgumentNullException.ThrowIfNull(bulkGetRequest);

        var endpointUrl = _urlBuilder.BuildItemsWithVariantBulkGetUrl();
        var response = await _actionInvoker.InvokeMethodAsync<ItemWithVariantBulkGetRequestModel, ContentItemsWithVariantsListingResponseServerModel>(endpointUrl, HttpMethod.Post, bulkGetRequest);

        return new ListingResponseModel<ContentItemWithVariantModel>(
            (continuationToken, url) => GetNextBulkGetItemsWithVariantsPageAsync(continuationToken, url, bulkGetRequest),
            response.Pagination?.Token,
            endpointUrl,
            response.Data);
    }

    private async Task<IListingResponse<ItemWithVariantFilterResultModel>> GetNextFilterItemsWithVariantsPageAsync(string continuationToken, string url, ItemWithVariantFilterRequestModel filterRequest)
    {
        var headers = new Dictionary<string, string>
        {
            { "x-continuation", continuationToken }
        };

        var response = await _actionInvoker.InvokeMethodAsync<ItemWithVariantFilterRequestModel, ItemWithVariantFilterListingResponseServerModel>(url, HttpMethod.Post, filterRequest, headers);

        return response;
    }

    private async Task<IListingResponse<ContentItemWithVariantModel>> GetNextBulkGetItemsWithVariantsPageAsync(string continuationToken, string url, ItemWithVariantBulkGetRequestModel bulkGetRequest)
    {
        var headers = new Dictionary<string, string>
        {
            { "x-continuation", continuationToken }
        };

        var response = await _actionInvoker.InvokeMethodAsync<ItemWithVariantBulkGetRequestModel, ContentItemsWithVariantsListingResponseServerModel>(url, HttpMethod.Post, bulkGetRequest, headers);

        return response;
    }
}
