using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Items;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public Task<IManagementResult<IReadOnlyList<ContentItemModel>>> ListContentItemsAsync(CancellationToken cancellationToken = default)
        => PageEnumerator.CollectAsync<ContentItemListingResponseServerModel, ContentItemModel>(
            _managementApi.ListContentItemsInternalAsync,
            page => page.Items,
            page => page.Pagination?.Token,
            cancellationToken);

    /// <inheritdoc />
    public IAsyncEnumerable<IManagementResult<IReadOnlyList<ContentItemModel>>> EnumerateContentItemPagesAsync(CancellationToken cancellationToken = default)
        => PageEnumerator.EnumerateAsync<ContentItemListingResponseServerModel, ContentItemModel>(
            _managementApi.ListContentItemsInternalAsync,
            page => page.Items,
            page => page.Pagination?.Token,
            cancellationToken);

    /// <inheritdoc />
    public async Task<IManagementResult<ContentItemModel>> GetContentItemAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.GetContentItemInternalAsync(identifier.ToUrlSegment(), cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<ContentItemModel>> CreateContentItemAsync(ContentItemCreateModel contentItem, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(contentItem);

        var response = await _managementApi.CreateContentItemInternalAsync(contentItem, cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<ContentItemModel>> UpsertContentItemAsync(Reference identifier, ContentItemUpsertModel contentItem, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(contentItem);

        var response = await _managementApi.UpsertContentItemInternalAsync(identifier.ToUrlSegment(), contentItem, cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult> DeleteContentItemAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.DeleteContentItemInternalAsync(identifier.ToUrlSegment(), cancellationToken);
        return await response.ToManagementResultAsync();
    }
}
