using Kontent.Ai.Management.Models.ProjectReport;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<ProjectReportModel> ValidateProjectAsync()
    {
        var endpointUrl = _urlBuilder.BuildValidationUrl();
        return await _actionInvoker.InvokeReadOnlyMethodAsync<ProjectReportModel>(endpointUrl, HttpMethod.Post);
    }
}
