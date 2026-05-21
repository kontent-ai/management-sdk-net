using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Roles;
using Kontent.Ai.Management.Models.Shared;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IManagementResult<EnvironmentRolesModel>> ListEnvironmentRolesAsync(CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.ListEnvironmentRolesInternalAsync(cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<EnvironmentRoleModel>> GetEnvironmentRoleAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.GetEnvironmentRoleInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id | ReferenceKinds.Codename), cancellationToken);
        return await response.ToManagementResultAsync();
    }
}
