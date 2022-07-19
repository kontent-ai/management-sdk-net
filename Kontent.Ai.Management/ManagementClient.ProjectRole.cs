using Kontent.Ai.Management.Models.Roles;
using Kontent.Ai.Management.Models.Shared;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<ProjectRolesModel> ListProjectRolesAsync()
    {
        var endpointUrl = _urlBuilder.BuildProjectRolesUrl();
        return await _actionInvoker.InvokeReadOnlyMethodAsync<ProjectRolesModel>(endpointUrl, HttpMethod.Get);
    }

    /// <inheritdoc />
    public async Task<ProjectRoleModel> GetProjectRoleAsync(Reference identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildProjectRoleUrl(identifier);
        return await _actionInvoker.InvokeReadOnlyMethodAsync<ProjectRoleModel>(endpointUrl, HttpMethod.Get);
    }
}
