namespace Kontent.Ai.Management.Extensions;

/// <summary>
/// Drives continuation-token paging for the client's <c>ListXAsync</c> methods: walks every page, one HTTP request at
/// a time, and collects them into a single result. Paging is an implementation detail — callers get the whole listing.
/// The first failed page short-circuits and is returned as the failure, so a listing is all-or-nothing rather than a
/// partial set.
/// </summary>
internal static class PageEnumerator
{
    public static async Task<IManagementResult<IReadOnlyList<TItem>>> CollectAsync<TPage, TItem>(
        Func<string?, CancellationToken, Task<IApiResponse<TPage>>> fetchPage,
        Func<TPage, IEnumerable<TItem>> selectItems,
        Func<TPage, string?> selectContinuationToken,
        CancellationToken cancellationToken = default)
    {
        var items = new List<TItem>();
        IManagementResult<IReadOnlyList<TItem>>? lastPage = null;
        string? continuationToken = null;

        while (true)
        {
            var response = await fetchPage(continuationToken, cancellationToken).ConfigureAwait(false);
            var page = await response
                .ToManagementResultAsync<TPage, IReadOnlyList<TItem>>(content => selectItems(content).ToList())
                .ConfigureAwait(false);

            if (!page.IsSuccess)
            {
                return page;
            }

            items.AddRange(page.Value);
            lastPage = page;

            continuationToken = selectContinuationToken(response.Content!);
            if (continuationToken is null)
            {
                return ManagementResult<IReadOnlyList<TItem>>.Success(items, lastPage.StatusCode, lastPage.RequestUrl, lastPage.ResponseHeaders);
            }
        }
    }
}
