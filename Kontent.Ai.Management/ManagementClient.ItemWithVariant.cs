using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.ItemWithVariant;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public Task<IManagementResult<IReadOnlyList<ItemWithVariantFilterResultModel>>> ListItemsWithVariantsByFilterAsync(ItemWithVariantFilterRequestModel filterRequest, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filterRequest);

        return PageEnumerator.CollectAsync<ItemWithVariantFilterListingResponseServerModel, ItemWithVariantFilterResultModel>(
            (token, ct) => _managementApi.FilterItemsWithVariantsInternalAsync(filterRequest, token, ct),
            page => page.Variants,
            page => page.Pagination?.Token,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<IManagementResult<IReadOnlyList<ContentItemWithVariantModel>>> ListItemsWithVariantsByBulkGetAsync(ItemWithVariantBulkGetRequestModel bulkGetRequest, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(bulkGetRequest);

        return PageEnumerator.CollectAsync<ContentItemsWithVariantsListingResponseServerModel, ContentItemWithVariantModel>(
            (token, ct) => _managementApi.BulkGetItemsWithVariantsInternalAsync(bulkGetRequest, token, ct),
            page => page.Data,
            page => page.Pagination?.Token,
            cancellationToken);
    }
}
