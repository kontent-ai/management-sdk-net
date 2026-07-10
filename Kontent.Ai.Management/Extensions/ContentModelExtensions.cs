using Kontent.Ai.Management.Models.ContentModel;
using System.Diagnostics.CodeAnalysis;
using System.Net;

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
    /// <para>
    /// <b>Experimental.</b> The snapshot feature is not yet a supported contract and may change or be removed in a
    /// future release. Suppress the <c>KAIM001</c> diagnostic to opt in.
    /// </para>
    /// <para>
    /// The export is all-or-nothing: if any listing page fails, that failure is returned and no partial snapshot is
    /// produced — a content model silently missing entries is worse than none. Each collection is ordered by codename
    /// so a serialized snapshot diffs deterministically.
    /// </para>
    /// </remarks>
    /// <param name="client">Content management client instance.</param>
    /// <param name="cancellationToken">Token to cancel the export.</param>
    /// <returns>A result wrapping the complete <see cref="ContentModelSnapshot"/>, or the first listing failure.</returns>
    [Experimental("KAIM001")]
    public static async Task<IManagementResult<ContentModelSnapshot>> ExportContentModelAsync(this IManagementClient client, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(client);

        var types = await client.ListContentTypesAsync(cancellationToken).ConfigureAwait(false);
        if (!types.IsSuccess)
        {
            return types.AsFailure<ContentModelSnapshot>();
        }

        var snippets = await client.ListContentTypeSnippetsAsync(cancellationToken).ConfigureAwait(false);
        if (!snippets.IsSuccess)
        {
            return snippets.AsFailure<ContentModelSnapshot>();
        }

        var taxonomies = await client.ListTaxonomyGroupsAsync(cancellationToken).ConfigureAwait(false);
        if (!taxonomies.IsSuccess)
        {
            return taxonomies.AsFailure<ContentModelSnapshot>();
        }

        var snapshot = new ContentModelSnapshot
        {
            Types = [.. types.Value.OrderBy(t => t.Codename, StringComparer.Ordinal)],
            Snippets = [.. snippets.Value.OrderBy(s => s.Codename, StringComparer.Ordinal)],
            Taxonomies = [.. taxonomies.Value.OrderBy(t => t.Codename, StringComparer.Ordinal)],
        };

        // Aggregates several successful calls into one snapshot, so report a synthetic success status.
        return ManagementResult<ContentModelSnapshot>.Success(snapshot, HttpStatusCode.OK);
    }
}
