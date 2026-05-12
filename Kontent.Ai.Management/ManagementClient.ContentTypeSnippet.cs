using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.TypeSnippets;
using Kontent.Ai.Management.Models.TypeSnippets.Patch;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<ContentTypeSnippetModel>> ListContentTypeSnippetsAsync()
    {
        var response = EnsureSuccess(await _managementApi.ListContentTypeSnippetsInternalAsync());

        return new ListingResponseModel<ContentTypeSnippetModel>(
            (continuationToken, _) => GetNextContentTypeSnippetsPageAsync(continuationToken),
            response.Pagination?.Token,
            url: string.Empty,
            response.Snippets);
    }

    private async Task<IListingResponse<ContentTypeSnippetModel>> GetNextContentTypeSnippetsPageAsync(string continuationToken)
        => EnsureSuccess(await _managementApi.ListContentTypeSnippetsInternalAsync(continuationToken));

    /// <inheritdoc />
    public async Task<ContentTypeSnippetModel> GetContentTypeSnippetAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return EnsureSuccess(await _managementApi.GetContentTypeSnippetInternalAsync(identifier.ToUrlSegment()));
    }

    /// <inheritdoc />
    public async Task<ContentTypeSnippetModel> CreateContentTypeSnippetAsync(ContentTypeSnippetCreateModel contentTypeSnippet)
    {
        ArgumentNullException.ThrowIfNull(contentTypeSnippet);

        return EnsureSuccess(await _managementApi.CreateContentTypeSnippetInternalAsync(contentTypeSnippet));
    }

    /// <inheritdoc />
    public async Task DeleteContentTypeSnippetAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.DeleteContentTypeSnippetInternalAsync(identifier.ToUrlSegment()));
    }

    /// <inheritdoc />
    public async Task<ContentTypeSnippetModel> ModifyContentTypeSnippetAsync(Reference identifier, IEnumerable<ContentTypeSnippetOperationBaseModel> changes)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        if (changes == null || !changes.Any())
        {
            throw new ArgumentException("Please provide at least one operation.", nameof(changes));
        }

        return EnsureSuccess(await _managementApi.ModifyContentTypeSnippetInternalAsync(identifier.ToUrlSegment(), changes));
    }
}
