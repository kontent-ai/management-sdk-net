using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Models.Items;
using Kontent.Ai.Management.Models.Shared;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<ContentItemModel>> ListContentItemsAsync()
    {
        var response = EnsureSuccess(await _managementApi.ListContentItemsInternalAsync());

        return new ListingResponseModel<ContentItemModel>(
            (continuationToken, _) => GetNextContentItemsPageAsync(continuationToken),
            response.Pagination?.Token,
            url: string.Empty,
            response.Items);
    }

    private async Task<IListingResponse<ContentItemModel>> GetNextContentItemsPageAsync(string continuationToken)
        => EnsureSuccess(await _managementApi.ListContentItemsInternalAsync(continuationToken));

    /// <inheritdoc />
    public async Task<ContentItemModel> GetContentItemAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return EnsureSuccess(await _managementApi.GetContentItemInternalAsync(identifier.ToUrlSegment()));
    }

    /// <inheritdoc />
    public async Task<ContentItemModel> CreateContentItemAsync(ContentItemCreateModel contentItem)
    {
        ArgumentNullException.ThrowIfNull(contentItem);

        return EnsureSuccess(await _managementApi.CreateContentItemInternalAsync(contentItem));
    }

    /// <inheritdoc />
    public async Task<ContentItemModel> UpsertContentItemAsync(Reference identifier, ContentItemUpsertModel contentItem)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(contentItem);

        return EnsureSuccess(await _managementApi.UpsertContentItemInternalAsync(identifier.ToUrlSegment(), contentItem));
    }

    /// <inheritdoc />
    public async Task DeleteContentItemAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.DeleteContentItemInternalAsync(identifier.ToUrlSegment()));
    }
}
