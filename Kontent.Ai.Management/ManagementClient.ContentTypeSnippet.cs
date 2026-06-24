using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.ContentModel.Patch;
using Kontent.Ai.Management.Models.TypeSnippets;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public Task<IManagementResult<IReadOnlyList<ContentTypeSnippetModel>>> ListContentTypeSnippetsAsync(CancellationToken cancellationToken = default)
        => PageEnumerator.CollectAsync<SnippetListingResponseServerModel, ContentTypeSnippetModel>(
            _managementApi.ListContentTypeSnippetsInternalAsync,
            page => page.Snippets,
            page => page.Pagination?.Token,
            cancellationToken);

    /// <inheritdoc />
    public async Task<IManagementResult<ContentTypeSnippetModel>> GetContentTypeSnippetAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.GetContentTypeSnippetInternalAsync(identifier.ToUrlSegment(), cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<ContentTypeSnippetModel>> CreateContentTypeSnippetAsync(ContentTypeSnippetCreateModel contentTypeSnippet, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(contentTypeSnippet);

        var response = await _managementApi.CreateContentTypeSnippetInternalAsync(contentTypeSnippet, cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult> DeleteContentTypeSnippetAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.DeleteContentTypeSnippetInternalAsync(identifier.ToUrlSegment(), cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<ContentTypeSnippetModel>> ModifyContentTypeSnippetAsync(Reference identifier, IEnumerable<ContentModelOperationBaseModel> changes, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(changes);

        var response = await _managementApi.ModifyContentTypeSnippetInternalAsync(identifier.ToUrlSegment(), changes, cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }
}
