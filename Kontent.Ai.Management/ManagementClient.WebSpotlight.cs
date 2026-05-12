using Kontent.Ai.Management.Models.WebSpotlight;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<WebSpotlightModel> ActivateWebSpotlightAsync(WebSpotlightActivateModel webSpotlightActivateModel)
    {
        ArgumentNullException.ThrowIfNull(webSpotlightActivateModel);

        return EnsureSuccess(await _managementApi.ActivateWebSpotlightInternalAsync(webSpotlightActivateModel));
    }

    /// <inheritdoc />
    public async Task<WebSpotlightModel> DeactivateWebSpotlightAsync()
        => EnsureSuccess(await _managementApi.DeactivateWebSpotlightInternalAsync());

    /// <inheritdoc />
    public async Task<WebSpotlightModel> GetWebSpotlightStatusAsync()
        => EnsureSuccess(await _managementApi.GetWebSpotlightStatusInternalAsync());
}
