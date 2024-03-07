using Kontent.Ai.Management.Models.Roles;
using Kontent.Ai.Management.Models.Shared;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<EnvironmentRolesModel> ListEnvironmentRolesAsync()
    {
        var endpointUrl = _urlBuilder.BuildEnvironmentRolesUrl();
        return await _actionInvoker.InvokeReadOnlyMethodAsync<EnvironmentRolesModel>(endpointUrl, HttpMethod.Get);
    }

    /// <inheritdoc />
    public async Task<EnvironmentRoleModel> GetEnvironmentRoleAsync(Reference identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildEnvironmentRoleUrl(identifier);
        return await _actionInvoker.InvokeReadOnlyMethodAsync<EnvironmentRoleModel>(endpointUrl, HttpMethod.Get);
    }
}
