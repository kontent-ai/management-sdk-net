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
    public Task<IManagementResult<ContentTypeSnippetModel>> GetContentTypeSnippetAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return _managementApi.GetContentTypeSnippetInternalAsync(identifier.ToUrlSegment(), cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public Task<IManagementResult<ContentTypeSnippetModel>> CreateContentTypeSnippetAsync(ContentTypeSnippetCreateModel contentTypeSnippet, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(contentTypeSnippet);

        return _managementApi.CreateContentTypeSnippetInternalAsync(contentTypeSnippet, cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public Task<IManagementResult> DeleteContentTypeSnippetAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return _managementApi.DeleteContentTypeSnippetInternalAsync(identifier.ToUrlSegment(), cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public Task<IManagementResult<ContentTypeSnippetModel>> ModifyContentTypeSnippetAsync(Reference identifier, IEnumerable<ContentModelOperationBaseModel> changes, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(changes);

        return _managementApi.ModifyContentTypeSnippetInternalAsync(identifier.ToUrlSegment(), changes, cancellationToken).ToManagementResultAsync();
    }
}
