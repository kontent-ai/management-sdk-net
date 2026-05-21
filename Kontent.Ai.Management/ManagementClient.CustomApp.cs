using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.CustomApps;
using Kontent.Ai.Management.Models.CustomApps.Patch;
using Kontent.Ai.Management.Models.Shared;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IManagementResult<IListingResponseModel<CustomAppModel>>> ListCustomAppsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.ListCustomAppsInternalAsync(cancellationToken: cancellationToken);
        return await response.ToManagementResultAsync(BuildCustomAppsPage);
    }

    private IListingResponseModel<CustomAppModel> BuildCustomAppsPage(CustomAppListingResponseServerModel payload)
        => new ListingResponseModel<CustomAppModel>(
            (continuationToken, _) => GetNextCustomAppsPageAsync(continuationToken),
            payload.Pagination?.Token,
            url: string.Empty,
            payload.CustomApps);

    private async Task<IListingResponse<CustomAppModel>> GetNextCustomAppsPageAsync(string continuationToken)
        => EnsureSuccess(await _managementApi.ListCustomAppsInternalAsync(continuationToken));

    /// <inheritdoc />
    public async Task<IManagementResult<CustomAppModel>> GetCustomAppAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.GetCustomAppInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id | ReferenceKinds.Codename), cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<CustomAppModel>> CreateCustomAppAsync(CustomAppCreateModel customApp, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(customApp);

        var response = await _managementApi.CreateCustomAppInternalAsync(customApp, cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult> DeleteCustomAppAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.DeleteCustomAppInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id | ReferenceKinds.Codename), cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<CustomAppModel>> ModifyCustomAppAsync(Reference identifier, IEnumerable<CustomAppOperationBaseModel> changes, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(changes);

        var response = await _managementApi.ModifyCustomAppInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id | ReferenceKinds.Codename), changes, cancellationToken);
        return await response.ToManagementResultAsync();
    }
}
