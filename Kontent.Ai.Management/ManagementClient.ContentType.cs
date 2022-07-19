using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management.Models.Types.Patch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<ContentTypeModel>> ListContentTypesAsync()
    {
        var endpointUrl = _urlBuilder.BuildTypeUrl();
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<ContentTypeListingResponseServerModel>(endpointUrl, HttpMethod.Get);

        return new ListingResponseModel<ContentTypeModel>(
            (token, url) => GetNextListingPageAsync<ContentTypeListingResponseServerModel, ContentTypeModel>(token, url),
            response.Pagination?.Token,
            endpointUrl,
            response.Types);
    }

    /// <inheritdoc />
    public async Task<ContentTypeModel> GetContentTypeAsync(Reference identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildTypeUrl(identifier);
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<ContentTypeModel>(endpointUrl, HttpMethod.Get);

        return response;
    }

    /// <inheritdoc />
    public async Task<ContentTypeModel> CreateContentTypeAsync(ContentTypeCreateModel contentType)
    {
        if (contentType == null)
        {
            throw new ArgumentNullException(nameof(contentType));
        }

        var endpointUrl = _urlBuilder.BuildTypeUrl();
        var response = await _actionInvoker.InvokeMethodAsync<ContentTypeCreateModel, ContentTypeModel>(endpointUrl, HttpMethod.Post, contentType);

        return response;
    }

    /// <inheritdoc />
    public async Task DeleteContentTypeAsync(Reference identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildTypeUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
    }

    /// <inheritdoc />
    public async Task<ContentTypeModel> ModifyContentTypeAsync(Reference identifier, IEnumerable<ContentTypeOperationBaseModel> changes)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        if (changes == null || !changes.Any())
        {
            throw new ArgumentException("Please provide at least one operation.", nameof(changes));
        }

        var endpointUrl = _urlBuilder.BuildTypeUrl(identifier);
        return await _actionInvoker.InvokeMethodAsync<IEnumerable<ContentTypeOperationBaseModel>, ContentTypeModel>(endpointUrl, new HttpMethod("PATCH"), changes);
    }
}
