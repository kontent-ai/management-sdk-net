using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.WebSpotlight;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IManagementResult<WebSpotlightModel>> ActivateWebSpotlightAsync(WebSpotlightActivateModel webSpotlightActivateModel, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(webSpotlightActivateModel);

        var response = await _managementApi.ActivateWebSpotlightInternalAsync(webSpotlightActivateModel, cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<WebSpotlightModel>> DeactivateWebSpotlightAsync(CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.DeactivateWebSpotlightInternalAsync(cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<WebSpotlightModel>> GetWebSpotlightStatusAsync(CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.GetWebSpotlightStatusInternalAsync(cancellationToken);
        return await response.ToManagementResultAsync();
    }
}
