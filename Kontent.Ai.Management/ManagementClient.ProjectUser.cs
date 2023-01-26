using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Users;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<UserModel> InviteUserIntoProjectAsync(UserInviteModel invitation)
    {
        if (invitation == null)
        {
            throw new ArgumentNullException(nameof(invitation));
        }

        var endpointUrl = _urlBuilder.BuildUsersUrl();
        return await _actionInvoker.InvokeMethodAsync<UserInviteModel, UserModel>(endpointUrl, HttpMethod.Post, invitation);
    }

    /// <inheritdoc />
    public async Task<UserModel> ModifyUsersRolesAsync(UserIdentifier identifier, UserModel user)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var endpointUrl = _urlBuilder.BuildModifyUsersRoleUrl(identifier);
        return await _actionInvoker.InvokeMethodAsync<UserModel, UserModel>(endpointUrl, HttpMethod.Put, user);
    }
}
