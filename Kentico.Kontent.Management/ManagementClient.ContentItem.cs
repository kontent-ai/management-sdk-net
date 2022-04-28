using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.Shared;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kentico.Kontent.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<ContentItemModel>> ListContentItemsAsync()
    {
        var endpointUrl = _urlBuilder.BuildItemsUrl();
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<ContentItemListingResponseServerModel>(endpointUrl, HttpMethod.Get);

        return new ListingResponseModel<ContentItemModel>(
            (token, url) => GetNextListingPageAsync<ContentItemListingResponseServerModel, ContentItemModel>(token, url),
            response.Pagination?.Token,
            endpointUrl,
            response.Items);
    }

    /// <inheritdoc />
    public async Task<ContentItemModel> GetContentItemAsync(Reference identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildItemUrl(identifier);
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<ContentItemModel>(endpointUrl, HttpMethod.Get);

        return response;
    }

    /// <inheritdoc />
    public async Task<ContentItemModel> CreateContentItemAsync(ContentItemCreateModel contentItem)
    {
        if (contentItem == null)
        {
            throw new ArgumentNullException(nameof(contentItem));
        }

        var endpointUrl = _urlBuilder.BuildItemsUrl();
        var response = await _actionInvoker.InvokeMethodAsync<ContentItemCreateModel, ContentItemModel>(endpointUrl, HttpMethod.Post, contentItem);

        return response;
    }

    /// <inheritdoc />
    public async Task<ContentItemModel> UpsertContentItemAsync(Reference identifier, ContentItemUpsertModel contentItem)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        if (contentItem == null)
        {
            throw new ArgumentNullException(nameof(contentItem));
        }

        var endpointUrl = _urlBuilder.BuildItemUrl(identifier);
        var response = await _actionInvoker.InvokeMethodAsync<ContentItemUpsertModel, ContentItemModel>(endpointUrl, HttpMethod.Put, contentItem);

        return response;
    }

    /// <inheritdoc />
    public async Task DeleteContentItemAsync(Reference identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildItemUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
    }
}
