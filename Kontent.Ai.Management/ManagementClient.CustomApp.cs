using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Models.CustomApps;
using Kontent.Ai.Management.Models.CustomApps.Patch;
using Kontent.Ai.Management.Models.Shared;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<CustomAppModel>> ListCustomAppsAsync()
    {
        var response = EnsureSuccess(await _managementApi.ListCustomAppsInternalAsync());

        return new ListingResponseModel<CustomAppModel>(
            (continuationToken, _) => GetNextCustomAppsPageAsync(continuationToken),
            response.Pagination?.Token,
            url: string.Empty,
            response.CustomApps);
    }

    private async Task<IListingResponse<CustomAppModel>> GetNextCustomAppsPageAsync(string continuationToken)
        => EnsureSuccess(await _managementApi.ListCustomAppsInternalAsync(continuationToken));

    /// <inheritdoc />
    public async Task<CustomAppModel> GetCustomAppAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return EnsureSuccess(await _managementApi.GetCustomAppInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id | ReferenceKinds.Codename)));
    }

    /// <inheritdoc />
    public async Task<CustomAppModel> CreateCustomAppAsync(CustomAppCreateModel customApp)
    {
        ArgumentNullException.ThrowIfNull(customApp);

        return EnsureSuccess(await _managementApi.CreateCustomAppInternalAsync(customApp));
    }

    /// <inheritdoc />
    public async Task DeleteCustomAppAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.DeleteCustomAppInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id | ReferenceKinds.Codename)));
    }

    /// <inheritdoc />
    public async Task<CustomAppModel> ModifyCustomAppAsync(Reference identifier, IEnumerable<CustomAppOperationBaseModel> changes)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(changes);

        return EnsureSuccess(await _managementApi.ModifyCustomAppInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id | ReferenceKinds.Codename), changes));
    }
}
