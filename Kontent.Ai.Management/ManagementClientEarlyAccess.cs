using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.VariantFilter;
using Kontent.Ai.Management.Modules.ActionInvoker;
using Kontent.Ai.Management.Modules.UrlBuilder;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

/// <summary>
/// Provides access to early access Content Management API features.
/// These features are experimental and may change or be removed in future versions.
/// </summary>
public sealed class ManagementClientEarlyAccess : IManagementClientEarlyAccess
{
    private readonly ActionInvoker _actionInvoker;
    private readonly EndpointUrlBuilder _urlBuilder;

    internal ManagementClientEarlyAccess(ActionInvoker actionInvoker, EndpointUrlBuilder urlBuilder)
    {
        _actionInvoker = actionInvoker ?? throw new ArgumentNullException(nameof(actionInvoker));
        _urlBuilder = urlBuilder ?? throw new ArgumentNullException(nameof(urlBuilder));
    }

    /// <inheritdoc />
    public async Task<IListingResponseModel<VariantFilterItemModel>> FilterVariantsAsync(VariantFilterRequestModel variantFilterRequest)
    {
        ArgumentNullException.ThrowIfNull(variantFilterRequest);

        var endpointUrl = _urlBuilder.BuildVariantFilterUrl();
        var response = await _actionInvoker.InvokeMethodAsync<VariantFilterRequestModel, VariantFilterListingResponseServerModel>(endpointUrl, HttpMethod.Post, variantFilterRequest);

        return new ListingResponseModel<VariantFilterItemModel>(
            GetNextListingPageAsync<VariantFilterListingResponseServerModel, VariantFilterItemModel>,
            response.Pagination?.Token,
            endpointUrl,
            response.Data);
    }

    private async Task<IListingResponse<TModel>> GetNextListingPageAsync<TListingResponse, TModel>(string continuationToken, string url)
        where TListingResponse : IListingResponse<TModel>
    {
        var headers = new Dictionary<string, string>
        {
            { "x-continuation", continuationToken }
        };
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<TListingResponse>(url, HttpMethod.Get, headers);

        return response;
    }
}