using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.TypeSnippets;
using Kentico.Kontent.Management.Models.TypeSnippets.Patch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kentico.Kontent.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<ContentTypeSnippetModel>> ListContentTypeSnippetsAsync()
    {
        var endpointUrl = _urlBuilder.BuildSnippetsUrl();
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<SnippetListingResponseServerModel>(endpointUrl, HttpMethod.Get);

        return new ListingResponseModel<ContentTypeSnippetModel>(
            (token, url) => GetNextListingPageAsync<SnippetListingResponseServerModel, ContentTypeSnippetModel>(token, url),
            response.Pagination?.Token,
            endpointUrl,
            response.Snippets);
    }

    /// <inheritdoc />
    public async Task<ContentTypeSnippetModel> GetContentTypeSnippetAsync(Reference identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildSnippetsUrl(identifier);
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<ContentTypeSnippetModel>(endpointUrl, HttpMethod.Get);

        return response;
    }

    /// <inheritdoc />
    public async Task<ContentTypeSnippetModel> CreateContentTypeSnippetAsync(ContentTypeSnippetCreateModel contentTypeSnippet)
    {
        if (contentTypeSnippet == null)
        {
            throw new ArgumentNullException(nameof(contentTypeSnippet));
        }

        var endpointUrl = _urlBuilder.BuildSnippetsUrl();
        var response = await _actionInvoker.InvokeMethodAsync<ContentTypeSnippetCreateModel, ContentTypeSnippetModel>(endpointUrl, HttpMethod.Post, contentTypeSnippet);

        return response;
    }

    /// <inheritdoc />
    public async Task DeleteContentTypeSnippetAsync(Reference identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildSnippetsUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
    }

    /// <inheritdoc />
    public async Task<ContentTypeSnippetModel> ModifyContentTypeSnippetAsync(Reference identifier, IEnumerable<ContentTypeSnippetOperationBaseModel> changes)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        if (changes == null || !changes.Any())
        {
            throw new ArgumentException("Please provide at least one operation.", nameof(changes));
        }

        var endpointUrl = _urlBuilder.BuildSnippetsUrl(identifier);
        return await _actionInvoker.InvokeMethodAsync<IEnumerable<ContentTypeSnippetOperationBaseModel>, ContentTypeSnippetModel>(endpointUrl, new HttpMethod("PATCH"), changes);
    }
}
