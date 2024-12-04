using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.TypeSnippets;
using Kontent.Ai.Management.Models.TypeSnippets.Patch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<ContentTypeSnippetModel>> ListContentTypeSnippetsAsync()
    {
        var endpointUrl = _urlBuilder.BuildSnippetsUrl();
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<SnippetListingResponseServerModel>(endpointUrl, HttpMethod.Get);

        return new ListingResponseModel<ContentTypeSnippetModel>(
            GetNextListingPageAsync<SnippetListingResponseServerModel, ContentTypeSnippetModel>,
            response.Pagination?.Token,
            endpointUrl,
            response.Snippets);
    }

    /// <inheritdoc />
    public async Task<ContentTypeSnippetModel> GetContentTypeSnippetAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildSnippetsUrl(identifier);
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<ContentTypeSnippetModel>(endpointUrl, HttpMethod.Get);

        return response;
    }

    /// <inheritdoc />
    public async Task<ContentTypeSnippetModel> CreateContentTypeSnippetAsync(ContentTypeSnippetCreateModel contentTypeSnippet)
    {
        ArgumentNullException.ThrowIfNull(contentTypeSnippet);

        var endpointUrl = _urlBuilder.BuildSnippetsUrl();
        var response = await _actionInvoker.InvokeMethodAsync<ContentTypeSnippetCreateModel, ContentTypeSnippetModel>(endpointUrl, HttpMethod.Post, contentTypeSnippet);

        return response;
    }

    /// <inheritdoc />
    public async Task DeleteContentTypeSnippetAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildSnippetsUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
    }

    /// <inheritdoc />
    public async Task<ContentTypeSnippetModel> ModifyContentTypeSnippetAsync(Reference identifier, IEnumerable<ContentTypeSnippetOperationBaseModel> changes)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        if (changes == null || !changes.Any())
        {
            throw new ArgumentException("Please provide at least one operation.", nameof(changes));
        }

        var endpointUrl = _urlBuilder.BuildSnippetsUrl(identifier);
        return await _actionInvoker.InvokeMethodAsync<IEnumerable<ContentTypeSnippetOperationBaseModel>, ContentTypeSnippetModel>(endpointUrl, HttpMethod.Patch, changes);
    }
}
