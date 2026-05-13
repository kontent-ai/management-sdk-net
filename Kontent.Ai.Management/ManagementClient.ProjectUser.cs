using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Users;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<UserModel> InviteUserIntoEnvironmentAsync(UserInviteModel invitation)
    {
        ArgumentNullException.ThrowIfNull(invitation);

        return EnsureSuccess(await _managementApi.InviteUserIntoEnvironmentInternalAsync(invitation));
    }

    /// <inheritdoc />
    public async Task<UserModel> ModifyUsersRolesAsync(UserIdentifier identifier, UserModel user)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(user);

        return EnsureSuccess(await _managementApi.ModifyUsersRolesInternalAsync(UserSegment(identifier), user));
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
