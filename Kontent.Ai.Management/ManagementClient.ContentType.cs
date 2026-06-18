using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management.Models.Types.Patch;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public Task<IManagementResult<IReadOnlyList<ContentTypeModel>>> ListContentTypesAsync(CancellationToken cancellationToken = default)
        => PageEnumerator.CollectAsync<ContentTypeListingResponseServerModel, ContentTypeModel>(
            _managementApi.ListContentTypesInternalAsync,
            page => page.Types,
            page => page.Pagination?.Token,
            cancellationToken);

    /// <inheritdoc />
    public async Task<IManagementResult<ContentTypeModel>> GetContentTypeAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.GetContentTypeInternalAsync(identifier.ToUrlSegment(), cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<ContentTypeModel>> CreateContentTypeAsync(ContentTypeCreateModel contentType, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(contentType);

        var response = await _managementApi.CreateContentTypeInternalAsync(contentType, cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult> DeleteContentTypeAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.DeleteContentTypeInternalAsync(identifier.ToUrlSegment(), cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<ContentTypeModel>> ModifyContentTypeAsync(Reference identifier, IEnumerable<ContentTypeOperationBaseModel> changes, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(changes);

        var response = await _managementApi.ModifyContentTypeInternalAsync(identifier.ToUrlSegment(), changes, cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }
}
