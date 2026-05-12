using Environment = Kontent.Ai.Management.Models.EnvironmentReport.Environment;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<Environment> GetEnvironmentInformationAsync()
        => EnsureSuccess(await _managementApi.GetEnvironmentInformationInternalAsync());
}
