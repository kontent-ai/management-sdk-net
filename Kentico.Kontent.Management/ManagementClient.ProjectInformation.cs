using Kentico.Kontent.Management.Models.ProjectReport;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kentico.Kontent.Management;

public partial class ManagementClient : IManagementClient
{
    /// <inheritdoc />
    public async Task<Project> GetProjectInformationAsync()
    {
        var endpointUrl = _urlBuilder.BuildProjectUrl();
        return await _actionInvoker.InvokeReadOnlyMethodAsync<Project>(endpointUrl, HttpMethod.Get);
    }
}
