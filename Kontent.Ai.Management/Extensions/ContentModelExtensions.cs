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

        var types = await client.ListContentTypesAsync(cancellationToken).ConfigureAwait(false);
        if (!types.IsSuccess)
        {
            return Project(types);
        }

        var snippets = await client.ListContentTypeSnippetsAsync(cancellationToken).ConfigureAwait(false);
        if (!snippets.IsSuccess)
        {
            return Project(snippets);
        }

        var taxonomies = await client.ListTaxonomyGroupsAsync(cancellationToken).ConfigureAwait(false);
        if (!taxonomies.IsSuccess)
        {
            return Project(taxonomies);
        }

        var snapshot = new ContentModelSnapshot
        {
            Types = [.. types.Value.OrderBy(t => t.Codename, StringComparer.Ordinal)],
            Snippets = [.. snippets.Value.OrderBy(s => s.Codename, StringComparer.Ordinal)],
            Taxonomies = [.. taxonomies.Value.OrderBy(t => t.Codename, StringComparer.Ordinal)],
        };

        return ManagementResult<ContentModelSnapshot>.Success(snapshot);
    }

    private static ManagementResult<ContentModelSnapshot> Project(IManagementResult failure) =>
        ManagementResult<ContentModelSnapshot>.Failure(failure.Error!, failure.StatusCode, failure.RequestUrl);
}
