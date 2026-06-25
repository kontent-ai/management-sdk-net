using Kontent.Ai.Management.Models.Roles;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Lists the roles in the environment.</summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/roles")]
    internal Task<IApiResponse<EnvironmentRolesModel>> ListEnvironmentRolesInternalAsync(
        CancellationToken cancellationToken = default);

    /// <summary>Gets a single role.</summary>
    /// <param name="identifier">The role identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference, ReferenceKinds)"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/roles/{**identifier}")]
    internal Task<IApiResponse<EnvironmentRoleModel>> GetEnvironmentRoleInternalAsync(
        string identifier,
        CancellationToken cancellationToken = default);
}
