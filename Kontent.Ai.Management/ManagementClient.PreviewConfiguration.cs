using Kontent.Ai.Management.Models.PreviewConfiguration;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<PreviewConfigurationModel> GetPreviewConfigurationAsync()
    {
        var endpointUrl = _urlBuilder.BuildPreviewConfigurationUrl();
        return await _actionInvoker.InvokeReadOnlyMethodAsync<PreviewConfigurationModel>(endpointUrl, HttpMethod.Get);
    }

    /// <inheritdoc />
    public async Task<PreviewConfigurationModel> UpdatePreviewConfigurationAsync(PreviewConfigurationModel previewConfiguration)
    {
        var endpointUrl = _urlBuilder.BuildPreviewConfigurationUrl();
        return await _actionInvoker.InvokeMethodAsync<PreviewConfigurationModel, PreviewConfigurationModel >(endpointUrl, HttpMethod.Put, previewConfiguration);
    }
}
