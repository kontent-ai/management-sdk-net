using System.Runtime.CompilerServices;

namespace Kontent.Ai.Management.Extensions;

/// <summary>
/// Drives continuation-token paging for the client's listing methods: walks pages one HTTP request at a time. Most
/// endpoints expose only the materialized form (<see cref="CollectAsync"/>); the large ones additionally expose the
/// page stream (<see cref="EnumerateAsync"/>) so callers can process huge listings without buffering them.
/// </summary>
internal static class PageEnumerator
{
    /// <summary>
    /// Streams pages lazily — one HTTP request per iteration. A failed page is yielded as a failed result and ends
    /// the stream; the next page is fetched only when the consumer pulls past the current one.
    /// </summary>
    public static async IAsyncEnumerable<IManagementResult<IReadOnlyList<TItem>>> EnumerateAsync<TPage, TItem>(
        Func<string?, CancellationToken, Task<IApiResponse<TPage>>> fetchPage,
        Func<TPage, IEnumerable<TItem>> selectItems,
        Func<TPage, string?> selectContinuationToken,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string? continuationToken = null;

        while (true)
        {
            var response = await fetchPage(continuationToken, cancellationToken).ConfigureAwait(false);
            var page = await response
                .ToManagementResultAsync<TPage, IReadOnlyList<TItem>>(content => selectItems(content).ToList())
                .ConfigureAwait(false);

            yield return page;

            if (!page.IsSuccess)
            {
                yield break;
            }

            continuationToken = selectContinuationToken(response.Content!);
            if (continuationToken is null)
            {
                yield break;
            }
        }
    }

    /// <summary>
    /// Drains every page into a single result. The first failed page short-circuits and is returned as the failure,
    /// so a listing is all-or-nothing rather than a partial set.
    /// </summary>
    public static async Task<IManagementResult<IReadOnlyList<TItem>>> CollectAsync<TPage, TItem>(
        Func<string?, CancellationToken, Task<IApiResponse<TPage>>> fetchPage,
        Func<TPage, IEnumerable<TItem>> selectItems,
        Func<TPage, string?> selectContinuationToken,
        CancellationToken cancellationToken = default)
    {
        var items = new List<TItem>();
        IManagementResult<IReadOnlyList<TItem>>? lastPage = null;

        await foreach (var page in EnumerateAsync(fetchPage, selectItems, selectContinuationToken, cancellationToken).ConfigureAwait(false))
        {
            if (!page.IsSuccess)
            {
                return page;
            }

            items.AddRange(page.Value);
            lastPage = page;
        }

        return ManagementResult<IReadOnlyList<TItem>>.Success(items, lastPage?.StatusCode, lastPage?.RequestUrl);
    }
}
