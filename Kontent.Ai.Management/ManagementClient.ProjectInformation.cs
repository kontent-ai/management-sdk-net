using Kontent.Ai.Management.Models.EnvironmentReport;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<Environment> GetEnvironmentInformationAsync()
    {
        var endpointUrl = _urlBuilder.BuildEnvironmentUrl();
        return await _actionInvoker.InvokeReadOnlyMethodAsync<Environment>(endpointUrl, HttpMethod.Get);
    }
}
