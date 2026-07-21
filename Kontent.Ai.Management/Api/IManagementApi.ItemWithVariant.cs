using Kontent.Ai.Management.Models.ItemWithVariant;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Lists one page of content item variants matching a filter.</summary>
    /// <param name="filterRequest">The filter (re-sent for every page).</param>
    /// <param name="continuationToken">Continuation token from a previous page's response; <c>null</c> for the first page.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Post("/items-with-variant/filter")]
    internal Task<IApiResponse<ItemWithVariantFilterListingResponseServerModel>> FilterItemsWithVariantsInternalAsync(
        [Body] ItemWithVariantFilterRequestModel filterRequest,
        [Header("x-continuation")] string? continuationToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>Lists one page of content item variants for a set of variant identifiers.</summary>
    /// <param name="bulkGetRequest">The variant identifiers (re-sent for every page).</param>
    /// <param name="continuationToken">Continuation token from a previous page's response; <c>null</c> for the first page.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Post("/items-with-variant/bulk-get")]
    internal Task<IApiResponse<ContentItemsWithVariantsListingResponseServerModel>> BulkGetItemsWithVariantsInternalAsync(
        [Body] ItemWithVariantBulkGetRequestModel bulkGetRequest,
        [Header("x-continuation")] string? continuationToken = null,
        CancellationToken cancellationToken = default);
}
