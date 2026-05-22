using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Users;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IManagementResult<UserModel>> InviteUserIntoEnvironmentAsync(UserInviteModel invitation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(invitation);

        var response = await _managementApi.InviteUserIntoEnvironmentInternalAsync(invitation, cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<UserModel>> ModifyUsersRolesAsync(UserIdentifier identifier, UserModel user, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(user);

        var response = await _managementApi.ModifyUsersRolesInternalAsync(UserSegment(identifier), user, cancellationToken);
        return await response.ToManagementResultAsync();
    }

    // A user is addressed by id or email — `{id}` or `email/{email}` (matches the legacy UserTemplate; Refit url-encodes the value).
    private static string UserSegment(UserIdentifier identifier)
    {
        if (identifier.Id is not null)
        {
            return identifier.Id;
        }

        if (!string.IsNullOrEmpty(identifier.Email))
        {
            return $"email/{identifier.Email}";
        }

        throw new ArgumentException("You must provide user id or email");
    }
}
