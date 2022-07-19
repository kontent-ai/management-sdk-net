using Kontent.Ai.Management.Models.ProjectReport;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<Project> GetProjectInformationAsync()
    {
        var endpointUrl = _urlBuilder.BuildProjectUrl();
        return await _actionInvoker.InvokeReadOnlyMethodAsync<Project>(endpointUrl, HttpMethod.Get);
    }
}
