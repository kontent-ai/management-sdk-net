using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Roles;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IManagementResult<IReadOnlyList<EnvironmentRoleModel>>> ListEnvironmentRolesAsync(CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.ListEnvironmentRolesInternalAsync(cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync(static envelope => envelope.Roles).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<EnvironmentRoleModel>> GetEnvironmentRoleAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.GetEnvironmentRoleInternalAsync(identifier.ToUrlSegment(), cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }
}
