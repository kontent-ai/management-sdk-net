using Kontent.Ai.Management.Extensions;
using Environment = Kontent.Ai.Management.Models.EnvironmentReport.Environment;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IManagementResult<Environment>> GetEnvironmentInformationAsync(CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.GetEnvironmentInformationInternalAsync(cancellationToken);
        return await response.ToManagementResultAsync();
    }
}
