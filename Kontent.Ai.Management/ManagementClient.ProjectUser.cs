using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Users;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public Task<IManagementResult<UserModel>> InviteUserIntoEnvironmentAsync(UserInviteModel invitation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(invitation);

        return _managementApi.InviteUserIntoEnvironmentInternalAsync(invitation, cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public Task<IManagementResult<UserModel>> ModifyUsersRolesAsync(UserIdentifier identifier, UserModel user, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(user);

        return _managementApi.ModifyUsersRolesInternalAsync(identifier.ToUrlSegment(), user, cancellationToken).ToManagementResultAsync();
    }
}
