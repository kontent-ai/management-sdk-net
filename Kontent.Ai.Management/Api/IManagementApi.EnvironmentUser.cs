using Kontent.Ai.Management.Models.Users;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Invites a user into the environment.</summary>
    /// <param name="invitation">The invitation.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Post("/users")]
    internal Task<IApiResponse<UserModel>> InviteUserIntoEnvironmentInternalAsync(
        [Body] UserInviteModel invitation,
        CancellationToken cancellationToken = default);

    /// <summary>Replaces the roles of a user in the environment.</summary>
    /// <param name="userIdentifier">The user identifier path segment (<c>{id}</c> or <c>email/{email}</c>).</param>
    /// <param name="roles">The roles to set.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/users/{**userIdentifier}/roles")]
    internal Task<IApiResponse<UserModel>> UpdateUserRolesInternalAsync(
        string userIdentifier,
        [Body] UserRolesUpdateModel roles,
        CancellationToken cancellationToken = default);
}
