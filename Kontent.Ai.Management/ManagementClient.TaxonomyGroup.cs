using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.TaxonomyGroups;
using Kontent.Ai.Management.Models.TaxonomyGroups.Patch;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public IAsyncEnumerable<IManagementResult<IReadOnlyList<TaxonomyGroupModel>>> EnumerateTaxonomyGroupPagesAsync(CancellationToken cancellationToken = default)
        => PageEnumerator.EnumerateAsync<TaxonomyGroupListingResponseServerModel, TaxonomyGroupModel>(
            _managementApi.ListTaxonomyGroupsInternalAsync,
            page => page.Taxonomies,
            page => page.Pagination?.Token,
            cancellationToken);

    /// <inheritdoc />
    public async Task<IManagementResult<TaxonomyGroupModel>> GetTaxonomyGroupAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.GetTaxonomyGroupInternalAsync(identifier.ToUrlSegment(), cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<TaxonomyGroupModel>> CreateTaxonomyGroupAsync(TaxonomyGroupCreateModel taxonomyGroup, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(taxonomyGroup);

        var response = await _managementApi.CreateTaxonomyGroupInternalAsync(taxonomyGroup, cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<TaxonomyGroupModel>> ModifyTaxonomyGroupAsync(Reference identifier, IEnumerable<TaxonomyGroupOperationBaseModel> changes, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.ModifyTaxonomyGroupInternalAsync(identifier.ToUrlSegment(), changes, cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult> DeleteTaxonomyGroupAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.DeleteTaxonomyGroupInternalAsync(identifier.ToUrlSegment(), cancellationToken);
        return await response.ToManagementResultAsync();
    }
}
