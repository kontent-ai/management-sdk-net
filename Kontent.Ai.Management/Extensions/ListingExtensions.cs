using Kontent.Ai.Management.Models.Languages;
using Kontent.Ai.Management.Models.TaxonomyGroups;
using System.Runtime.CompilerServices;

namespace Kontent.Ai.Management.Extensions;

/// <summary>
/// Extension methods related to listing endpoints.
/// </summary>
public static class ListingExtensions
{
    /// <summary>
    /// Flattens a page stream from an <c>EnumerateXPagesAsync</c> method into a flat stream of items.
    /// </summary>
    /// <remarks>
    /// The caller opts out of the per-page result channel by using this extension, so a failed page has nowhere to
    /// surface as data: it is thrown as a <see cref="ManagementResultException"/>. Consume the page stream directly
    /// when a mid-enumeration failure must be handled without an exception.
    /// </remarks>
    /// <typeparam name="T">The item type, e.g. <see cref="LanguageModel"/> or <see cref="TaxonomyGroupModel"/>.</typeparam>
    /// <param name="pages">A page stream produced by an <c>EnumerateXPagesAsync</c> method.</param>
    /// <param name="cancellationToken">Token to cancel the enumeration.</param>
    /// <returns>An async stream of every item across all pages.</returns>
    /// <exception cref="ManagementResultException">A page failed to load.</exception>
    public static IAsyncEnumerable<T> Items<T>(
        this IAsyncEnumerable<IManagementResult<IReadOnlyList<T>>> pages,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(pages);
        return Iterator(pages, cancellationToken);

        static async IAsyncEnumerable<T> Iterator(
            IAsyncEnumerable<IManagementResult<IReadOnlyList<T>>> pages,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var page in pages.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                if (!page.IsSuccess)
                {
                    throw new ManagementResultException(page.Error!, page.StatusCode);
                }

                foreach (var item in page.Value)
                {
                    yield return item;
                }
            }
        }
    }
}
