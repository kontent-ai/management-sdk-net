using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.TaxonomyGroups;
using Kontent.Ai.Management.Models.TaxonomyGroups.Patch;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public Task<IManagementResult<IReadOnlyList<TaxonomyGroupModel>>> ListTaxonomyGroupsAsync(CancellationToken cancellationToken = default)
        => PageEnumerator.CollectAsync<TaxonomyGroupListingResponseServerModel, TaxonomyGroupModel>(
            _managementApi.ListTaxonomyGroupsInternalAsync,
            page => page.Taxonomies,
            page => page.Pagination?.Token,
            cancellationToken);

    /// <inheritdoc />
    public async Task<IManagementResult<TaxonomyGroupModel>> GetTaxonomyGroupAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.GetTaxonomyGroupInternalAsync(identifier.ToUrlSegment(), cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<TaxonomyGroupModel>> CreateTaxonomyGroupAsync(TaxonomyGroupCreateModel taxonomyGroup, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(taxonomyGroup);

        var response = await _managementApi.CreateTaxonomyGroupInternalAsync(taxonomyGroup, cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<TaxonomyGroupModel>> ModifyTaxonomyGroupAsync(Reference identifier, IEnumerable<TaxonomyGroupOperationBaseModel> changes, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(changes);

        var response = await _managementApi.ModifyTaxonomyGroupInternalAsync(identifier.ToUrlSegment(), changes, cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult> DeleteTaxonomyGroupAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.DeleteTaxonomyGroupInternalAsync(identifier.ToUrlSegment(), cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }
}
