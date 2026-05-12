using Kontent.Ai.Management.Models.PreviewConfiguration;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<PreviewConfigurationModel> GetPreviewConfigurationAsync()
        => EnsureSuccess(await _managementApi.GetPreviewConfigurationInternalAsync());

    /// <inheritdoc />
    public async Task<PreviewConfigurationModel> ModifyPreviewConfigurationAsync(PreviewConfigurationModel previewConfiguration)
        => EnsureSuccess(await _managementApi.ModifyPreviewConfigurationInternalAsync(previewConfiguration));
}
