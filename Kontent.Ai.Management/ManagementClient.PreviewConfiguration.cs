using Kontent.Ai.Management.Models.PreviewConfiguration;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<PreviewConfigurationResponseModel> GetPreviewConfigurationAsync()
    {
        var endpointUrl = _urlBuilder.BuildPreviewConfigurationUrl();
        return await _actionInvoker.InvokeReadOnlyMethodAsync<PreviewConfigurationResponseModel>(endpointUrl, HttpMethod.Get);
    }

    /// <inheritdoc />
    public async Task<PreviewConfigurationResponseModel> UpdatePreviewConfigurationAsync(PreviewConfigurationRequestModel previewConfiguration)
    {
        var endpointUrl = _urlBuilder.BuildPreviewConfigurationUrl();
        return await _actionInvoker.InvokeMethodAsync<PreviewConfigurationRequestModel, PreviewConfigurationResponseModel >(endpointUrl, HttpMethod.Put, previewConfiguration);
    }
}
