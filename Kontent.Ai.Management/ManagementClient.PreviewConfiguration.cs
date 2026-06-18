using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.PreviewConfiguration;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IManagementResult<PreviewConfigurationModel>> GetPreviewConfigurationAsync(CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.GetPreviewConfigurationInternalAsync(cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<PreviewConfigurationModel>> ModifyPreviewConfigurationAsync(PreviewConfigurationModel previewConfiguration, CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.ModifyPreviewConfigurationInternalAsync(previewConfiguration, cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }
}
