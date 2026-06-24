using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Environments;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IManagementResult<EnvironmentInformationModel>> GetEnvironmentInformationAsync(CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.GetEnvironmentInformationInternalAsync(cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }
}
