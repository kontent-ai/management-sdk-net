using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Models.Roles;
using Kontent.Ai.Management.Models.Shared;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<EnvironmentRolesModel> ListEnvironmentRolesAsync()
        => EnsureSuccess(await _managementApi.ListEnvironmentRolesInternalAsync());

    /// <inheritdoc />
    public async Task<EnvironmentRoleModel> GetEnvironmentRoleAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return EnsureSuccess(await _managementApi.GetEnvironmentRoleInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id | ReferenceKinds.Codename)));
    }
}
