using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.TypeSnippets;
using Kontent.Ai.Management.Models.TypeSnippets.Patch;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public IAsyncEnumerable<IManagementResult<IReadOnlyList<ContentTypeSnippetModel>>> EnumerateContentTypeSnippetPagesAsync(CancellationToken cancellationToken = default)
        => PageEnumerator.EnumerateAsync<SnippetListingResponseServerModel, ContentTypeSnippetModel>(
            _managementApi.ListContentTypeSnippetsInternalAsync,
            page => page.Snippets,
            page => page.Pagination?.Token,
            cancellationToken);

    /// <inheritdoc />
    public async Task<IManagementResult<ContentTypeSnippetModel>> GetContentTypeSnippetAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.GetContentTypeSnippetInternalAsync(identifier.ToUrlSegment(), cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<ContentTypeSnippetModel>> CreateContentTypeSnippetAsync(ContentTypeSnippetCreateModel contentTypeSnippet, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(contentTypeSnippet);

        var response = await _managementApi.CreateContentTypeSnippetInternalAsync(contentTypeSnippet, cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult> DeleteContentTypeSnippetAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.DeleteContentTypeSnippetInternalAsync(identifier.ToUrlSegment(), cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<ContentTypeSnippetModel>> ModifyContentTypeSnippetAsync(Reference identifier, IEnumerable<ContentTypeSnippetOperationBaseModel> changes, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(changes);

        var response = await _managementApi.ModifyContentTypeSnippetInternalAsync(identifier.ToUrlSegment(), changes, cancellationToken);
        return await response.ToManagementResultAsync();
    }
}
