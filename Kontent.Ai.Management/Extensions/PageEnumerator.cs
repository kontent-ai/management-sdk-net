using System.Runtime.CompilerServices;
using Refit;

namespace Kontent.Ai.Management.Extensions;

/// <summary>
/// Drives continuation-token paging for the <c>EnumerateXPagesAsync</c> client methods: walks pages lazily, one
/// HTTP request at a time, and yields each as an <see cref="IManagementResult{T}"/>. A failed page is yielded as a
/// failed result and ends the stream; the next page is fetched only when the consumer pulls past the current one.
/// </summary>
internal static class PageEnumerator
{
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
            var result = await response
                .ToManagementResultAsync<TPage, IReadOnlyList<TItem>>(page => selectItems(page).ToList())
                .ConfigureAwait(false);

            yield return result;

            if (!result.IsSuccess)
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
}
