using Kontent.Ai.Management.Models.ContentModel;

namespace Kontent.Ai.Management.Extensions;

/// <summary>
/// Content-model tooling built on top of <see cref="IManagementClient"/>.
/// </summary>
public static class ContentModelExtensions
{
    /// <summary>
    /// Exports the environment's entire content model — content types, snippets and taxonomy groups — into a single
    /// snapshot, draining the pagination of each listing.
    /// </summary>
    /// <remarks>
    /// The export is all-or-nothing: if any listing page fails, that failure is returned and no partial snapshot is
    /// produced — a content model silently missing entries is worse than none, since it would generate an incomplete
    /// model. Each collection is ordered by codename so a serialized snapshot diffs deterministically.
    /// </remarks>
    /// <param name="client">Content management client instance.</param>
    /// <param name="cancellationToken">Token to cancel the export.</param>
    /// <returns>A result wrapping the complete <see cref="ContentModelSnapshot"/>, or the first listing failure.</returns>
    public async static Task<IManagementResult<ContentModelSnapshot>> ExportContentModelAsync(this IManagementClient client, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(client);

        var (types, typesFailure) = await DrainAsync(client.EnumerateContentTypePagesAsync(cancellationToken), cancellationToken);
        if (typesFailure is not null)
        {
            return Project(typesFailure);
        }

        var (snippets, snippetsFailure) = await DrainAsync(client.EnumerateContentTypeSnippetPagesAsync(cancellationToken), cancellationToken);
        if (snippetsFailure is not null)
        {
            return Project(snippetsFailure);
        }

        var (taxonomies, taxonomiesFailure) = await DrainAsync(client.EnumerateTaxonomyGroupPagesAsync(cancellationToken), cancellationToken);
        if (taxonomiesFailure is not null)
        {
            return Project(taxonomiesFailure);
        }

        var snapshot = new ContentModelSnapshot
        {
            Types = [.. types.OrderBy(t => t.Codename, StringComparer.Ordinal)],
            Snippets = [.. snippets.OrderBy(s => s.Codename, StringComparer.Ordinal)],
            Taxonomies = [.. taxonomies.OrderBy(t => t.Codename, StringComparer.Ordinal)],
        };

        return ManagementResult<ContentModelSnapshot>.Success(snapshot);
    }

    private static ManagementResult<ContentModelSnapshot> Project(IManagementResult failure) =>
        ManagementResult<ContentModelSnapshot>.Failure(failure.Error!, failure.StatusCode, failure.RequestUrl, failure.ResponseHeaders);

    // Drains a page stream into a flat list, or returns the first failed page so the caller can project it. Unlike
    // ListingExtensions.Items, a failed page does not throw — the export stays within the result pattern.
    private static async Task<(List<T> Items, IManagementResult? Failure)> DrainAsync<T>(
        IAsyncEnumerable<IManagementResult<IReadOnlyList<T>>> pages,
        CancellationToken cancellationToken)
    {
        var items = new List<T>();

        await foreach (var page in pages.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (!page.IsSuccess)
            {
                return (items, page);
            }

            items.AddRange(page.Value);
        }

        return (items, null);
    }
}
