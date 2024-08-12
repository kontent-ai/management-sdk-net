using Kontent.Ai.Management.Models.Items;
using Kontent.Ai.Management.Models.Shared;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<ContentItemModel>> ListContentItemsAsync()
    {
        var endpointUrl = _urlBuilder.BuildItemsUrl();
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<ContentItemListingResponseServerModel>(endpointUrl, HttpMethod.Get);

        return new ListingResponseModel<ContentItemModel>(
            GetNextListingPageAsync<ContentItemListingResponseServerModel, ContentItemModel>,
            response.Pagination?.Token,
            endpointUrl,
            response.Items);
    }

    /// <inheritdoc />
    public async Task<ContentItemModel> GetContentItemAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildItemUrl(identifier);
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<ContentItemModel>(endpointUrl, HttpMethod.Get);

        return response;
    }

    /// <inheritdoc />
    public async Task<ContentItemModel> CreateContentItemAsync(ContentItemCreateModel contentItem)
    {
        ArgumentNullException.ThrowIfNull(contentItem);

        var endpointUrl = _urlBuilder.BuildItemsUrl();
        var response = await _actionInvoker.InvokeMethodAsync<ContentItemCreateModel, ContentItemModel>(endpointUrl, HttpMethod.Post, contentItem);

        return response;
    }

    /// <inheritdoc />
    public async Task<ContentItemModel> UpsertContentItemAsync(Reference identifier, ContentItemUpsertModel contentItem)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        ArgumentNullException.ThrowIfNull(contentItem);

        var endpointUrl = _urlBuilder.BuildItemUrl(identifier);
        var response = await _actionInvoker.InvokeMethodAsync<ContentItemUpsertModel, ContentItemModel>(endpointUrl, HttpMethod.Put, contentItem);

        return response;
    }

    /// <inheritdoc />
    public async Task DeleteContentItemAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildItemUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
    }
}
