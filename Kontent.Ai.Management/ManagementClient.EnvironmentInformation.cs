using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Environments;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public Task<IManagementResult<EnvironmentInformationModel>> GetEnvironmentInformationAsync(CancellationToken cancellationToken = default)
    {
        return _managementApi.GetEnvironmentInformationInternalAsync(cancellationToken).ToManagementResultAsync();
    }
}
