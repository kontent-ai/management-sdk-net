using Kontent.Ai.Management.Models.Languages;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.TaxonomyGroups;
using System.Runtime.CompilerServices;

namespace Kontent.Ai.Management.Extensions;

/// <summary>
/// Extension methods related to listing endpoints.
/// </summary>
public static class ListingExtensions
{
    /// <summary>
    /// This is extension method for listing methods from <see cref="IManagementClient"/>.
    /// It goes page by page until it gets all items for given entity.
    /// </summary>
    /// <typeparam name="T">Entity model e.g <see cref="LanguageModel"/>, <see cref="TaxonomyGroupModel"/> and etc.</typeparam>
    /// <param name="method">To be extended method that returns <see cref="Task{IListingResponseModel}"/></param>
    /// <returns><see cref="List{T}"/> all items for environment</returns>
    public async static Task<List<T>> GetAllAsync<T>(this Task<IListingResponseModel<T>> method)
    {
        var result = new List<T>();
        var response = await method;

        while (true)
        {
            result.AddRange(response);

            if (!response.HasNextPage())
            {
                break;
            }

            response = await response.GetNextPage();
        }

        return result;
    }

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
